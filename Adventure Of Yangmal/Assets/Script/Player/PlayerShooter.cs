
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    enum eSHOOT_DIR
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }

    eSHOOT_DIR _playerShootDir;

    float _curTime = 0; // ���ݽð� �ʽð�

    public float _shootTerm; // ����

    public float _bulletSpeed; // �Ѿ� �ӵ�
    public Vector2 _shootDir;

    public bool _isShoot = false; // �Ѿ� �߻� ������ Ȯ��
    PlayerMove _playerMove;

    PlayerItemManager _playerItem;

    // ���̵� �̻��� ������ ���� ����
    float _gCurTime;
    public float _gShootTime;
    public float _gLaunchForce;


    // ��ź ������ ���� ����
    float _bCurTime;
    public float _bShootTime;
    public float _bLaunchForce;

    // pool Manager ����
    PlayerBulletPoolManager _poolManager;

    // ���� ȿ����
    public AudioSource _missileShoot;
    public AudioSource _catBallShoot;
    public AudioSource _BombShoot;


    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerItem = GetComponent<PlayerItemManager>();
        _poolManager = GameObject.FindWithTag("playerBulletPool").GetComponent<PlayerBulletPoolManager>();
    }


    void Update()
    {
        // ���� �ð��� ���Ӻ��� ���� ���� ��쿡�� �߻� ����
        // �߻� �ÿ��� ���� ���� = true

        if ((Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.W)))
            _isShoot = true;


        // �߻� ��ư�� ���� �÷��̾� ���� ����
        SetShootDir();

        // *_isShoot ���¿����� �߻� �� �ִϸ��̼�
        if (_isShoot)
        {
            // 1) �÷��̾� ���⿡ ���� �ִϸ��̼� ����
            SetShootAni(_playerShootDir);

            // 2) ��Ÿ�� ���� �߻�
            if (_curTime >= _shootTerm)
                PlayerShoot(_playerShootDir);

            // 3) ���̵�̻��� ������ ���̵� �̻��� ��Ÿ�� ���� �߻�
            if (_playerItem._isGuideMissile)
            {
                if (_gCurTime >= _gShootTime)
                {
                    ShootGuideMissile(_shootDir);
                }

            }

            // 4) ��ź ������ ��ź ��Ÿ�� ���� �߻�
            if(_playerItem._isBomb)
            {
                if (_bCurTime >= _bShootTime)
                    ShootBomb(_shootDir);
            }


            // �߻簡 ������ ���� ���� = false
            if ((Input.GetKeyUp(KeyCode.A)) ||
                (Input.GetKeyUp(KeyCode.D)) ||
                (Input.GetKeyUp(KeyCode.S)) ||
                (Input.GetKeyUp(KeyCode.W)))
            {
                _playerMove.NoneAni();
                _isShoot = false;
            }

            // curTime ���ϱ�
            _curTime += Time.deltaTime;

            // guideMissile ������ �������̶�� gCurTime ���ϱ�
            if (_playerItem._isGuideMissile)
                _gCurTime += Time.deltaTime;

            // bomb ������ �������̶�� bCurTime ���ϱ�
            if (_playerItem._isBomb)
                _bCurTime += Time.deltaTime;

        }
    }

    // �⺻ ����
    void Shoot(Vector2 ShootDirection) // ���� ������ �Ű������� �޴� �޼���
    {
        // ������Ʈ Ǯ���� �Ѿ� �뿩
        GameObject bullet
            = _poolManager.GetObject(
                _poolManager._playerBulletPool,
                _poolManager._playerBulletPrefab,
                transform.position,
                transform.rotation);

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(ShootDirection * _bulletSpeed, ForceMode2D.Impulse); // �Ѿ� �߻�

        _catBallShoot.Play();

        _curTime = 0; // �����Ŀ��� �ʽð踦 0���� ������.
    }


    // ���̵� �̻��� ����
    void ShootGuideMissile(Vector2 ShootDirection)
    {

        Quaternion rotation;


        if (ShootDirection == Vector2.left)
            rotation = Quaternion.Euler(0, 0, 90);
        else if (ShootDirection == Vector2.down)
            rotation = Quaternion.Euler(0, 0, 180);
        else if (ShootDirection == Vector2.right)
            rotation = Quaternion.Euler(0, 0, 270);
        else
            rotation = transform.rotation;

        // ������Ʈ Ǯ���� ���̵� �̻��� �뿩
        GameObject guideMissile = _poolManager.GetObject(
            _poolManager._guideMissilePool,
            _poolManager._guideMissilePrefab,
            transform.position,
            rotation);

        _gLaunchForce = guideMissile.GetComponent<GuideMissile>()._launchForce;

        Rigidbody2D bulletRigid = guideMissile.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(ShootDirection *_gLaunchForce , ForceMode2D.Impulse);

        _missileShoot.Play();

        _gCurTime = 0;
    }


    // ��ź ����
    void ShootBomb(Vector2 ShootDirection)
    {
        // ������Ʈ Ǯ���� bomb �뿩
        GameObject bomb = _poolManager.GetObject(
            _poolManager._bombPool,
            _poolManager._bombPrefab,
            transform.position,
            transform.rotation);


        Rigidbody2D bulletRigid = bomb.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(ShootDirection * _bLaunchForce, ForceMode2D.Impulse);
        StartCoroutine(bomb.GetComponent<Bomb>().BombEffect());

        _BombShoot.Play();


        _bCurTime = 0;
    }

    // ���ÿ� Ű�� �������� �������� ���� �߻� Ű�� ���� �÷��̾� �� ���� ����
    void SetShootDir()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _playerShootDir = eSHOOT_DIR.LEFT;

        else if (Input.GetKeyDown(KeyCode.D))
            _playerShootDir = eSHOOT_DIR.RIGHT;

        else if (Input.GetKeyDown(KeyCode.S))
            _playerShootDir = eSHOOT_DIR.DOWN;

        else if (Input.GetKeyDown(KeyCode.W))
            _playerShootDir = eSHOOT_DIR.UP;
    }


    // �÷��̾� �� ���⿡ ���� �ִϸ��̼� ��ü
   void SetShootAni(eSHOOT_DIR shootDir)
    {
        if (shootDir == eSHOOT_DIR.LEFT)
            _playerMove.LeftWalkAni();

        else if (shootDir == eSHOOT_DIR.RIGHT)
            _playerMove.RightWalkAni();

        else if (shootDir == eSHOOT_DIR.UP)
            _playerMove.BackWalkAni();

        else if (shootDir == eSHOOT_DIR.DOWN)
            _playerMove.FrontWalkAni();

    }

    // �÷��̾��� �� ���⿡ ���� �߻�
    void PlayerShoot(eSHOOT_DIR shootDir)
    {
        if (shootDir == eSHOOT_DIR.LEFT)
        {
            Shoot(Vector2.left);
            _shootDir = Vector2.left;
        }

        else if (shootDir == eSHOOT_DIR.RIGHT)
        {
            Shoot(Vector2.right);
            _shootDir = Vector2.right;
        }

        else if (shootDir == eSHOOT_DIR.UP)
        {
            Shoot(Vector2.up);
           _shootDir = Vector2.up;
        }

        else if (shootDir == eSHOOT_DIR.DOWN)
        {
            Shoot(Vector2.down);
            _shootDir = Vector2.down;
        }
    }

    public void ResetPlayerShoot()
    {
        _shootTerm = 0.5f;
        _bulletSpeed = 15;
        _isShoot = false;
        _gShootTime = 3;
        _bShootTime = 5;
        _bLaunchForce = 10;
    }

}
