using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
// ��� ������ ����� �Ӽ� Ŭ����
{
    public enum eENEMY_STATE
    {
        STROLL, //�÷��̾� �߰� ��
        FOLLOW, //�÷��̾� �߰�
        STUN, //����
        DIE, // ����
    }

    public eENEMY_STATE _curState = eENEMY_STATE.STROLL;

    // �� ������ Ŭ���� ����
    protected EnemyMoveAi _ai;
    public Rigidbody2D _myRigid;

    // �÷��̾� ������ ����
    public GameObject _player;

    // �÷��̾�� ������� �ֱ� ����
    public PlayerStatus _playerStatus;

    // ���� �ڷ�ƾ�� ����
    public SpriteRenderer _spriteRenderer;
    public bool _isDoingDie = false;
    public Color _originColor;

    // �� Ŭ���� ����� ����
    public bool _isDead = false;
    public float _hp = 100;
    public float _damage;
    public float _speed;
    public float _range;
    public float _stunSpeed;

    public bool _isStun = false;
    public bool _isDoingStun = false;

    //SetEnemy�� ���� ����

    public float setHp;
    public float setDamage;
    public float setSpeed;
    public float setRange;

    //enemy7�� Ư�� ���̽�
    public bool _isEnemy7 = false;

    public AudioSource _damageSound;


    // �� ������ �پ��� ���������� �ʱ�ȭ�� ���� �޼���
    public void SetEnemy(float hp, float damage, float speed, float range)
    {
        _hp = hp;
        _damage = damage;
        _speed = speed;
        _range = range;
        _stunSpeed = _speed - 3f;
    }

    // ������ 
    public virtual void Move()
    { }

    // ����� �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }


    // �÷��̾�&������ �Ÿ� üũ 
    protected bool IsPlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, _player.transform.position) <= range;
    }

    // ���� �˻�
    protected void IsDead()
    {
        if(_hp <= 0)
        {
            // ���� ó��
            _isDead = true;
            _curState = eENEMY_STATE.DIE;
            _hp = 0;

            // �浹�� ���� �÷��̾� ������ ����
            gameObject.GetComponent<Collider2D>().enabled = false;
            
        }
    }

    // �� ���� �ڷ�ƾ
    protected IEnumerator Die()
    {
        // ���� �ڷ�ƾ �� �� ������ ����
        _isDoingDie = true;
        float _a = 1f;

        // ���̵� �ƿ� 
        while(_a >= 0)
        {
            _a -= 0.05f;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // ������ ���
        Quaternion itemRotation = Quaternion.Euler(0, 0, 0);
        EnemyItemDrop enemyItem = GetComponent<EnemyItemDrop>();

        if (enemyItem._dropItem != null)
        {
            GameObject Item =
                Instantiate(enemyItem._dropItem, transform.position, itemRotation);
        }

        // �ڷ�ƾ ���� �� pooling ���� setActive = false & ���� �ʱ�ȭ
        gameObject.SetActive(false);

        // �÷�, hp, ���� �ʱ�ȭ
        if (_isEnemy7)
        {
            gameObject.GetComponent<Enemy7>()._isDoingFollow = false;
            gameObject.GetComponent<Enemy7>()._isDoingShake = false;
        }

        _spriteRenderer.color = _originColor;
        _hp = setHp;
        _curState = eENEMY_STATE.STROLL;
        _isDead = false;
        _isDoingDie = false;
        _isStun = false;
        _isDoingStun = false;
        _speed = setSpeed;
        GetComponent<Collider2D>().enabled = true;

        // ������ ��ũ ��
        if (GetComponent<SpawnerLink>() != null)
            GetComponent<SpawnerLink>()._isMinus = false;
    }

    // ���� �ް� ���ϻ��� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �Ѿ˰� �浹�� & enemy7�� �ƴ϶��
        if (other.tag == "Player Bullet" && !_isEnemy7)
        {
            // ���ϻ��� ����
            _isStun = true;

        }
    }

    // �÷��̾�� �浹�� ������ ������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // �÷��̾�� ������ ����
            _playerStatus.PlayerGetDamage(_damage);
        }
    }


    // �÷��̾�� �浹 ���ӽ�  �÷��̾ ���� ���� ���ϵ��� ó�� , Enemy7 �� ����
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!_isEnemy7)
        {
            if (other.gameObject.tag == "Player" &&
              _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }


    // �÷��̾�� �浹 ������ ���󺹱�, Enemy7�� ����
    private void OnCollisionExit2D(Collision2D other)
    {
        if (!_isEnemy7)
        {
            if (other.gameObject.tag == "Player" &&
                _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.None;
                _myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }


    // ���� �ڷ�ƾ
    protected IEnumerator Stun()
    {

        // ai ��ũ��Ʈ�� ��� �ڷ�ƾ ����
        _ai.StopAllCoroutines();

        // ���� �ڷ�ƾ �ѹ��� ������ ����
        _isDoingStun = true;

        // �����̴� ȿ�� ����
        float _a = 0;

        // ���� �ӵ� ��� �� ���� �ӵ��� �ٲٱ�
        float originSpeed = setSpeed;
        _speed = _stunSpeed;

        // 2�� ������
        for(int count = 0; count <2; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.08f);
        }

        // �ӵ� ���󺹱�
        _speed = originSpeed;
 

        // ���� �ڷ�ƾ �� ���� ������ ����
        _isDoingStun = false;


        // ���� ���� ��� && STROLL ���� ����
        _isStun = false;
        _curState = eENEMY_STATE.STROLL;

        // �������� ������ �� �ֵ��� false  ó��
        _ai._chooseDir = false;

    }

    // �� hp ����
    public void EnemyGetDamage(float damage)
    {
        _damageSound.Play();
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>()._isCameraShakeStart = true;
        _hp -= damage;
    }
}
