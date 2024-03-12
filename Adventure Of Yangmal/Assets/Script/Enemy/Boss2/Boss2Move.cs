using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Move : MonoBehaviour
{
    Rigidbody2D _myRigid;
    public float _moveSpeed;
    public float _originSpeed;
    public bool _isWall;
    Vector2 _proDirection; // ������ �����̴� ���� ����

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



    // ������ �浹 �˻� --> �浹�� isWall = true;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "World")
            _isWall = true;
        else
            _isWall = false;
    }

    // Boss2 �� ������1
    void NormalMove()
    {
        // ���� ���ٸ� _proDirection  ���� ���� �̵� 
        if (!_isWall)
        {
            _myRigid.velocity = _proDirection * _moveSpeed;
        }


        else if (_isWall)
        {
            if (_proDirection == Vector2.right)
            {
                // ������ ������ �̹��� ������
                transform.localScale = new Vector2(-1, 1);
                // �������� �̵�
                _myRigid.velocity = Vector2.left * _moveSpeed;
                _proDirection = Vector2.left;
                _isWall = false;
            }

            else if (_proDirection == Vector2.left)
            {
                // �������� ������ �̹��� ������
                transform.localScale = new Vector2(1, 1);

                // ���������� �̵�
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

        // startMove ���� ���̶�� 
        if (!_isDoingPage2Startmove)
        {
            _isDoingPage2Startmove = true;
            // page2Move ����
            StartCoroutine(GotoOrigin_Shake());
        }


        // shake ���� �� ���ѽñ�
        else if (_isDoneShake)
            CrushWall();

        // ���μ��� �Ϸ� �� ������ġ���� ������ ����
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
        // �÷��̾�  �ݶ��̴� ���� & �������� ���ϰ� �ϱ�(������ �÷��̾�� �ظ� ��ġ�� �ʱ� ����)
        _player.GetComponent<Collider2D>().enabled = false;
        _originPlayerMoveSpeed = _player.GetComponent<PlayerMove>()._moveSpeed;
        _player.GetComponent<PlayerMove>()._moveSpeed = 0f;


        // 0) origin position ���� �̵�
        _myRigid.velocity = _pause;

        // ī�޶� ��� ��ȯ
        Boss2CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<Boss2CameraController>();
        camera._curMode = MainCameraMove.eCAMERA_MODE.BOSS2_FOLLOW;

        while (!_isOriginPosition)
        {
            Vector2 direction = _origin - _myRigid.position;
            _myRigid.velocity = direction.normalized * _moveSpeed;
            yield return null;
        }

        // boss2 ���� & �ð��ʱ�ȭ
        _myRigid.velocity = _pause;
        _checkPauseTime = 0;

        // origin ��ġ �������� ���
        yield return new WaitUntil(() => _isOriginPosition == true);

        // sake ����
        StartCoroutine(Shake());

    }

    IEnumerator Shake()
    {
        Vector2 origin = _myRigid.position;
        Vector2 random = new Vector2(0f, 0f);
        Vector2 proRandom = new Vector2(0f, 0f);

        // ���� �ð� ���� shake �� ����
        while (_checkPauseTime < _pauseTime - 0.25f)
        {
            // 1 ) Shake ���� ����
            // ������ �������� �������ٸ� ����������
            if (proRandom.x < 0)
                random = new Vector2(Random.Range(0f, 0.5f), 0f);

            // ������ ���������� �������ٸ� ��������
            else if (proRandom.x > 0)
                random = new Vector2(Random.Range(-0.5f, 0f), 0f);

            // ������ �������� �ʾҴٸ� �¿� �� ����������
            else if (proRandom.x == 0)
                random = new Vector2(Random.Range(-0.5f, 0.5f), 0f);

            // 2 ) Shake ���� ��ŭ �̵�
            _myRigid.position = Vector2.Lerp(origin, origin + random, 0.5f);

            // 3 ) �̵��� ���� proRandom �� ����
            proRandom = random;

            // 4 ) ���� �����ӱ��� ���
            yield return null;
        }
        // 5 ) ������Ʈ ��ġ ���󺹱�
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

        // ī�޶� ��� ��ȯ
        Boss2CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<Boss2CameraController>();
        camera._curMode = MainCameraMove.eCAMERA_MODE.NORMAL;

        // ����Ʈ���� �̵� & ī�޶� �����ȯ �Ϸ�� stage ���� ����
        _stageManager._curState = Boss2_StageManager.eSTAGE_STATE.PAGE2;

        // �÷��̾�  �ݶ��̴� �ѱ� & ���ǵ� ����
        _player.GetComponent<Collider2D>().enabled = true;
        _player.GetComponent<PlayerMove>()._moveSpeed = _originPlayerMoveSpeed;


        Debug.Log("page2 ����");

        // ���� ���� �ٲٱ� 
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
        // 3�� ���
        yield return new WaitForSeconds(3f);

        //moveTime �ʱ�ȭ
        _page2MoveTime = 0;

        Vector2 direction;
        direction = (_player.transform.position - gameObject.transform.position).normalized;
        
        // endMoveTime ���� �̵�
        while(_page2MoveTime < _endMoveTime)
        {
            _myRigid.velocity = _moveSpeed * direction;
            yield return null;
        }

        // MoveTime �ʱ�ȭ
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
