using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Enemy
{

    public GameObject _deadEffect;
    public GameObject _babyEnemy;
    public bool _isDeadEffectDoing = false;
    public bool _firstLook = false;
    Enemy6BabyPoolManager _poolManager;

    public override void Move()
    {
        base.Move();
        // 얼굴 방향 먼저 설정
        SetFace();
        // 이동
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    void Awake()
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
        // 캐싱
        _player = GameObject.FindWithTag("Player");
        _playerStatus = PlayerStatus.Instance;
        _poolManager = GameObject.FindWithTag("enemy6BabyPool").GetComponent<Enemy6BabyPoolManager>();
    }


    void Update()
    {
        // 죽음 검사
        IsDead();

        // 처음으로 player를 목격 && Follow 상태라면 range범위를 넓힌다.
        // 처음 player 목격 후에는 계속해서 enemy6가 따라붙는 효과
        if(_firstLook && _curState == eENEMY_STATE.FOLLOW)
        {
            setRange = 20f;
            _range = setRange;
        }

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
                // 목격처리 --> range  확장
                if(!_firstLook)
                    _firstLook = true;

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
                    // 움직임 멈춤
                    _ai.StopMove();
                    // 아기오리 5마리 생성 후 죽음
                    if (!_isDeadEffectDoing)
                    {
                        _isDeadEffectDoing = true;
                        StartCoroutine(Dead_NewBornEnemy());
                    }
                }
                break;
        }//switch(_curState)

    }


    // 얼굴 방향 설정
    void SetFace()
    {
        if(_ai._direction.x > 0){transform.localScale = new Vector3(1, 1, 1);}
        if(_ai._direction.x < 0){transform.localScale = new Vector3(-1, 1, 1);}
        if(_ai._direction.x == 0){transform.localScale = new Vector3(1, 1, 1);}
    }


    IEnumerator Dead_NewBornEnemy()
    {
        // 죽음효과 생성
        GameObject deadEffect =
            Instantiate(_deadEffect, transform.position, transform.rotation);

        // Enemy6 콜라이더 끄기
        gameObject.GetComponent<Collider2D>().enabled = false;

        // Enemy6 페이드아웃
        StartCoroutine(EffectFadeOut(_spriteRenderer));

        for (int count = 0; count < 5; count++)
        {
            // pooling 이용 baby 생성
            GameObject babyEnemy = _poolManager.GetObject(
                _poolManager._babyPool,
                _poolManager._baby,
                transform.position,
                transform.rotation);

            yield return new WaitForSeconds(0.2f);
        }

        // 죽음 효과 페이드아웃 & 본체 setActive false
        StartCoroutine(DeadEffectFadeOut(deadEffect.GetComponent<SpriteRenderer>()));
        _firstLook = false;
    }

    IEnumerator EffectFadeOut(SpriteRenderer _sprite)
    {
        float _a = 1f;

        // 페이드 아웃 
        while (_a >= 0)
        {
            _a -= 0.05f;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator DeadEffectFadeOut(SpriteRenderer _sprite)
    {
        float _a = 1f;

        // 페이드 아웃 
        while (_a >= 0)
        {
            _a -= 0.05f;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // Enemy 부모클래스의 die 코루틴을 사용하지 않음
        // 이 죽음 코루틴으로 e6 죽음 관련 변수 초기화 & setActive false
        _isDoingDie = false;
        _isDeadEffectDoing = false;


        _spriteRenderer.color = _originColor;
        _hp = setHp;
        _curState = eENEMY_STATE.STROLL;
        _isDead = false;
        _isDoingDie = false;
        _isStun = false;
        _isDoingStun = false;
        _speed = setSpeed;
        GetComponent<Collider2D>().enabled = true;

        // 스포너 링크 시
        if (GetComponent<SpawnerLink>() != null)
            GetComponent<SpawnerLink>()._isMinus = false;


        // 효과 destroy
        Destroy(_sprite.gameObject);
        gameObject.SetActive(false);
    }

}
