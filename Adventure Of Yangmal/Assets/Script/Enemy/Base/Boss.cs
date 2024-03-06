using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
    // ��� ������ ����� �Ӽ� Ŭ����
{
    public enum eBOSS_STATE
    {
        IDLE,
        STUN,
        DIE
    }

    public eBOSS_STATE _curState = eBOSS_STATE.IDLE;

    // ������ ���� ȿ���� ����
    public bool _isDead = false;
    public SpriteRenderer _spriteRenderer;
    public bool _isDoingDie = false;

    // ������ �ɷ�ġ
    public float _hp;
    public float _damage;

    // ����
    public bool _isStun = false;
    public bool _isDoingStun = false;

    // �÷��̾�� ������� �ֱ� ����
    public PlayerStatus _playerStatus;
    public GameObject _player;

    public Rigidbody2D _myRigid;


    // ���� �˻�
    protected void IsDead()
    {
        if (_hp <= 0)
        {
            // ���� ó��
            _isDead = true;
            _curState = eBOSS_STATE.DIE;
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
        while (_a >= 0)
        {
            _a -= 0.05f;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // �ڷ�ƾ ���� �� ������Ʈ ����
        Destroy(gameObject);
    }


    // ���� �ް� ���ϻ��� ���� & �÷��̾�� �浹�� ������ ������(boss isTrigger OO)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �Ѿ˰� �浹��
        if (other.tag == "Player Bullet" )
        {
            // ���ϻ��� ����
            _isStun = true;
        }

        // �÷��̾�� �浹�� ������ ����
        if (other.gameObject.tag == "Player" &&
            _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _playerStatus.PlayerGetDamage(_damage);
        }

    }


    // �÷��̾�� �浹�� ������ ������(boss isTrigger XX)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // �÷��̾�� ������ ����
            _playerStatus.PlayerGetDamage(_damage);
        }
    }


    // �÷��̾�� �浹 ���ӽ�  �÷��̾ ���� ���� ���ϵ��� ó�� (boss isTrigger XX)
    private void OnCollisionStay2D(Collision2D other)
    {
       if (other.gameObject.tag == "Player" &&
              _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        
    }

    // �÷��̾�� �浹 ������ ���󺹱�(boss isTrigger XX)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _myRigid.constraints = RigidbodyConstraints2D.None;
             _myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
         }
        
    }



    // ���� �ڷ�ƾ
    protected IEnumerator Stun()
    {

        // ���� �ڷ�ƾ �ѹ��� ������ ����
        _isDoingStun = true;

        // �����̴� ȿ�� ����
        float _a = 0;


        // 2�� ������
        for (int count = 0; count < 2; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.08f);
        }

        // ���� ����ġ
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);


        // ���� �ڷ�ƾ �� ���� ������ ����
        _isDoingStun = false;


        // ���� ���� ��� && STROLL ���� ����
        _isStun = false;
        _curState = eBOSS_STATE.IDLE;


    }


    // ���ݹ޾����� ������ hp ���� & ī�޶���ŷ
    public void BossGetDamage(float damage)
    {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>()._isCameraShakeStart = true;
        _hp -= damage;
    }


}
