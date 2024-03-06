
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

    float _curTime = 0; // 공격시간 초시계

    public float _shootTerm; // 공속

    public float _bulletSpeed; // 총알 속도
    public Vector2 _shootDir;

    public bool _isShoot = false; // 총알 발사 중인지 확인
    PlayerMove _playerMove;

    PlayerItemManager _playerItem;

    // 가이드 미사일 아이템 관련 변수
    float _gCurTime;
    public float _gShootTime;
    public float _gLaunchForce;


    // 폭탄 아이템 관련 변수
    float _bCurTime;
    public float _bShootTime;
    public float _bLaunchForce;

    // pool Manager 참조
    PlayerBulletPoolManager _poolManager;

    // 슈팅 효과음
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
        // 현재 시간이 공속보다 지나 있을 경우에만 발사 가능
        // 발사 시에는 슈팅 상태 = true

        if ((Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.W)))
            _isShoot = true;


        // 발사 버튼에 따른 플레이어 방향 설정
        SetShootDir();

        // *_isShoot 상태에서만 발사 및 애니메이션
        if (_isShoot)
        {
            // 1) 플레이어 방향에 따른 애니메이션 설정
            SetShootAni(_playerShootDir);

            // 2) 쿨타임 차면 발사
            if (_curTime >= _shootTerm)
                PlayerShoot(_playerShootDir);

            // 3) 가이드미사일 있을시 가이드 미사일 쿨타임 차면 발사
            if (_playerItem._isGuideMissile)
            {
                if (_gCurTime >= _gShootTime)
                {
                    ShootGuideMissile(_shootDir);
                }

            }

            // 4) 폭탄 있을시 폭탄 쿨타임 차면 발사
            if(_playerItem._isBomb)
            {
                if (_bCurTime >= _bShootTime)
                    ShootBomb(_shootDir);
            }


            // 발사가 끝나면 슈팅 상태 = false
            if ((Input.GetKeyUp(KeyCode.A)) ||
                (Input.GetKeyUp(KeyCode.D)) ||
                (Input.GetKeyUp(KeyCode.S)) ||
                (Input.GetKeyUp(KeyCode.W)))
            {
                _playerMove.NoneAni();
                _isShoot = false;
            }

            // curTime 더하기
            _curTime += Time.deltaTime;

            // guideMissile 아이템 적용중이라면 gCurTime 더하기
            if (_playerItem._isGuideMissile)
                _gCurTime += Time.deltaTime;

            // bomb 아이템 적용중이라면 bCurTime 더하기
            if (_playerItem._isBomb)
                _bCurTime += Time.deltaTime;

        }
    }

    // 기본 슈팅
    void Shoot(Vector2 ShootDirection) // 슈팅 방향을 매개변수로 받는 메서드
    {
        // 오브젝트 풀에서 총알 대여
        GameObject bullet
            = _poolManager.GetObject(
                _poolManager._playerBulletPool,
                _poolManager._playerBulletPrefab,
                transform.position,
                transform.rotation);

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(ShootDirection * _bulletSpeed, ForceMode2D.Impulse); // 총알 발사

        _catBallShoot.Play();

        _curTime = 0; // 공격후에는 초시계를 0으로 돌린다.
    }


    // 가이드 미사일 슈팅
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

        // 오브젝트 풀에서 가이드 미사일 대여
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


    // 폭탄 슈팅
    void ShootBomb(Vector2 ShootDirection)
    {
        // 오브젝트 풀에서 bomb 대여
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

    // 동시에 키를 누르더라도 마지막에 누른 발사 키에 따른 플레이어 몸 방향 설정
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


    // 플레이어 몸 방향에 따른 애니메이션 교체
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

    // 플레이어의 몸 방향에 따른 발사
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
