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
    

    //  enemy7 움직임 재정의
    public override void Move()
    {
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    void Awake()
    {
        // enemy7 처리
        _isEnemy7 = true;
        // 적 체력, 대미지, 스피드, 감지범위 설정
        SetEnemy(setHp, setDamage, setSpeed, setRange);

        // 캐싱
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

        // 스턴 상태 진입 검사 , FOLLOW 중에는  스턴 안됨
        if (_isStun && _curState != eENEMY_STATE.DIE)
        {
            if(_curState != eENEMY_STATE.FOLLOW)
                _curState = eENEMY_STATE.STUN;
        }

        // 플레이어 죽음 검사 --  플레이어가 죽음 상태일때 STROLL 상태 진입
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;


        switch (_curState)
        {
            // 1) STROLL 상태
            case (eENEMY_STATE.STROLL):
                _speed = setSpeed;
                // follow time 체크
                _checkFollowTime += Time.deltaTime;
                // follow 상태 진입 시간이 되었다면 FOLLOW & follow time 리셋
                if (_checkFollowTime >= _startFollowtime && _curState != eENEMY_STATE.DIE)
                {
                    _curState = eENEMY_STATE.FOLLOW;
                    _checkFollowTime = 0f;
                }
                // follow 상태 진입 시간이 되지 않았다면 Storll
                else
                    _ai.Stroll();
                break;

            // 2 ) FOLLOW 상태
            case (eENEMY_STATE.FOLLOW):
                // 정지 상태 유지 시간이라면
                if (_checkPauseTime < _pauseTime)
                {
                    // 정지
                    _myRigid.velocity = new Vector2(0, 0);
                    // 정지 상태 시간 체크
                    _checkPauseTime += Time.deltaTime;
                    // shake 효과
                    if (!_isDoingShake)
                        StartCoroutine(Shake());
                }

                // 정지 상태 유지 시간이 끝났다면 FOLLOW 코루틴 시작
                if(_checkPauseTime >= _pauseTime)
                {
                    // Shake종료 후 Follow 대쉬 한 번만 실행을 위함
                    if (!_isDoingShake&&!_isDoingFollow)
                        StartCoroutine(Enemy6_Follow());

                    // 추적 중 충돌 검사
                    _ai.isWall();

                    // 추적 중 벽과 충돌 시 충돌 회피 & STROLL 상태 진입
                    if (_ai._isWall)
                    {
                        StartCoroutine(_ai.ReturnFromWall());
                        _curState = eENEMY_STATE.STROLL;
                        break;
                    }
                    
                }
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

    // Enemy6 follow 새로 제작
    IEnumerator Enemy6_Follow()
    {
        _isDoingFollow = true;

        // 0) STROLL 상태의 코루틴 모두 중지 & _speed =  follow 속도로 설정
        _ai.StopAllCoroutines();
        _speed = _followSpeed;

        // 1) 직진 방향 세팅
        _ai._direction = (_ai._playerTrsf.position - _ai.transform.position).normalized;

        // 2) 직진 방향으로 발사
        _myRigid.AddForce(_ai._direction * _followSpeed, ForceMode2D.Impulse);

        // 3) STROLL 복귀까지 대기
        yield return new WaitForSeconds(2f);


        // 4) 랜덤방향 설정을 위함
        _ai._chooseDir = false;

        // 5) checkfollowTime & _isDoingFollow리셋 & Stroll 상태 진입
        _curState = eENEMY_STATE.STROLL;
        _checkPauseTime = 0f;
        _isDoingFollow = false;
        _isDoingShake = false;
    }


    // 좌우 shake 효과
    IEnumerator Shake()
    {
        _isDoingShake = true;
        Vector2 origin = _myRigid.position;
        Vector2 random = new Vector2(0f,0f);
        Vector2 proRandom = new Vector2(0f,0f);

        // 정지 시간 동안 shake 를 위함
        while (_checkPauseTime < _pauseTime - 0.25f)
        {
            // 1 ) Shake 방향 설정
            // 이전에 왼쪽으로 움직였다면 오른쪽으로
            if (proRandom.x < 0)
                random = new Vector2(Random.Range(0f, 0.25f), 0f);

            // 이전에 오른쪽으로 움직였다면 왼쪽으로
            else if (proRandom.x > 0)
                random = new Vector2(Random.Range(-0.25f, 0f), 0f);

            // 이전에 움직이지 않았다면 좌우 중 랜덤값으로
            else if (proRandom.x == 0)
                random = new Vector2(Random.Range(-0.25f, 0.25f), 0f);

            // 2 ) Shake 방향 만큼 이동
            _myRigid.position = Vector2.Lerp(origin,origin + random,0.5f);

            // 3 ) 이동한 방향 proRandom 에 저장
            proRandom = random;

            // 4 ) 다음 프레임까지 대기
            yield return null;
        }
        // 5 ) 오브젝트 위치 원상복구
        _myRigid.position = origin;
        _isDoingShake = false;
    }

    // CollisionEnter2D 재정의
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" &&  _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // 플레이어에게 데미지 입힘
            _playerStatus.PlayerGetDamage(_damage);
            Debug.Log("적과 충돌, 플레이어 체력 : " + _playerStatus._hp);

            // 플레이어 통과 및 투명도 반으로 낮춤
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
        }
        
    }

    // trigger 상태에서 player와의 충돌에서 벗어날 때
    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어와의 충돌에서 벗어나거나 플레이어의 상태가 죽음 이라면 투명도와 trigger 상태 원상복구
        if (other.tag == "Player" || _playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        }
    }

    // enemy7 플레이어 총알과 충돌시
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌시 & Follow 상태가 아니라면
        if (other.tag == "Player Bullet" && _curState != eENEMY_STATE.FOLLOW)
        {
            // 스턴상태 진입
            _isStun = true;

        }
    }

}
