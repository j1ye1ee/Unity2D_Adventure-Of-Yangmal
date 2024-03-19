using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Singleton<PlayerStatus>
{
    // �÷��̾� ����( hp, ���ݷ�, Ư�� ����, ���� ��) �� ���� ���� Ŭ����
    // �÷��̾��� ���� & ������� ���¸� ������
    // �÷��̾� ���� = ALIVE, DIE
    public enum ePLAYER_STATE
    {
        ALIVE,
        DIE
    }

    public ePLAYER_STATE _curState { get; private set; }

    // �÷��̾� hp
    public float _hp{ get; private set; }
    public float _maxHp { get; private set; }
    HpManager _hpManager;

    // �÷��̾� ���ݷ�
    public float _damage { get; private set; }

    // �÷��̾� ����
    public float _def {get; private set;}

    // �÷��̾� ���� ����
    bool _isDoingDie = false;
    GameObject _playerDieUi;
    SpriteRenderer _spriteRenderer;
    
    // �÷��̾� ���� ����
    public bool _isAttacked = false;
    bool _isDoingStun = false;
    public bool _isStun = false;

    public Vector2 _afterAttackedDir;

    // �÷��̾� ȿ����
    public AudioSource _damageAudio;



    void Start()
    {
        // �÷��̾� & ui �ʱ�ȭ
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
        // ���� �˻� & ����ȿ��
        IsDead();

        // ���� ���� ���� && ���� �ڷ�ƾ ���� ���̶��
        if (_isStun && !_isDoingStun)
            StartCoroutine(Stun());

        switch(_curState)
        {
           // 1 ) ALIVE ����
            case ePLAYER_STATE.ALIVE:
                break;

           // 2 ) DIE ����
            case ePLAYER_STATE.DIE:
                // ���� �ڷ�ƾ ����
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

    // �÷��̾� ���� �˻�
    public void IsDead()
    {
        // �÷��̾�hp<=0  &&  ALIVE ���¶�� ���� �ڷ�ƾ ����
        if (_hp <= 0 && _curState == ePLAYER_STATE.ALIVE)
        {
            _hp = 0;
            _curState = ePLAYER_STATE.DIE;
        }
    }

    // �÷��̾� ���� �ڷ�ƾ
     IEnumerator Die()
     {
        Debug.Log("���� �ڷ�ƾ");
            // ���� �ڷ�ƾ �� �� ������ ����
            _isDoingDie = true;
            float _a = 1f;

            // ���̵� �ƿ� 
            while (_a >= 0)
            {
                _a -= 0.05f;
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
                yield return new WaitForSeconds(0.05f);
            }


        // �÷��̾� ���� ui open
        StartCoroutine(_playerDieUi.GetComponent<EndStageUi>().Flow());

        // ���ӿ��� ó��
        GameManager.Instance.SetIsGameOver(true);

     }

    // �÷��̾� HP ����_���ݹ޾��� ��
    public void PlayerGetDamage(float damage)
    {
        _damageAudio.Play();

        // �÷��̾� ���� ����
        damage = damage * (1f- _def);

        // �÷��̾ ���� ���¶�� �������� ���� �ʰ� �Ѵ�.
        if (_isStun)
            return;

        // ���� ���°� �ƴ϶�� �������� �޴´�.
        else
        {
            if (_hp - damage <= 0)
                _hp = 0;

            else if (_hp - damage > 0)
                _hp -= damage;

            _hpManager.HeartSet();
            // ���� ���� on
            _isStun = true;
        }
    }


    // �÷��̾� ����&���� ����
    // ���� �ڷ�ƾ
    protected IEnumerator Stun()
    {
        // ���� �ڷ�ƾ �ѹ��� ������ ����
        _isDoingStun = true;

        // �����̴� ȿ�� ����
        float _a = 0;

        // 6�� ������
        for (int count = 0; count < 6; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.15f);
        }

        // ���� �ڷ�ƾ �� ���� ������ ����
        _isDoingStun = false;

        // ���� ���� ��� 
        _isStun = false;

    }


    // �÷��̾��� �ִ� ü�� ���
    public void PlayerGetMaxHp(float hp)
    {
        _maxHp += hp;
        _hpManager.HeartSet();
    }

    // �÷��̾��� �ִ� ü�� ����
    public void PlayerMaxHpSet(float hp)
    {
        _maxHp = hp;
        _hpManager.HeartSet();
    }

    // �÷��̾� HP ���
    public void PlayerGetHP(float hp)
    {
        _hp += hp;
        _hpManager.HeartSet();
    }

    // �÷��̾� ������ ���
    public void DamageUp(float damage)
    {
        _damage += damage;
    }

    // �÷��̾� ������ ����
    public void DamageSet(float damage)
    {
        _damage = damage;
    }

    // �÷��̾� hp ����
    public void PlayerHpSet(float hp)
    {
        _hp = hp;
        _hpManager.HeartSet();
    }

    // �÷��̾� ���� ����
    public void PlayerDexSet(float dex)
    {
        _def = dex;
    }

    // �÷��̾� ��� ��
    public void PlayerDexUp(float dex)
    {
        _def += dex;
    }

    // �÷��̾� ���� �ʱ�ȭ
    public void PlayerSpriteReset()
    {
        float a = 255;
        _spriteRenderer.color = new Color(
            _spriteRenderer.color.r,
            _spriteRenderer.color.g,
            _spriteRenderer.color.b,
            a);

    }


    // PlayerStatus class ���� ���� �ʱ�ȭ
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

    // ��Ȱ ġƮŰ �޼���
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
