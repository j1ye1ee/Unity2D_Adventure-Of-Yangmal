using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAi : MonoBehaviour
{
    // 적 움직임 담당 클래스

    public Enemy _enemy;

    public bool _isWall = false;
    [SerializeField]

    public bool _chooseDir = false;

    public Vector2 _direction;
    public Transform  _playerTrsf;

    [SerializeField]
    float _rayDistance;

    public float _randomsetMinTime;
    public float _randomsetMaxTime;

    public float _pauseFollowTime;



    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _direction = GetRandomVector2();
    }

    private void Start()
    {
        if (!GameManager.Instance._isGameOver)
            _playerTrsf = PlayerStatus.Instance.gameObject.GetComponent<Transform>();
    }


    private void Update()
    {
        // 움직이는 방향으로 레이 그리기
        Debug.DrawRay(transform.position, _direction.normalized*_rayDistance, Color.red);


    }


    // stroll 상태의 움직임
    public void Stroll()
    {
        // 1) 충돌 검사
        isWall();

        // 2) 방향 세팅
           
          // 벽 충돌시
            if (_isWall )
                // 충돌 회피
                StartCoroutine(ReturnFromWall());

         // 벽 충돌하지 않을 시 && 방향 설정이 되지 않았을 시
            else if (!_isWall && !_chooseDir)
                // 랜덤 방향 set
                StartCoroutine(ChooseRandomDirection());

        // 3) 움직임
        _enemy.Move();
    }

    public void Follow()
    {
        // 0) STROLL 상태의 코루틴 모두 중지
        StopAllCoroutines();
        _enemy._speed = _enemy.setSpeed;
        

        // 1) 충돌 검사
        isWall();

        // 2)방향 세팅

        // 벽 충돌시
        if (_isWall)
        //충돌 회피
        {
            StartCoroutine(ReturnFromWall());
            //FOLLOW 상태에서 벽과 무한충돌을 방지하기 위함
            StartCoroutine(PauseFollow());
        }

        //벽 충돌하지 않을 시 방향 = 플레이어 쪽으로
        else if (!_isWall)
        {
            _direction = (_playerTrsf.position - transform.position).normalized;
        }

        // 3) 움직임
        _enemy.Move();

        // 4) 랜덤방향 설정을 위함
        _chooseDir = false;
    }

    // follow상태에서 벽과 충돌시 무한 충돌을 방지하기 위함
    public IEnumerator PauseFollow()
    { 

        // 원래 감지값을 저장한 후 감지 기능을 차단
        _enemy._range = 0;
        // enemy stroll 상태 전환
        _enemy._curState = Enemy.eENEMY_STATE.STROLL;
        // 대기 시간 후 감지값 원복
        yield return new WaitForSeconds(_pauseFollowTime);
        _enemy._range = _enemy.setRange;
    }


    public void StopMove()
    {
        _direction = new Vector2(0, 0);
        _enemy._speed = 0;
        _enemy.Move();
        _chooseDir = false;
    }


    // Random방향 정하는 코루틴
    IEnumerator ChooseRandomDirection()
    {
        _chooseDir = true;
        yield return new WaitForSeconds(Random.Range(_randomsetMinTime, _randomsetMaxTime));
        _direction = GetRandomVector2();
        _chooseDir = false;
    }

    // 랜덤 방향 벡터 retrun
    Vector2 GetRandomVector2()
    {
        Vector2 randomDir;

        do
            randomDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        //(0,0)생성될 시 다시 반복문
        while (randomDir.x == 0 && randomDir.y == 0);

        //랜덤벡터 반환
        return randomDir;
    }

    // 충돌 검사
    public void isWall()
    {
        // 1) 벽과의 충돌 검사
        RaycastHit2D hitData;


        hitData = Physics2D.Raycast
            (transform.position, _direction, _rayDistance, LayerMask.GetMask("World"));

        if (hitData.collider != null)
        {
            _isWall = true;
            StartCoroutine(DownSpeed(_enemy.setSpeed));
        }
        else
            _isWall = false;

    }

    // Enemy 끼리의 충돌시 랜덤한 방향으로 재이동
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _direction = GetRandomVector2();
            _enemy.Move();
        }

    }



    // 벽 충돌시 잠시 정지 코루틴
    public IEnumerator DownSpeed(float speed)
    {
        _enemy._speed = 0f;
        yield return new WaitForSeconds(0.035f);
        _enemy._speed = speed;
    }

        // 충돌 회피 , 원점대칭 이용
       public IEnumerator ReturnFromWall()
    {
        // 충돌 방향 저장 = collisionDir
        Vector2 collisionDir = _direction;

        // 원점대칭 방향 = originDir
        Vector2 originDir = -1 * collisionDir;

        // 충돌 반대 방향으로 방향 지정, 움직임
        _direction = originDir;
        _enemy.Move();

        // 0.05초  뒤 랜덤방향 설정 
        yield return new WaitForSeconds(0.05f);

        // 랜덤방향 벡터 = randomDir
        Vector2 randomDir = new Vector2(0f,0f);

        // 움직인 방향의 벡터가 1사분면에 위치할때
        if (originDir.x > 0 && originDir.y > 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(0f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 1f);
        }

        // 2사분면 위치
        else if (originDir.x < 0 && originDir.y > 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(0f, 1f);
        }

        // 3사분면 위치
        else if (originDir.x < 0 && originDir.y < 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 0f);
        }

        // 4사분면 위치
        else if (originDir.x > 0 && originDir.y < 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 0f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 1f);
        }

        // x축 위치
        else if (originDir.y == 0)
        {
            if (originDir.x > 0)
            {
                do
                {
                    randomDir.x = Random.Range(0f, 1f);
                    randomDir.y = Random.Range(-1f, 1f);
                }
                // 0,0 이 나오면 다시 실행한다.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

            else if(originDir.x <0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 0f);
                    randomDir.y = Random.Range(-1f, 1f);
                }
                // 0,0 이 나오면 다시 실행한다.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

        }//else if(originDir.y == 0)

        // y축 위치
        else if (originDir.x == 0)
        {
            if (originDir.y > 0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 1f);
                    randomDir.y = Random.Range(0f, 1f);
                }
                // 0,0 이 나오면 다시 실행한다.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

            else if(originDir.y<0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 1f);
                    randomDir.y = Random.Range(-1f, 0f);
                }
                // 0,0 이 나오면 다시 실행한다.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

        }//else if(originDir.x == 0)



        _direction = randomDir.normalized;

    }



}
