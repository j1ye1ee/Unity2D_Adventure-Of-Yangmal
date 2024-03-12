using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Move : MonoBehaviour
{
    Rigidbody2D _myRigid;
    public float _moveSpeed;
    public float _originSpeed;
    public bool _isWall;
    Vector2 _proDirection; // 이전의 움직이는 방향 저장

    Boss2_StageManager _stageManager;

    // page2Start move
    Vector2 _pause = new Vector2(0, 0);
    Vector2 _origin;
    public bool _isOriginPosition = false;
    bool _isDoingPage2Startmove = false;
    float _checkPauseTime;
    float _pauseTime = 3f;
    bool _isDoneShake = false;
    bool _isDoneCrushWall = false;
    public bool _isGoal = false;

    public bool _isDoingPage2Move = false;
    public float _page2MoveTime;
   public float _endMoveTime;

    float _originPlayerMoveSpeed;

    GameObject _player;

    public GameObject _square4;

    public AudioSource _clushWallSound;


    void Start()
    {
        _stageManager 
            = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();

        _myRigid = GetComponent<Rigidbody2D>();
        _origin = _myRigid.position;
        _proDirection = Vector2.right;
        _originSpeed = _moveSpeed;
        _player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        switch(_stageManager._curState)
        {
            case Boss2_StageManager.eSTAGE_STATE.NONE:
                NormalMove();
                break;
            case Boss2_StageManager.eSTAGE_STATE.PAGE1CLEAR:
                _checkPauseTime += Time.deltaTime;
                StartPage2Move();
                break;
            case Boss2_StageManager.eSTAGE_STATE.PAGE2:
                FollowMove();
                break;
            case Boss2_StageManager.eSTAGE_STATE.STAGECLEAR:
                break;
        }
    }



    // 벽과의 충돌 검사 --> 충돌시 isWall = true;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "World")
            _isWall = true;
        else
            _isWall = false;
    }

    // Boss2 의 움직임1
    void NormalMove()
    {
        // 벽이 없다면 _proDirection  뱡향 으로 이동 
        if (!_isWall)
        {
            _myRigid.velocity = _proDirection * _moveSpeed;
        }


        else if (_isWall)
        {
            if (_proDirection == Vector2.right)
            {
                // 왼쪽을 보도록 이미지 뒤집기
                transform.localScale = new Vector2(-1, 1);
                // 왼쪽으로 이동
                _myRigid.velocity = Vector2.left * _moveSpeed;
                _proDirection = Vector2.left;
                _isWall = false;
            }

            else if (_proDirection == Vector2.left)
            {
                // 오른쪽을 보도록 이미지 뒤집기
                transform.localScale = new Vector2(1, 1);

                // 오른쪽으로 이동
                _myRigid.velocity = Vector2.right * _moveSpeed;
                _proDirection = Vector2.right;
                _isWall = false;
            }

        }

    }


    void StartPage2Move()
    {
        if(_square4!=null)
            Destroy(_square4);

        // startMove 시작 전이라면 
        if (!_isDoingPage2Startmove)
        {
            _isDoingPage2Startmove = true;
            // page2Move 시작
            StartCoroutine(GotoOrigin_Shake());
        }


        // shake 종료 후 벽뿌시기
        else if (_isDoneShake)
            CrushWall();

        // 벽부수기 완료 후 지정위치까지 앞으로 전진
        else if (_isDoneCrushWall)
            StartCoroutine(MoveToPoint());

    }


    void FollowMove()
    {
        _page2MoveTime += Time.deltaTime;
        if (!_isDoingPage2Move)
            StartCoroutine(Page2Move());
    }


    IEnumerator GotoOrigin_Shake()
    {
        // 플레이어  콜라이더 끄기 & 움직이지 못하게 하기(무빙중 플레이어에게 해를 끼치지 않기 위함)
        _player.GetComponent<Collider2D>().enabled = false;
        _originPlayerMoveSpeed = _player.GetComponent<PlayerMove>()._moveSpeed;
        _player.GetComponent<PlayerMove>()._moveSpeed = 0f;


        // 0) origin position 으로 이동
        _myRigid.velocity = _pause;

        // 카메라 모드 전환
        Boss2CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<Boss2CameraController>();
        camera._curMode = MainCameraMove.eCAMERA_MODE.BOSS2_FOLLOW;

        while (!_isOriginPosition)
        {
            Vector2 direction = _origin - _myRigid.position;
            _myRigid.velocity = direction.normalized * _moveSpeed;
            yield return null;
        }

        // boss2 정지 & 시간초기화
        _myRigid.velocity = _pause;
        _checkPauseTime = 0;

        // origin 위치 도착까지 대기
        yield return new WaitUntil(() => _isOriginPosition == true);

        // sake 시작
        StartCoroutine(Shake());

    }

    IEnumerator Shake()
    {
        Vector2 origin = _myRigid.position;
        Vector2 random = new Vector2(0f, 0f);
        Vector2 proRandom = new Vector2(0f, 0f);

        // 정지 시간 동안 shake 를 위함
        while (_checkPauseTime < _pauseTime - 0.25f)
        {
            // 1 ) Shake 방향 설정
            // 이전에 왼쪽으로 움직였다면 오른쪽으로
            if (proRandom.x < 0)
                random = new Vector2(Random.Range(0f, 0.5f), 0f);

            // 이전에 오른쪽으로 움직였다면 왼쪽으로
            else if (proRandom.x > 0)
                random = new Vector2(Random.Range(-0.5f, 0f), 0f);

            // 이전에 움직이지 않았다면 좌우 중 랜덤값으로
            else if (proRandom.x == 0)
                random = new Vector2(Random.Range(-0.5f, 0.5f), 0f);

            // 2 ) Shake 방향 만큼 이동
            _myRigid.position = Vector2.Lerp(origin, origin + random, 0.5f);

            // 3 ) 이동한 방향 proRandom 에 저장
            proRandom = random;

            // 4 ) 다음 프레임까지 대기
            yield return null;
        }
        // 5 ) 오브젝트 위치 원상복구
        _myRigid.position = origin;

        _isDoneShake = true;
    }

    void CrushWall()
    {
        _isDoneShake = false;
        GameObject.FindWithTag("Boss2Wall").GetComponent<Boss2WallGroup>().RigidBodyOn();
        _myRigid.AddForce(Vector2.down * 100f, ForceMode2D.Impulse);
        _isDoneCrushWall = true;
        StartCoroutine(SoundOn());
    }
    

    IEnumerator MoveToPoint()
    {
        GameObject movePoint;
        movePoint = GameObject.Find("movePoint");

        while (!_isGoal)
        {
            transform.position = Vector2.Lerp(transform.position, movePoint.transform.position, Time.deltaTime*0.2f);
            yield return null;
        }


        yield return new WaitForSeconds(3f);

        // 카메라 모드 전환
        Boss2CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<Boss2CameraController>();
        camera._curMode = MainCameraMove.eCAMERA_MODE.NORMAL;

        // 포인트까지 이동 & 카메라 모드전환 완료시 stage 상태 변경
        _stageManager._curState = Boss2_StageManager.eSTAGE_STATE.PAGE2;

        // 플레이어  콜라이더 켜기 & 스피드 원복
        _player.GetComponent<Collider2D>().enabled = true;
        _player.GetComponent<PlayerMove>()._moveSpeed = _originPlayerMoveSpeed;


        Debug.Log("page2 진입");

        // 벽돌 상태 바꾸기 
        GameObject bricks = GameObject.FindWithTag("Boss2Wall");
        for (int index = 0; index < bricks.transform.childCount; index++)
        {
            GameObject brick = bricks.transform.GetChild(index).gameObject;
            brick.GetComponent<Boss2Bricks>()._eState = Boss2Bricks.eBRICKSTATE.AFTER_PAGE2;
        }

    }

    IEnumerator Page2Move()
    {
        _isDoingPage2Move = true;
        // 3초 대기
        yield return new WaitForSeconds(3f);

        //moveTime 초기화
        _page2MoveTime = 0;

        Vector2 direction;
        direction = (_player.transform.position - gameObject.transform.position).normalized;
        
        // endMoveTime 까지 이동
        while(_page2MoveTime < _endMoveTime)
        {
            _myRigid.velocity = _moveSpeed * direction;
            yield return null;
        }

        // MoveTime 초기화
        _isDoingPage2Move = false;
        _page2MoveTime = 0;
    }

    IEnumerator SoundOn()
    {
        _clushWallSound.PlayOneShot(_clushWallSound.clip);
        yield return new WaitForSeconds(0.5f);
        _clushWallSound.PlayOneShot(_clushWallSound.clip);
        yield return new WaitForSeconds(0.3f);
        _clushWallSound.PlayOneShot(_clushWallSound.clip);
        yield return new WaitForSeconds(0.1f);



    }
}
