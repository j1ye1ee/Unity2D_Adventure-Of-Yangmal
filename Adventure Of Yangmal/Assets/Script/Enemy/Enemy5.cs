using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{

    public List<Vector2> _Directions = new List<Vector2>();

    // enemy5 ������ ������
    public override void Move()
    {
        base.Move();
        // �����̵��� ���� ���� ������
        ResetDirection(_ai._direction);
        // ������
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
    void Update()
    {
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
                    DestroyTrail(GetComponent<Enemy5trail>()._trail);
                    StartCoroutine(Die());
                }
                break;
        }//switch(_curState)
    }




    // �����̵��� ���� ���� ������
    void ResetDirection(Vector2 originDir)
    {
        // 1) x = 0 �� ������ 1,4 ��и�
        if (originDir.x >= 0 && originDir.x <= 1)
        {
            // originDir�� 1��и��� ��
            // y = x ������ �������� ����
            if (originDir.y > 0)
            {
                if (originDir.x == 0 && originDir.y == 1)
                    _ai._direction = Vector2.up;
                else if (originDir.y < originDir.x)
                    _ai._direction = Vector2.right;
                else if (originDir.y > originDir.x)
                    _ai._direction = Vector2.up;
                else if (originDir.y == originDir.x)
                {
                    _Directions.Add(Vector2.up);
                    _Directions.Add(Vector2.right);
                    RandomDir();
                }
            }//if( originDir.y > 0)

            // originDir�� 4��и��϶�
            // y = -x ������ �������� ����
            else if (originDir.y < 0)
            {
                if (originDir.x == 0 && originDir.y == -1)
                    _ai._direction = Vector2.down;
                else if (originDir.y > -1f * originDir.x)
                    _ai._direction = Vector2.right;
                else if (originDir.y < -1f * originDir.x)
                    _ai._direction = Vector2.down;
                else if (originDir.y == -1f * originDir.x)
                {
                    _Directions.Add(Vector2.right);
                    _Directions.Add(Vector2.down);
                    RandomDir();
                }
            }//else if( originDir.y <0)

            else if (originDir.y == 0)
                _ai._direction = Vector2.right;
        }// if (originDir.x >= 0 && originDir.x <= 1)

        // 2) x = 0 �� �����ϴ�  2,3��и�
        else if (originDir.x >= -1 && originDir.x <=0)
        {
            // originDir�� 2��и��� ��
            // y = -x ������ �������� ����
            if (originDir.y > 0)
            {
                if (originDir.x == 0 && originDir.y == 1)
                    _ai._direction = Vector2.up;
                else if (originDir.y < -1f*originDir.x)
                    _ai._direction = Vector2.left;
                else if (originDir.y > -1f*originDir.x)
                    _ai._direction = Vector2.up;
                else if (originDir.y == -1f*originDir.x)
                {
                    _Directions.Add(Vector2.left);
                    _Directions.Add(Vector2.up);
                    RandomDir();
                }
            }//if( originDir.y > 0)

            // originDir�� 3��и��϶�
            // y = x ������ �������� ����
            else if (originDir.y < 0)
            {
                if (originDir.x == 0 && originDir.y == -1)
                    _ai._direction = Vector2.down;
                else if (originDir.y > originDir.x)
                    _ai._direction = Vector2.left;
                else if (originDir.y < originDir.x)
                    _ai._direction = Vector2.down;
                else if (originDir.y == originDir.x)
                {
                    _Directions.Add(Vector2.left);
                    _Directions.Add(Vector2.down);
                    RandomDir();
                }
            }//else if( originDir.y <0)

            else if (originDir.y == 0)
                _ai._direction = Vector2.left;
        }//else if (originDir.x >= -1 && originDir.x <0)

        _Directions.Clear();
    }//void ResetDirection(Vector2 originDir)


    // y =x, y = -x�� ��ġ���� �� �������� ���Ǹ� ����
    void RandomDir()
    {
        int dirNum = Random.Range(0, 2);
        _ai._direction = _Directions[dirNum];
    }

    // ����� ����Ʈ ������ trail ��� ���� 
    void DestroyTrail(List<GameObject> trailList)
    {
        for(int count = 0; count < trailList.Count; count ++)
        {
            if(trailList[count] != null)
                trailList[count].gameObject.SetActive(false);
        }
        trailList.Clear();

    }


}
