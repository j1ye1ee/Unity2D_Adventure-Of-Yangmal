using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Singleton<PlayerStatus>
{
    // 플레이어 스텟( hp, 공격력, 특수 공격, 버프 등) 및 생명 관리 클래스
    // 플레이어의 죽음 & 살아있음 상태를 관리함
    // 플레이어 상태 = ALIVE, DIE
    public enum ePLAYER_STATE
    {
        ALIVE,
        DIE
    }

    public ePLAYER_STATE _curState { get; private set; }

    // 플레이어 hp
    public float _hp{ get; private set; }
    public float _maxHp { get; private set; }
    HpManager _hpManager;

    // 플레이어 공격력
    public float _damage { get; private set; }

    // 플레이어 방어력
    public float _def {get; private set;}

    // 플레이어 죽음 관련
    bool _isDoingDie = false;
    GameObject _playerDieUi;
    SpriteRenderer _spriteRenderer;
    
    // 플레이어 스턴 관련
    public bool _isAttacked = false;
    bool _isDoingStun = false;
    public bool _isStun = false;

    public Vector2 _afterAttackedDir;

    // 플레이어 효과음
    public AudioSource _damageAudio;



    void Start()
    {
        // 플레이어 & ui 초기화
        SetPlayer();
        UiManager.Instance.SetPlayerDamage();
        UiManager.Instance.SetPlayerDex();
        UiManager.Instance.SetPlayerGold();

        _curState = ePLAYER_STATE.ALIVE;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hpManager = GameObject.FindWithTag("HpManager").GetComponent<HpManager>();
        _playerDieUi = GameObject.FindWithTag("Stage Fail Ui");
    }


    void Update()
    {
        // 죽음 검사 & 죽음효과
        IsDead();

        // 스턴 상태 진입 && 스턴 코루틴 시작 전이라면
        if (_isStun && !_isDoingStun)
            StartCoroutine(Stun());

        switch(_curState)
        {
           // 1 ) ALIVE 상태
            case ePLAYER_STATE.ALIVE:
                break;

           // 2 ) DIE 상태
            case ePLAYER_STATE.DIE:
                // 죽음 코루틴 실행
                if (_isDoingDie == false)
                    StartCoroutine(Die());

                break;
        }
    }

    void SetPlayer()
    {
        _hp = 60;
        _maxHp = 60;
        _damage = 10;
        _def = 0;

    }

    // 플레이어 죽음 검사
    public void IsDead()
    {
        // 플레이어hp<=0  &&  ALIVE 상태라면 죽음 코루틴 실행
        if (_hp <= 0 && _curState == ePLAYER_STATE.ALIVE)
        {
            _hp = 0;
            _curState = ePLAYER_STATE.DIE;
        }
    }

    // 플레이어 죽음 코루틴
     IEnumerator Die()
     {
        Debug.Log("죽음 코루틴");
            // 죽음 코루틴 한 번 실행을 위함
            _isDoingDie = true;
            float _a = 1f;

            // 페이드 아웃 
            while (_a >= 0)
            {
                _a -= 0.05f;
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
                yield return new WaitForSeconds(0.05f);
            }


        // 플레이어 죽음 ui open
        StartCoroutine(_playerDieUi.GetComponent<EndStageUi>().Flow());

        // 게임오버 처리
        GameManager.Instance.SetIsGameOver(true);

     }

    // 플레이어 HP 차감_공격받았을 시
    public void PlayerGetDamage(float damage)
    {
        _damageAudio.Play();

        // 플레이어 방어력 적용
        damage = damage * (1f- _def);

        // 플레이어가 스턴 상태라면 데미지를 받지 않게 한다.
        if (_isStun)
            return;

        // 스턴 상태가 아니라면 데미지를 받는다.
        else
        {
            if (_hp - damage <= 0)
                _hp = 0;

            else if (_hp - damage > 0)
                _hp -= damage;

            _hpManager.HeartSet();
            // 스턴 상태 on
            _isStun = true;
        }
    }


    // 플레이어 스턴&무적 상태
    // 스턴 코루틴
    protected IEnumerator Stun()
    {
        // 스턴 코루틴 한번만 실행을 위함
        _isDoingStun = true;

        // 깜빡이는 효과 변수
        float _a = 0;

        // 6번 깜빡임
        for (int count = 0; count < 6; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.15f);
        }

        // 스턴 코루틴 한 번만 실행을 위함
        _isDoingStun = false;

        // 스턴 상태 벗어남 
        _isStun = false;

    }


    // 플레이어의 최대 체력 상승
    public void PlayerGetMaxHp(float hp)
    {
        _maxHp += hp;
        _hpManager.HeartSet();
    }

    // 플레이어의 최대 체력 세팅
    public void PlayerMaxHpSet(float hp)
    {
        _maxHp = hp;
        _hpManager.HeartSet();
    }

    // 플레이어 HP 상승
    public void PlayerGetHP(float hp)
    {
        _hp += hp;
        _hpManager.HeartSet();
    }

    // 플레이어 데미지 상승
    public void DamageUp(float damage)
    {
        _damage += damage;
    }

    // 플레이어 데미지 세팅
    public void DamageSet(float damage)
    {
        _damage = damage;
    }

    // 플레이어 hp 세팅
    public void PlayerHpSet(float hp)
    {
        _hp = hp;
        _hpManager.HeartSet();
    }

    // 플레이어 방어력 세팅
    public void PlayerDexSet(float dex)
    {
        _def = dex;
    }

    // 플레이어 방어 업
    public void PlayerDexUp(float dex)
    {
        _def += dex;
    }

    // 플레이어 투명도 초기화
    public void PlayerSpriteReset()
    {
        float a = 255;
        _spriteRenderer.color = new Color(
            _spriteRenderer.color.r,
            _spriteRenderer.color.g,
            _spriteRenderer.color.b,
            a);

    }


    // PlayerStatus class 관련 상태 초기화
    public void PlayerStatusAllReset()
    {
        GetComponent<Animator>().enabled = true;

        _curState = ePLAYER_STATE.ALIVE;

        _hp = 60;
        _maxHp = 60;
        _damage = 10;
        _def = 0;
        
        _isDoingDie = false;
        _isAttacked = false;
        _isDoingStun = false;
        _isStun = false;

        PlayerSpriteReset();
        _hpManager.HpManagerReset();


    }

    // 부활 치트키 메서드
    public void PlayerReviveSet()
    {
        _curState = ePLAYER_STATE.ALIVE;
        UiManager.Instance.ReturnPlayer(this.gameObject);

        _hp = _maxHp;

        _isDoingDie = false;
        _isAttacked = false;
        _isDoingStun = false;
        _isStun = false;

        PlayerSpriteReset();
        _hpManager.HeartSet();

    }

}
