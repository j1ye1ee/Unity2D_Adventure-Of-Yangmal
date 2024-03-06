using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{

    [SerializeField]
    float _rotateSpeed;
    

    // enemy2 ������ ������
    public override void Move()
    {
        base.Move();
        _myRigid.velocity = _ai._direction.normalized * _speed;

    }

    // ���� ������
    void SpinDonut()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.fixedDeltaTime));
    }
    
    private void Awake()
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

    void Update()
    {
        //���� ������
        SpinDonut();

        // ���� �˻�
        IsDead();

        // ���� ���� ���� �˻�
        if (_isStun && _curState != eENEMY_STATE.DIE)
        {
            _curState = eENEMY_STATE.STUN;
        }

        // �÷��̾� ���� �˻� --  �÷��̾ ���� �����϶� STROLL ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;

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
