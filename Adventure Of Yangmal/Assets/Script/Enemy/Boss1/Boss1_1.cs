using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1 : Boss
{
    // Boss 1_1 의 행동&생명 관리 클래스


    public enum eBOSS1_1STATE
    {
        STROLL, // 돌아다니기
        STOP, // 잠깐 멈추기
        STEP, // 지형 밟기 쿵
        DIE // 죽음
    }

    public eBOSS1_1STATE _iState = eBOSS1_1STATE.STROLL;

    // stage Manager 참조
    Boss1_StageManager _stageManager;

    // boss 1_1 움직임 클래스 참조
    public Boss1_1Move _move;
    public float _originSpeed;


    // shake 효과
    bool _isDoingShake = false;
    public float _checkShakeTime;
    public float _shakeTime;

    // STEP  상태
    public float _checkBeforStepTime;
    public float _BeforeStepTime;
    public float _downPoint;
    public Vector2 _stepPoint;
    public bool _isDoingStep;
    public float _rate;
    public float _stepSpeed;
    public bool _rateCheck = true;

    // STROLL 상태
    public float _checkStrollTime;
    public float _strollTime;

    // STOP 상태
    public GameObject _shadowPrefab;
    GameObject _shadow;
    public float _shadowDistX;
    public float _shadowDistY;

    // DIE 상태
    public bool _isAllDestroy = false;

    // 이펙트 클래스 참조
    public Boss1_1Effect _effect;

    // 효과음
    public AudioSource _sound;

    private void Awake()
    {
        // 콜라이더는 STEP 상태일때만 on
        GetComponent<Collider2D>().enabled = false;
        _effect = GetComponent<Boss1_1Effect>();
        _move = GetComponent<Boss1_1Move>();
        _myRigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss1_StageManager>();

    }


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerStatus = PlayerStatus.Instance;

        // move 클래스에서 정지 후 다시 움직일때를 위해 origin speed 저장
        _originSpeed = _move._moveSpeed;
    }


    void FixedUpdate()
    {
        // 1) 죽음 검사
        IsDead();

        // 2) 스턴 검사
        if (_isStun && _curState != eBOSS_STATE.DIE)
          _curState = eBOSS_STATE.STUN;


        // 3) 플레이어 죽음 검사 -- 플레이어 죽음 상태일때 STROLL 상태 진입
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE || _player == null)
            _iState = eBOSS1_1STATE.STROLL;
        


        // 4) boss 클래스의 상태
        switch(_curState)
        {
            case eBOSS_STATE.IDLE:
                break;

            case eBOSS_STATE.STUN:
                if (!_isDoingStun)
                    StartCoroutine(Stun());
                break;

            case eBOSS_STATE.DIE:
                _iState = eBOSS1_1STATE.DIE;
                break;
        }


        // 5) boss1_1의 독자적 상태 iSTATE
        switch (_iState)
        {
            case eBOSS1_1STATE.STROLL:
                Stroll();
                break;

            case eBOSS1_1STATE.STOP:
                STOP();
                break;

            case eBOSS1_1STATE.STEP:
                Step();
                break;

            case eBOSS1_1STATE.DIE:
                if(!_isAllDestroy)
                {
                    // 아래 함수들 한 번만 호출을 위함
                    _isAllDestroy = true;

                    // 죽은색으로 변경
                    _spriteRenderer.color = new Color(150 / 255f, 150 / 255f, 150 / 255f);

                    // 모든 코루틴 정지
                    StopAllCoroutines();

                    // 코루틴 정지로 인해 moveSpeed = 0 인 상태라면 originSpeed로 변경
                    _move._moveSpeed = _originSpeed;

                    // 그림자, 이펙트 전부 삭제
                    AllClearEffect();

                    _stageManager._isDead1 = true;
                }
                break;
        }
    }


    // DIE 상태일때 그림자, 이펙트 삭제
    void AllClearEffect()
    {
        // 그림자 삭제
        if (_shadow != null)
            Destroy(_shadow);

        // 이펙트 클래스 접근 --> 이펙트 삭제
        _effect.DestroyEffect_DIE();
    }



    // STROLL 상태
    void Stroll()
    {
        // 1) 이전의 그림자 삭제
        if (_shadow != null)
            Destroy(_shadow);

        // 2) 속도 복구
        _move._moveSpeed = _originSpeed;

        // 플레이어가 죽음 상태라면 계속 stroll 상태 유지를 위함
        if (_playerStatus._curState != PlayerStatus.ePLAYER_STATE.DIE)
        
        {
            // 3) Stroll 타임 체크
            _checkStrollTime += Time.deltaTime;

            // 4) Stroll 시간을 넘어선다면 Stop 으로 상태변화
            if (_checkStrollTime > _strollTime)
            {
                _iState = eBOSS1_1STATE.STOP;
                _checkStrollTime = 0f;
            }
        }
    }


    // STOP 상태
    void STOP()
    {
        // 1) boss1 정지 시키기
        _move._moveSpeed = 0f;

        // 2) stop & Shake 시간 체킹
        _checkBeforStepTime += Time.deltaTime;
        _checkShakeTime += Time.deltaTime;

        // 3) shake 중이지 않다면 shake 코루틴 실행
        if (!_isDoingShake)
        {
            StartCoroutine(Shake());
        }

        // 4) Stop 시간을 넘어선다면 STEP 으로 상태변화
        //     관련  변수들 초기화
        if (_checkBeforStepTime > _BeforeStepTime)
        {
            _iState = eBOSS1_1STATE.STEP;
            _checkShakeTime = 0f;
            _checkBeforStepTime = 0f;
            _isDoingShake = false;
        }
    }



    // STEP 상태
    void Step()
    {
        // 1) _rateCheck = true 라면 _rate의 수치 점점 ++ 
        //      lerp 이동을 위함
        if (_rateCheck)
            _rate += Time.deltaTime * _stepSpeed;

        // 2) _rateCheck = false 라면 _rate 수치 = 0으로 초기화
        else if (!_rateCheck)
            _rate = 0f;

        // 3) 그림자 위로 쿵 코루틴 한 번만 실행. 효과음 재생
        if (!_isDoingStep)
        {
            StartCoroutine(StepGround());
            _sound.PlayOneShot(_sound.clip);
        }

    }


    // 좌우 흔들림 + 그림자 효과
    IEnumerator Shake()
    {
        // shake 코루틴 한 번만 실행을 위함
        _isDoingShake = true;

        // 1) 그림자 효과 만들고 _shadow 에 저장
        GameObject Shadow =
            Instantiate(_shadowPrefab,
            new Vector2(transform.position.x + _shadowDistX, transform.position.y + _shadowDistY)
            , transform.rotation);

        _shadow = Shadow;
            
        // 2) 그림자 생성 후 boss1_1 과 그림자 같이 흔들리는 효과

        Vector2 origin = _myRigid.position;
        Vector2 shadowOrigin = _shadow.transform.position;
        
        Vector2 random = new Vector2(0f, 0f);
        Vector2 proRandom = new Vector2(0f, 0f);

  
        // 3) shakeTime 시간 동안 그림자와 함께 shake
          while (_checkShakeTime < _shakeTime)
           {
              // Shake 방향 설정
              // 이전에 왼쪽으로 움직였다면 오른쪽으로
              if (proRandom.x < 0)
                 random = new Vector2(Random.Range(0f, 0.25f), 0f);

              // 이전에 오른쪽으로 움직였다면 왼쪽으로
              else if (proRandom.x > 0)
                 random = new Vector2(Random.Range(-0.25f, 0f), 0f);

              // 이전에 움직이지 않았다면 좌우 중 랜덤값으로
              else if (proRandom.x == 0)
                 random = new Vector2(Random.Range(-0.25f, 0.25f), 0f);

              // Shake 방향 만큼 이동
               _myRigid.position = Vector2.Lerp(origin, origin + random, 0.5f);
                if(_shadow!=null)
                    _shadow.transform.position = Vector2.Lerp(shadowOrigin, shadowOrigin + random, 0.5f); 

              // 이동한 방향 proRandom 에 저장
              proRandom = random;

              // 다음 프레임까지 대기
              yield return null;
        }//while (_checkShakeTime < _shakeTime)


        // 4) 오브젝트 위치 원상복구
        _myRigid.position = origin;
        _shadow.transform.position = shadowOrigin;

    }// IEnumerator Shake()



    // boss1_1을  movePoint로 옮기는 코루틴
    IEnumerator MoveStep(Vector2 movePoint)
    {
        while(_myRigid.position.y != movePoint.y)
        {
            _myRigid.position = Vector2.Lerp(_myRigid.position, movePoint, _rate);
            yield return null;
        }
    }



    // 그림자 위치로 하강 -->  장판 효과 --> 상승 까지의 코루틴 
    IEnumerator StepGround()
    {
        // 스텝 코루틴 한 번만  실행을 위함
        _isDoingStep = true;
        // boss1_1 스텝 위치
        _stepPoint = new Vector2(_myRigid.position.x, _myRigid.position.y + _downPoint);
        Vector2 origin = _myRigid.position;
        _rateCheck = true;


        // 1) 아래로 하강
        StartCoroutine(MoveStep(_stepPoint));

        // 2) _stepPoint.y == _position.y 가 된다면 코루틴 계속 진행
        yield return new WaitUntil(()=>_stepPoint.y == _myRigid.position.y);
        StopCoroutine(MoveStep(_stepPoint));

        // 2.5) boss1_1 하강 종료 후 콜라이더 on
        GetComponent<Collider2D>().enabled = true;

        // 3) _rate 초기화 및 체크 정지
        _rateCheck = false;

        // 4) 랜덤 방향 장판 효과 on 
        _effect.StartEffect();

        // 5) 대기
        yield return new WaitForSeconds(3f);

        // 6) _rate 체크 시작 및 이펙트 삭제 후 원위치로 돌아가기
        _rateCheck = true;
        StartCoroutine(_effect.DestroyEffect(_effect._effectList));

        // 7) 원위치로 상승 코루틴
        StartCoroutine(MoveStep(origin));

        // 8) _myRigid.position == origin 이 된다면 코루틴 계속 진행
        yield return new WaitUntil(() => _myRigid.position.y == origin.y);

        // 9) 죽음 상태가 아니라면 STROLL 로 회귀
        if(_curState!= eBOSS_STATE.DIE)
            _iState = eBOSS1_1STATE.STROLL;

        // 10) 다음 코루틴 위해 _rateCheck, _isDoingStep false
        _rateCheck = false;
        _isDoingStep = false;
        GetComponent<Collider2D>().enabled = false;
    }


}


