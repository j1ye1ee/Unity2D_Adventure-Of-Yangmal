using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAi : MonoBehaviour
{
    // �� ������ ��� Ŭ����

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
        // �����̴� �������� ���� �׸���
        Debug.DrawRay(transform.position, _direction.normalized*_rayDistance, Color.red);


    }


    // stroll ������ ������
    public void Stroll()
    {
        // 1) �浹 �˻�
        isWall();

        // 2) ���� ����
           
          // �� �浹��
            if (_isWall )
                // �浹 ȸ��
                StartCoroutine(ReturnFromWall());

         // �� �浹���� ���� �� && ���� ������ ���� �ʾ��� ��
            else if (!_isWall && !_chooseDir)
                // ���� ���� set
                StartCoroutine(ChooseRandomDirection());

        // 3) ������
        _enemy.Move();
    }

    public void Follow()
    {
        // 0) STROLL ������ �ڷ�ƾ ��� ����
        StopAllCoroutines();
        _enemy._speed = _enemy.setSpeed;
        

        // 1) �浹 �˻�
        isWall();

        // 2)���� ����

        // �� �浹��
        if (_isWall)
        //�浹 ȸ��
        {
            StartCoroutine(ReturnFromWall());
            //FOLLOW ���¿��� ���� �����浹�� �����ϱ� ����
            StartCoroutine(PauseFollow());
        }

        //�� �浹���� ���� �� ���� = �÷��̾� ������
        else if (!_isWall)
        {
            _direction = (_playerTrsf.position - transform.position).normalized;
        }

        // 3) ������
        _enemy.Move();

        // 4) �������� ������ ����
        _chooseDir = false;
    }

    // follow���¿��� ���� �浹�� ���� �浹�� �����ϱ� ����
    public IEnumerator PauseFollow()
    { 

        // ���� �������� ������ �� ���� ����� ����
        _enemy._range = 0;
        // enemy stroll ���� ��ȯ
        _enemy._curState = Enemy.eENEMY_STATE.STROLL;
        // ��� �ð� �� ������ ����
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


    // Random���� ���ϴ� �ڷ�ƾ
    IEnumerator ChooseRandomDirection()
    {
        _chooseDir = true;
        yield return new WaitForSeconds(Random.Range(_randomsetMinTime, _randomsetMaxTime));
        _direction = GetRandomVector2();
        _chooseDir = false;
    }

    // ���� ���� ���� retrun
    Vector2 GetRandomVector2()
    {
        Vector2 randomDir;

        do
            randomDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        //(0,0)������ �� �ٽ� �ݺ���
        while (randomDir.x == 0 && randomDir.y == 0);

        //�������� ��ȯ
        return randomDir;
    }

    // �浹 �˻�
    public void isWall()
    {
        // 1) ������ �浹 �˻�
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

    // Enemy ������ �浹�� ������ �������� ���̵�
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _direction = GetRandomVector2();
            _enemy.Move();
        }

    }



    // �� �浹�� ��� ���� �ڷ�ƾ
    public IEnumerator DownSpeed(float speed)
    {
        _enemy._speed = 0f;
        yield return new WaitForSeconds(0.035f);
        _enemy._speed = speed;
    }

        // �浹 ȸ�� , ������Ī �̿�
       public IEnumerator ReturnFromWall()
    {
        // �浹 ���� ���� = collisionDir
        Vector2 collisionDir = _direction;

        // ������Ī ���� = originDir
        Vector2 originDir = -1 * collisionDir;

        // �浹 �ݴ� �������� ���� ����, ������
        _direction = originDir;
        _enemy.Move();

        // 0.05��  �� �������� ���� 
        yield return new WaitForSeconds(0.05f);

        // �������� ���� = randomDir
        Vector2 randomDir = new Vector2(0f,0f);

        // ������ ������ ���Ͱ� 1��и鿡 ��ġ�Ҷ�
        if (originDir.x > 0 && originDir.y > 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(0f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 1f);
        }

        // 2��и� ��ġ
        else if (originDir.x < 0 && originDir.y > 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(0f, 1f);
        }

        // 3��и� ��ġ
        else if (originDir.x < 0 && originDir.y < 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 1f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 0f);
        }

        // 4��и� ��ġ
        else if (originDir.x > 0 && originDir.y < 0)
        {
            randomDir.x = Random.Range(-1f, 1f);
            if (randomDir.x <= 0)
                randomDir.y = Random.Range(-1f, 0f);
            else if (randomDir.x > 0)
                randomDir.y = Random.Range(-1f, 1f);
        }

        // x�� ��ġ
        else if (originDir.y == 0)
        {
            if (originDir.x > 0)
            {
                do
                {
                    randomDir.x = Random.Range(0f, 1f);
                    randomDir.y = Random.Range(-1f, 1f);
                }
                // 0,0 �� ������ �ٽ� �����Ѵ�.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

            else if(originDir.x <0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 0f);
                    randomDir.y = Random.Range(-1f, 1f);
                }
                // 0,0 �� ������ �ٽ� �����Ѵ�.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

        }//else if(originDir.y == 0)

        // y�� ��ġ
        else if (originDir.x == 0)
        {
            if (originDir.y > 0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 1f);
                    randomDir.y = Random.Range(0f, 1f);
                }
                // 0,0 �� ������ �ٽ� �����Ѵ�.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

            else if(originDir.y<0)
            {
                do
                {
                    randomDir.x = Random.Range(-1f, 1f);
                    randomDir.y = Random.Range(-1f, 0f);
                }
                // 0,0 �� ������ �ٽ� �����Ѵ�.
                while (randomDir.x == 0 && randomDir.y == 0);
            }

        }//else if(originDir.x == 0)



        _direction = randomDir.normalized;

    }



}
