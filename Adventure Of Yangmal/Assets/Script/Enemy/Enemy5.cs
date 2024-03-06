using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{

    public List<Vector2> _Directions = new List<Vector2>();

    // enemy5 움직임 재정의
    public override void Move()
    {
        base.Move();
        // 직각이동을 위한 방향 재정의
        ResetDirection(_ai._direction);
        // 움직임
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    void Awake()
    {
        // 적 체력, 대미지, 스피드, 감지범위 설정
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
        // 죽음 검사
        IsDead();

        // 스턴 상태 진입 검사
        if (_isStun && _curState != eENEMY_STATE.DIE)
        {
            _curState = eENEMY_STATE.STUN;
        }

        // 플레이어 죽음 검사 --  플레이어가 죽음 상태일때 STROLL 상태 진입
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;

        switch (_curState)
        {
            // 1) STROLL 상태
            case (eENEMY_STATE.STROLL):
                // 플레이어가 범위 안에 들어왔는지 확인
                // 적 상태가 DIE 인지 확인
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                {
                    // FOLLOW상태 진입
                    _curState = eENEMY_STATE.FOLLOW;
                }
                _ai.Stroll();
                break;

            // 2 ) FOLLOW 상태
            case (eENEMY_STATE.FOLLOW):
                // 플레이어가 범위 안에 들어있는지 계속 확인
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _ai.Follow();

                // 플레이어가 범위 밖으로 나간다면 STROLL
                else if (!IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _curState = eENEMY_STATE.STROLL;
                break;

            // 3 ) STUN 상태
            case (eENEMY_STATE.STUN):
                if (!_isDoingStun) // 스턴 코루틴이 시작되지 않았다면
                    StartCoroutine(Stun());
                // 스턴 코루틴이 진행중이라면 호출하지 않는다.
                break;


            // 4) DIE 상태
            case (eENEMY_STATE.DIE):
                // 죽음 코루틴이 실행하지 않은 상태라면
                if (!_isDoingDie)
                {
                    // 움직임 멈추고 죽음 코루틴 시작
                    _ai.StopMove();
                    DestroyTrail(GetComponent<Enemy5trail>()._trail);
                    StartCoroutine(Die());
                }
                break;
        }//switch(_curState)
    }




    // 직각이동을 위한 방향 재정의
    void ResetDirection(Vector2 originDir)
    {
        // 1) x = 0 을 포함한 1,4 사분면
        if (originDir.x >= 0 && originDir.x <= 1)
        {
            // originDir이 1사분면일 때
            // y = x 직선을 기준으로 나눔
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

            // originDir이 4사분면일때
            // y = -x 직선을 기준으로 나눔
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

        // 2) x = 0 을 포함하는  2,3사분면
        else if (originDir.x >= -1 && originDir.x <=0)
        {
            // originDir이 2사분면일 때
            // y = -x 직선을 기준으로 나눔
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

            // originDir이 3사분면일때
            // y = x 직선을 기준으로 나눔
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


    // y =x, y = -x와 일치했을 시 랜덤방향 정의를 위함
    void RandomDir()
    {
        int dirNum = Random.Range(0, 2);
        _ai._direction = _Directions[dirNum];
    }

    // 사망시 리스트 내부의 trail 모두 삭제 
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
