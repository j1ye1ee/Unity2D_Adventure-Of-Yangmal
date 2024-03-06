using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{

    [SerializeField]
    float _rotateSpeed;
    

    // enemy2 움직임 재정의
    public override void Move()
    {
        base.Move();
        _myRigid.velocity = _ai._direction.normalized * _speed;

    }

    // 도넛 굴리기
    void SpinDonut()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.fixedDeltaTime));
    }
    
    private void Awake()
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
        //도넛 굴리기
        SpinDonut();

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
                    StartCoroutine(Die());
                }
                break;
        }//switch(_curState)

    }
}
