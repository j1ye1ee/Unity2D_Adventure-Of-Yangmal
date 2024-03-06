using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    float _jumpTime;
    float _setJumpTime = 0.3f;
    public bool _jumpOn;
    public float _jumpSpeed;

    // enemy3 ������ ������
    public override void Move()
    {
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    
    void Awake()
    {
        // �� ü��, �����, ���ǵ�, �������� ����
        SetEnemy(setHp, setDamage, setSpeed, setRange);

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

    private void FixedUpdate()
    {
        // enemy3�� ����ִ� ���¶��
        if (_curState != eENEMY_STATE.DIE)
        {
            // ���� ��Ÿ���� ����
            if (_jumpTime >= _setJumpTime)
            {
                // ����
                if (_jumpOn)
                {
                    // ���� �����Ͻô� ���� x
                    if (_curState == eENEMY_STATE.STUN)
                        return;
                    else
                    {
                        Vector2 jumpPosition = new Vector2(transform.position.x, transform.position.y + 10f);
                        _myRigid.transform.position = Vector2.Lerp(transform.position, jumpPosition, Time.deltaTime * _jumpSpeed);
                    }

                    _jumpTime = 0f;
                    _jumpOn = false;
                }//if (_jumpTime >= _setJumpTime)

                // �ٿ�
                else if (!_jumpOn)
                {
                    // ���� �����Ͻô� �ٿ� x
                    if (_curState == eENEMY_STATE.STUN)
                        return;
                    else
                    {
                        Vector2 jumpPosition = new Vector2(transform.position.x, transform.position.y - 10f);
                        _myRigid.transform.position = Vector2.Lerp(transform.position, jumpPosition, Time.deltaTime * _jumpSpeed);
                    }

                    _jumpTime = 0f;
                    _jumpOn = true;
                }//else if (!_jumpOn)
            }//if (_jumpTime >= _setJumpTime)

            _jumpTime += Time.deltaTime;

        }//if(_curState != eENEMY_STATE.DIE)
    }



    void Update()
    {
        // ���� �˻�
        IsDead();

        // ���� ���� ���� �˻�
        if (_isStun && _curState != eENEMY_STATE.DIE)
            _curState = eENEMY_STATE.STUN;


        // �÷��̾� ���� �˻� --  �÷��̾ ���� �����϶� STROLL ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;


        // ���¸ӽ�
        switch (_curState)
        {
            // 1) STROLL ����
            case (eENEMY_STATE.STROLL):
                // �÷��̾ ���� �ȿ� ���Դ��� Ȯ��
                // �� ���°� DIE ���� Ȯ��
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                {
                    // FOLLOW���� ����
                    _curState = eENEMY_STATE.FOLLOW;
                }
                _ai.Stroll();
                break;

            // 2 ) FOLLOW ����
            case (eENEMY_STATE.FOLLOW):
                // �÷��̾ ���� �ȿ� ����ִ��� ��� Ȯ��
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _ai.Follow();

                // �÷��̾ ���� ������ �����ٸ� STROLL
                else if (!IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _curState = eENEMY_STATE.STROLL;
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

}
