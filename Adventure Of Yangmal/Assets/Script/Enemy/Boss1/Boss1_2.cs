using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_2 : Boss
{
    // Boss1_2 의 행동&생명 관리 클래스


    // StageManager 참조
    Boss1_StageManager _stageManager;

    // 그림자 생성시 앞에 벽이 있는지 확인
    public enum eISWALL
    {
        RIGHTWALL,
        LEFTWALL,
        NOWALL
    }

    // 독자적 행동 상태 4가지
    public enum eBOSS1_2STATE
    {
        FOLLOW,
        STOP,
        STEP,
        DIE
    }

    public eBOSS1_2STATE _iState = eBOSS1_2STATE.FOLLOW;
    public eISWALL _wall = eISWALL.NOWALL;
    public Boss1_1 _boss1_1;

    // 움직임
    public float _distance; // boss1_1 과의 거리차를 두기 위함
    public float _rayDistance;
    public float _followRate;

    // 벽 감지
    public bool _isRightWall;
    public bool _isLeftWall;


    // 그림자 생성
    public List<GameObject> _shadows = new List<GameObject>();


    Vector2 _dist;
    public float _downPoint;
    public int _shadowCountMin;
    public int _shadowCountMax;
    public float _xDistMin;
    public float _xDistMax;
    public float _yDistMin;
    public float _yDistMax;


    // 그림자 생성 후 포물선점 생성
    public List<GameObject> _parabolaSpots = new List<GameObject>();
    public float _upPoint;



    // 그림자 생성 -> 포물선점 생성 -> 꿍꿍꿍 찍는 움직임
    public int _stepCount;
    public bool _isDoingBeforeStep = false;
    public bool _isDoingStep = false;
    public float _moveSpeed;
    public float _rate;

    // 풀링 관련 변수
    Boss1_2EffectPoolManager _poolManager;

    // 이펙트 관리 클래스 참조
    public Boss1_2Effect _boss1_2Effect;

    // boss1_1  사망시 독자적 움직임을 위한 변수
    public float _stopTime;
    public float _checkStopTime;

    // 사망시 이펙트 삭제를 위함
    public bool _isAllDestroy = false;

    // 효과음
    public AudioSource _sound;


    void Start()
    {
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss1_StageManager>();

        // 발바닥 찍고 나서 collider on
        GetComponent<Collider2D>().enabled = false;

        _boss1_1 = FindObjectOfType<Boss1_1>();
        _myRigid = gameObject.GetComponent<Rigidbody2D>();
        _boss1_2Effect = GetComponent<Boss1_2Effect>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
        _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        _poolManager = GameObject.FindWithTag("boss1_2EffectPool").GetComponent<Boss1_2EffectPoolManager>();

    }


    // boss1_1 의 상태에 따라 행동이 달라지므로 LateUpdate
    void LateUpdate()
    {
        // 1) 죽음 검사
        IsDead();

        // 2) 스턴 검사
        if (_isStun && _curState != eBOSS_STATE.DIE)
            _curState = eBOSS_STATE.STUN;


        // 3) boss 클래스의 상태
        switch (_curState)
        {
            case eBOSS_STATE.IDLE:
                break;

            case eBOSS_STATE.STUN:
                if (!_isDoingStun)
                    StartCoroutine(Stun());
                break;

            case eBOSS_STATE.DIE:
                _iState = eBOSS1_2STATE.DIE;
                break;
        }


        // 4) 그림자생성이나 스텝 밟기를 하지 않고 있을 경우에만 iSTATE 상태변화를 줌
        // boss1_1이 정지하면 iSTATE == STOP
        if (!_isDoingStep && !_isDoingBeforeStep)
        {
            // boss 1_1 살아있을때 --> boss1의 움직임을 따름
            if (_boss1_1._iState != Boss1_1.eBOSS1_1STATE.DIE)
            {
                if (_boss1_1._iState == Boss1_1.eBOSS1_1STATE.STOP)
                    _iState = eBOSS1_2STATE.STOP;
            }

        }

        // 5) boss 1_1 죽어있을때 &  follow 상태일때 --> 독자적으로 움직이기
        if (_boss1_1._iState == Boss1_1.eBOSS1_1STATE.DIE 
            && _iState == eBOSS1_2STATE.FOLLOW)
        {
            // 정지까지의 상태변화 시간 체크
            _checkStopTime += Time.deltaTime;

            // 정지상태 돌입 시간이 되었다면 iSTATE --> STOP
            if (_checkStopTime >= _stopTime)
            {
                _iState = eBOSS1_2STATE.STOP;
                _checkStopTime = 0;
            }

        }


        //// 5.5) 플레이어 죽음 검사 -- 플레이어 죽음 상태일때 FOLLOW 상태 진입
        //if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE || _player == null)
        //{
        //    _iState = eBOSS1_2STATE.FOLLOW;
        //}




            // 6) boss1_2 클래스의 독자적 상태 iSTATE
            switch (_iState)
        {
            case eBOSS1_2STATE.FOLLOW:
                Boss1_2Follow(_boss1_1._myRigid.position); // boss1_1따라 이동
                break;

            case eBOSS1_2STATE.STOP:
                _followRate = 0f;
                Boss1_2Stop();  //스텝 밟을 그림자 생성
                break;

            case eBOSS1_2STATE.STEP:
                Boss1_2Step(); // 생성한 그림자 위로 포물선 이동
                break;

            case eBOSS1_2STATE.DIE:
                if(!_isAllDestroy) // 죽음시 이펙트 삭제
                { 
                    // 죽은색으로 변경
                    _spriteRenderer.color = new Color(150 / 255f, 150 / 255f, 150 / 255f);

                    // 모든 코루틴 정지
                    StopAllCoroutines();

                    // 그림자, 이펙트 전부 삭제
                    AllClearEffect();

                    _stageManager._isDead2 = true;
                }

                Boss1_2Follow(_boss1_1._myRigid.position);
                break;

        }
    }






    // FOLLOW
    void Boss1_2Follow(Vector2 direction)
    {
        // Lerp로 움직이므로 followRate를 점점 크게 만든다
        // --> _distance 이상이 되면 0으로 초기화
        if (_followRate >= _distance)
            _followRate = 0f;

        _followRate = Mathf.Clamp(_followRate, 0f, _distance);
        _followRate += Time.deltaTime * 0.5f;

        // newDirection  = Lerp로 이동하고자 하는 목적지
        Vector2 newDirection;
        newDirection = new Vector2(direction.x, direction.y - 7f);
        _myRigid.position = Vector2.Lerp(_myRigid.position,newDirection , _followRate);
    }


    // STOP
    void Boss1_2Stop()
    {
        // 코루틴 한 번만실행을 위함
        if (!_isDoingBeforeStep)
            StartCoroutine(InstantiateShadows());
        
    }

    // STEP
    void Boss1_2Step()
    {
        // 자연스러운 lerp 이동을 위해 _rate 의 크기를 점차적으로 늘려준다.
        _rate += Time.deltaTime * _moveSpeed;

        // 코루틴 한 번만 실행을 위함
        if (!_isDoingStep)
            StartCoroutine(Step());
            


        
    }



    // 죽음시 이펙트 & 그림자 삭제
    void AllClearEffect()
    {
        // 이펙트 클래스 접근 --> 삭제
        _boss1_2Effect.AllDestroy();

        // 그림자 리스트 접근 --> 삭제

        for (int listNum = 0; listNum < _shadows.Count; listNum++)
            _shadows[listNum].gameObject.SetActive(false);

        _shadows.Clear();

    }




    //  그림자 생성시 벽 감지 --> eISWALL 상태를 결정
    void IsWall(Vector2 objectPosition)
    {
        // 1) 오브젝트의 포지션 위치에서 오른쪽 벽 충돌 먼저 검사
        RaycastHit2D hitDataRight;
        hitDataRight = Physics2D.Raycast
            (objectPosition, Vector2.right, _rayDistance, LayerMask.GetMask("World"));

        // 2) 오른쪽 벽에 충돌 감지시 --> RIGHTWALL 상태
        if (hitDataRight.collider != null)
        {
            _isRightWall = true;
            _wall = eISWALL.RIGHTWALL;
        }

        // 3) 오른쪽 벽 충돌 감지하지 않았을 시
        else 
        {
            _isRightWall = false;

            // 4) 왼쪽 검사
            RaycastHit2D hitDataLeft;
            hitDataLeft = Physics2D.Raycast
                (objectPosition, Vector2.left, _rayDistance, LayerMask.GetMask("World"));

            //5) 왼쪽 벽 충돌 감지시 --> LEFTWALL 상태
            if (hitDataLeft.collider != null)
            {
                _isLeftWall = true;
                _wall = eISWALL.LEFTWALL;
            }

            //6) 왼쪽 벽 충돌 감지하지 않았을 시
            else
                _isLeftWall = false;
        }

        // 7) 오른쪽, 왼쪽 검사 후 둘다 벽이 없을 시 --> NOWALL 상태
        if (!_isRightWall && !_isLeftWall)
        {
            _wall = eISWALL.NOWALL;
        }
    }




    // 그림자 생성 위치 / 를 생성
    Vector2 Position(Vector2 origin, Vector2 dist)
    {
        // 현 위치 + Dist 만큼을 더함
        // _downPoint만큼 y좌표 하강

        Vector2 position = origin + dist;
        return position;
    }




    // eISWALL == RIGHTWALL / eISWALL == LEFTWALL 인 경우
    // Dist(다음 그림자 생성을 위해 현재 그림자 위치에 더하는 Vecto2 값)를 구할 때
    Vector2 XYDist()
    {
        Vector2 Dist;
        float xDist = 0f;
        float yDist = 0f;

        // 1) xDist 설정
            // 오른쪽에 벽이 있을때
        if (_wall == eISWALL.RIGHTWALL)
        {
            // xDist = 랜덤 숫자 생성 및 음수로 설정
            xDist = -1f * Random.Range(_xDistMin, _xDistMax);
        }

            // 왼쪽에 벽이 있을때
        else if (_wall == eISWALL.LEFTWALL)
        {
            // xDist = 랜덤 숫자 생성 및 양수로 설정
            xDist = Random.Range(_xDistMin, _xDistMax);
        }


        // 2) yDist 설정
        yDist = Random.Range(_yDistMin, _yDistMax);

        // 3) 값 반환
        Dist = new Vector2(xDist, yDist);
        return Dist;
    }




    // 그림자 생성시 eISWALL == NOWALL 인 경우
    // 왼쪽, 오른쪽 랜덤 방향의 Dist 를 구할 때
    Vector2 RandomXYDist()
    {
        Vector2 Dist;
        float xDist = 0f;
        float yDist = 0f;

        
        // xDist = 50%의 확률로 음수 or 양수
          int isNegative = Random.Range(1, 101);

        if (isNegative > 50f)
            xDist = -1f * Random.Range(_xDistMin, _xDistMax);
       
        else
            xDist = Random.Range(_xDistMin, _xDistMax);

        Dist = new Vector2(xDist, yDist);
        return Dist;
    }




    // 그림자 생성 코루틴
    IEnumerator InstantiateShadows()
    {
        // 코루틴 한 번만 실행을 위함
        _isDoingBeforeStep = true;

        // 1) 그림자 갯수 랜덤 설정
        _stepCount = Random.Range(_shadowCountMin, _shadowCountMax + 1);
       

        // 2) 같은 방향으로 생성할 그림자 개수 카운트 초기화
        int sameDirCount = 0;

        // 3) 생성한 그림자의 방향을 저장할 변수 선언
        bool isRight = false;

        // 4) boss1_2의 위치에서 벽 검사 
        IsWall(transform.position);

        // 5) 벽 검사에 따른 _dist 값 결정

            // 오른쪽이나 왼쪽 벽이 있을 경우 XYDist()
        if (_wall == eISWALL.RIGHTWALL || _wall == eISWALL.LEFTWALL)
            _dist = XYDist();

            // 벽이 없을 경우 RandomXYDist()
        else if(_wall == eISWALL.NOWALL)
            _dist = RandomXYDist();

        // 6) 첫번째 그림자 pool 에서 대여
        // Position 메서드 활용하여 그림자 생성할 위치 Vector2 결정(원래 위치 + dist)  
        GameObject firstShadow = _poolManager.GetObject(
            _poolManager._shadowPool,
            _poolManager._shadow,
            Position(new Vector2(transform.position.x, transform.position.y + _downPoint), _dist),
            transform.rotation);

            //  _shadows 리스트에 그림자 저장
        _shadows.Add(firstShadow);


        // 7) 생성한 그림자 진행방향 저장
        if (_dist.x > 0)
                isRight = true;

        else if (_dist.x < 0)
                isRight = false;

        // 8) 첫번째 그림자 생성 후 0.5초 대기 
        yield return new WaitForSeconds(0.5f);


        // 9) 그림자 갯수 -1 만큼 반복하여 나머지 생성(첫째 그림자 생성했으므로)
        for (int num = 0; num <  _stepCount-1; num ++)
        {
            // 먼저 생성한 그림자의 iswall 검사
            IsWall(_shadows[num].transform.position);


            // 1.  먼저 생성한 그림자의 iswall 결과가 NOWALL 일 경우
            //          _shadowCountMin 만큼 같은 방향으로 그림자 생성
            if(_wall == eISWALL.NOWALL && sameDirCount < _shadowCountMin)
            {
                // 첫번째 생성한 그림자가 오른쪽으로 생성되었다면 오른쪽으로 생성 유도
                if (isRight)
                    _wall = eISWALL.LEFTWALL;

                // 먼저 생성한 그림자가 왼쪽으로 생성되었다면 오른쪽으로 생성 유도
                else if (!isRight)
                    _wall = eISWALL.RIGHTWALL;

                // 결과에 따라 적절한 위치에 그림자 pool 에서 생성
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, XYDist()),
                    transform.rotation);

                // 리스트에 추가 및 NOWALL로 상태 초기화
                _wall = eISWALL.NOWALL;
                _shadows.Add(shadow);
                sameDirCount++;
            }



            // 2. 먼저 생성한 그림자의 iswall 결과가 NOWALL이 아닐 경우 
            else if(_wall != eISWALL.NOWALL)
            {
                // eISWALL 상태에 따라 적절한 위치에 그림자 pool에서 생성
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, XYDist()),
                    transform.rotation);

                // _dist 에 따른 isRight(그림자 진행 방향) 설정
                _dist = XYDist();
                if (_dist.x > 0)
                    isRight = true;

                else if (_dist.x < 0)
                    isRight = false;


                // sameDirCount 초기화
                sameDirCount = 0;

                // 리스트에 추가
                _shadows.Add(shadow);
            }



            // 3.  같은 방향의 그림자 생성이 모두 이루어짐 & NOWALL 이 아닌 경우
            else if(_wall == eISWALL.NOWALL && sameDirCount >= _shadowCountMin)
            {
                // 왼쪽, 오른쪽 랜덤으로 적절한 위치에 그림자 pool 에서 생성
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, RandomXYDist()),
                    transform.rotation);

                // sameDirCount 초기화
                sameDirCount = 0;
                // 리스트에 추가
                _shadows.Add(shadow);
            }

            yield return new WaitForSeconds(0.5f);
        }// for (int num = 0; num <  count-1; num ++)


        // 10) 그림자 생성 완료 후 포물선 이동을 위한 점 생성
        MakeParabolaSpots();

        // 11) 그림자와 점 생성 후 step 으로 넘어가도록 상태 전이
        _iState = eBOSS1_2STATE.STEP;

    }




    // 포물선 이동을 위한 점 생성
    void MakeParabolaSpots()
    {
        
        Vector2 makePoint;
        float x;
        float y;

        // 1) 생성된 그림자의 갯수만큼 반복하여 점 생성
        for (int count = 0; count < _shadows.Count; count ++)
        {
             // 첫번째 makePoint 는 boss1_2 와 첫 번째 그림자 사이에 생성
            if (count == 0)
            {
                if (_myRigid.transform.position.y >= _shadows[count].transform.position.y)
                    y = _myRigid.transform.position.y + _upPoint;

                else
                    y = _shadows[count].transform.position.y + _upPoint;

                // x 값 결정 = 두 x 값의 평균
                x = (_myRigid.transform.position.x + _shadows[count].transform.position.x) * 0.5f;
            }


            // 두번째 makePoint 부터는 그림자와 그림자 사이에 점 생성
            else
            {
                // n 번째 그림자의 y 값과 n+1 번째 그림자의 y값을 비교
                // n 번째 그림자의 y 값이 더 크다면
                if (_shadows[count-1].transform.position.y >= _shadows[count].transform.position.y)
                    y = _shadows[count].transform.position.y + _upPoint;

                // n 번째 그림자의 y 값이 더 작다면
                else
                    y = _shadows[count -1].transform.position.y + _upPoint;
                // x 값 결정 = 두 x 값의 평균
                x = (_shadows[count-1].transform.position.x + _shadows[count].transform.position.x) * 0.5f;
            }

           
            // n 번째 makePoint 결정 & 생성 & 리스트에 추가
            makePoint = new Vector2(x, y);
            GameObject spot = _poolManager.GetObject(
                _poolManager._parabolaSpotPool,
                _poolManager._parabolaSpot,
                makePoint,
                transform.rotation);

            _parabolaSpots.Add(spot);
        }

    }





    // 그림자와 점 생성 이후 쿵쿵쿵따라가 밟는 코루틴
    IEnumerator Step()
    {
        _rate = 0f;
        _isDoingStep = true;
        // step 상태일때만 콜라이더 on
        GetComponent<Collider2D>().enabled = true;
        Vector2 origin = _myRigid.position;



        // 1) boss1_2 위치 ~ shadow[0] 까지 이동시키기
        while (_myRigid.transform.position != _shadows[0].transform.position)
         {
            Vector2 point1 =
                        Vector2.Lerp(_myRigid.transform.position,
                           _parabolaSpots[0].transform.position, _rate);
            Vector2 point2 =
                        Vector2.Lerp(_parabolaSpots[0].transform.position,
                                        _shadows[0].transform.position, _rate);
            Vector2 position =
                        Vector2.Lerp(point1, point2, _rate);

            _myRigid.position = position;

            yield return null;
         }

        _rate = 0;
                // 이동 완료 후 크랙 이펙트 생성
        StartCoroutine(_boss1_2Effect.CrackEffect());


        // 2) s[count] ~ s[count+1] 까지 이동 반복
        for(int count = 1; count<=_parabolaSpots.Count-1; count++)
        {
            while(_myRigid.transform.position != _shadows[count].transform.position)
            {
                Vector2 point1 =
                    Vector2.Lerp(_myRigid.transform.position,
                                        _parabolaSpots[count].transform.position, _rate);
                Vector2 point2 =
                        Vector2.Lerp(_parabolaSpots[count].transform.position,
                                        _shadows[count].transform.position, _rate);
                Vector2 position =
                        Vector2.Lerp(point1, point2, _rate);

                _myRigid.position = position;

                yield return null;
            }

                // 다음 s 에 도착하면 rate 초기화 , count++
            if (_rate > 1f)
            {
                _rate = 0;
                // 크랙 이펙트 생성
                StartCoroutine(_boss1_2Effect.CrackEffect());
                _sound.PlayOneShot(_sound.clip);
            }
        }//for(int count = 1; count<=_parabolaSpots.Count; count++)



        // 3) 그림자 생성 ~스텝 까지의 루틴 마친 후 그림자, 포물선점 게임오브젝트 삭제
        for (int listNum = 0; listNum < _stepCount; listNum++)
        {
            _shadows[listNum].gameObject.SetActive(false);
            _parabolaSpots[listNum].gameObject.SetActive(false);
        }

               // 이펙트 오브젝트 파괴
        StartCoroutine(_boss1_2Effect.DestroyEffect(_boss1_2Effect._effectList));


        // 4) shadows , parabolaSpots 모두 파괴될때까지 기다렸다가 다음 진행
        yield return new WaitUntil
            (() => !_shadows[_stepCount - 1].gameObject.activeSelf && ! _parabolaSpots[_stepCount - 1].gameObject.activeSelf);



        // 5) 다음 루틴을 위한 변수 초기화 & boss1_2 제자리 및 리스트 초기화
        while (_myRigid.position != origin)
        {
            _myRigid.position = Vector2.Lerp(_myRigid.position, origin, _rate / _moveSpeed * 0.05f) ;
            yield return null;
        }

        _rate = 0f;
        _iState = eBOSS1_2STATE.FOLLOW;
        GetComponent<Collider2D>().enabled = false;
        _isDoingBeforeStep = false;
        _isDoingStep = false;
        _shadows.Clear();
        _parabolaSpots.Clear();
        _boss1_2Effect._allEffect.Clear();

    }


    
}
