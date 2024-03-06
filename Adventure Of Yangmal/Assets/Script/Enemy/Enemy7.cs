using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy7 : Enemy
{

    public float _followSpeed;
    public float _checkFollowTime;
    public float _startFollowtime;
    public float _checkPauseTime;
    public float _pauseTime;

    public bool _isDoingFollow = false;
    public bool _isDoingShake = false;
    

    //  enemy7 ������ ������
    public override void Move()
    {
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    void Awake()
    {
        // enemy7 ó��
        _isEnemy7 = true;
        // �� ü��, �����, ���ǵ�, �������� ����
        SetEnemy(setHp, setDamage, setSpeed, setRange);

        // ĳ��
        _ai = GetComponent<EnemyMoveAi>();
        _myRigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerStatus = PlayerStatus.Instance;
    }

    void Update()
    {
        // ���� �˻�
        IsDead();

        // ���� ���� ���� �˻� , FOLLOW �߿���  ���� �ȵ�
        if (_isStun && _curState != eENEMY_STATE.DIE)
        {
            if(_curState != eENEMY_STATE.FOLLOW)
                _curState = eENEMY_STATE.STUN;
        }

        // �÷��̾� ���� �˻� --  �÷��̾ ���� �����϶� STROLL ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;


        switch (_curState)
        {
            // 1) STROLL ����
            case (eENEMY_STATE.STROLL):
                _speed = setSpeed;
                // follow time üũ
                _checkFollowTime += Time.deltaTime;
                // follow ���� ���� �ð��� �Ǿ��ٸ� FOLLOW & follow time ����
                if (_checkFollowTime >= _startFollowtime && _curState != eENEMY_STATE.DIE)
                {
                    _curState = eENEMY_STATE.FOLLOW;
                    _checkFollowTime = 0f;
                }
                // follow ���� ���� �ð��� ���� �ʾҴٸ� Storll
                else
                    _ai.Stroll();
                break;

            // 2 ) FOLLOW ����
            case (eENEMY_STATE.FOLLOW):
                // ���� ���� ���� �ð��̶��
                if (_checkPauseTime < _pauseTime)
                {
                    // ����
                    _myRigid.velocity = new Vector2(0, 0);
                    // ���� ���� �ð� üũ
                    _checkPauseTime += Time.deltaTime;
                    // shake ȿ��
                    if (!_isDoingShake)
                        StartCoroutine(Shake());
                }

                // ���� ���� ���� �ð��� �����ٸ� FOLLOW �ڷ�ƾ ����
                if(_checkPauseTime >= _pauseTime)
                {
                    // Shake���� �� Follow �뽬 �� ���� ������ ����
                    if (!_isDoingShake&&!_isDoingFollow)
                        StartCoroutine(Enemy6_Follow());

                    // ���� �� �浹 �˻�
                    _ai.isWall();

                    // ���� �� ���� �浹 �� �浹 ȸ�� & STROLL ���� ����
                    if (_ai._isWall)
                    {
                        StartCoroutine(_ai.ReturnFromWall());
                        _curState = eENEMY_STATE.STROLL;
                        break;
                    }
                    
                }
                break;

            // 3 ) STUN ����
            case (eENEMY_STATE.STUN):
                if (!_isDoingStun) // ���� �ڷ�ƾ�� ���۵��� �ʾҴٸ�
                    StartCoroutine(Stun());
                // ���� �ڷ�ƾ�� �������̶�� ȣ������ �ʴ´�.
                break;

            // 4) DIE ����
            case (eENEMY_STATE.DIE):
                // ���� �ڷ�ƾ�� �������� ���� ���¶��
                if (!_isDoingDie)
                {
                    // ������ ���߰� ���� �ڷ�ƾ ����
                    _ai.StopMove();
                    StartCoroutine(Die());
                }
                break;
        }//switch(_curState)

    }

    // Enemy6 follow ���� ����
    IEnumerator Enemy6_Follow()
    {
        _isDoingFollow = true;

        // 0) STROLL ������ �ڷ�ƾ ��� ���� & _speed =  follow �ӵ��� ����
        _ai.StopAllCoroutines();
        _speed = _followSpeed;

        // 1) ���� ���� ����
        _ai._direction = (_ai._playerTrsf.position - _ai.transform.position).normalized;

        // 2) ���� �������� �߻�
        _myRigid.AddForce(_ai._direction * _followSpeed, ForceMode2D.Impulse);

        // 3) STROLL ���ͱ��� ���
        yield return new WaitForSeconds(2f);


        // 4) �������� ������ ����
        _ai._chooseDir = false;

        // 5) checkfollowTime & _isDoingFollow���� & Stroll ���� ����
        _curState = eENEMY_STATE.STROLL;
        _checkPauseTime = 0f;
        _isDoingFollow = false;
        _isDoingShake = false;
    }


    // �¿� shake ȿ��
    IEnumerator Shake()
    {
        _isDoingShake = true;
        Vector2 origin = _myRigid.position;
        Vector2 random = new Vector2(0f,0f);
        Vector2 proRandom = new Vector2(0f,0f);

        // ���� �ð� ���� shake �� ����
        while (_checkPauseTime < _pauseTime - 0.25f)
        {
            // 1 ) Shake ���� ����
            // ������ �������� �������ٸ� ����������
            if (proRandom.x < 0)
                random = new Vector2(Random.Range(0f, 0.25f), 0f);

            // ������ ���������� �������ٸ� ��������
            else if (proRandom.x > 0)
                random = new Vector2(Random.Range(-0.25f, 0f), 0f);

            // ������ �������� �ʾҴٸ� �¿� �� ����������
            else if (proRandom.x == 0)
                random = new Vector2(Random.Range(-0.25f, 0.25f), 0f);

            // 2 ) Shake ���� ��ŭ �̵�
            _myRigid.position = Vector2.Lerp(origin,origin + random,0.5f);

            // 3 ) �̵��� ���� proRandom �� ����
            proRandom = random;

            // 4 ) ���� �����ӱ��� ���
            yield return null;
        }
        // 5 ) ������Ʈ ��ġ ���󺹱�
        _myRigid.position = origin;
        _isDoingShake = false;
    }

    // CollisionEnter2D ������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" &&  _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // �÷��̾�� ������ ����
            _playerStatus.PlayerGetDamage(_damage);
            Debug.Log("���� �浹, �÷��̾� ü�� : " + _playerStatus._hp);

            // �÷��̾� ��� �� ���� ������ ����
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
        }
        
    }

    // trigger ���¿��� player���� �浹���� ��� ��
    private void OnTriggerExit2D(Collider2D other)
    {
        // �÷��̾���� �浹���� ����ų� �÷��̾��� ���°� ���� �̶�� ������ trigger ���� ���󺹱�
        if (other.tag == "Player" || _playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        }
    }

    // enemy7 �÷��̾� �Ѿ˰� �浹��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �Ѿ˰� �浹�� & Follow ���°� �ƴ϶��
        if (other.tag == "Player Bullet" && _curState != eENEMY_STATE.FOLLOW)
        {
            // ���ϻ��� ����
            _isStun = true;

        }
    }

}
