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
        // �� ���� ���� ����
        SetFace();
        // �̵�
        _myRigid.velocity = _ai._direction.normalized * _speed;
    }

    void Awake()
    {
        // �� ü��, �����, ���ǵ�, �������� ����
        SetEnemy(setHp, setDamage, setSpeed, setRange);

        _ai = GetComponent<EnemyMoveAi>();
        _myRigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;

    }

    private void Start()
    {
        // ĳ��
        _player = GameObject.FindWithTag("Player");
        _playerStatus = PlayerStatus.Instance;
        _poolManager = GameObject.FindWithTag("enemy6BabyPool").GetComponent<Enemy6BabyPoolManager>();
    }


    void Update()
    {
        // ���� �˻�
        IsDead();

        // ó������ player�� ��� && Follow ���¶�� range������ ������.
        // ó�� player ��� �Ŀ��� ����ؼ� enemy6�� ����ٴ� ȿ��
        if(_firstLook && _curState == eENEMY_STATE.FOLLOW)
        {
            setRange = 20f;
            _range = setRange;
        }

        // ���� ���� ���� �˻�
        if (_isStun && _curState != eENEMY_STATE.DIE)
        {
            _curState = eENEMY_STATE.STUN;
        }

        // �÷��̾� ���� �˻� --  �÷��̾ ���� �����϶� STROLL ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE)
            _curState = eENEMY_STATE.STROLL;

        switch (_curState)
        {
            // 1) STROLL ����
            case (eENEMY_STATE.STROLL):
                // �÷��̾ ���� �ȿ� ���Դ��� Ȯ��
                // �� ���°� DIE ���� Ȯ��
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                {
                    // FOLLOW���� ����
                    _curState = eENEMY_STATE.FOLLOW;
                }
                _ai.Stroll();
                break;

            // 2 ) FOLLOW ����
            case (eENEMY_STATE.FOLLOW):
                // ���ó�� --> range  Ȯ��
                if(!_firstLook)
                    _firstLook = true;

                // �÷��̾ ���� �ȿ� ����ִ��� ��� Ȯ��
                if (IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _ai.Follow();

                // �÷��̾ ���� ������ �����ٸ� STROLL
                else if (!IsPlayerInRange(_range) && _curState != eENEMY_STATE.DIE)
                    _curState = eENEMY_STATE.STROLL;
                break;

            // 3 ) STUN ����
            case (eENEMY_STATE.STUN):
                if (!_isDoingStun) // ���� �ڷ�ƾ�� ���۵��� �ʾҴٸ�
                    StartCoroutine(Stun());
                // ���� �ڷ�ƾ�� �������̶�� ȣ������ �ʴ´�.
                break;


            // 4) DIE ����
            case (eENEMY_STATE.DIE):
                // ���� �ڷ�ƾ�� �������� ���� ���¶��
                if (!_isDoingDie)
                {
                    // ������ ����
                    _ai.StopMove();
                    // �Ʊ���� 5���� ���� �� ����
                    if (!_isDeadEffectDoing)
                    {
                        _isDeadEffectDoing = true;
                        StartCoroutine(Dead_NewBornEnemy());
                    }
                }
                break;
        }//switch(_curState)

    }


    // �� ���� ����
    void SetFace()
    {
        if(_ai._direction.x > 0){transform.localScale = new Vector3(1, 1, 1);}
        if(_ai._direction.x < 0){transform.localScale = new Vector3(-1, 1, 1);}
        if(_ai._direction.x == 0){transform.localScale = new Vector3(1, 1, 1);}
    }


    IEnumerator Dead_NewBornEnemy()
    {
        // ����ȿ�� ����
        GameObject deadEffect =
            Instantiate(_deadEffect, transform.position, transform.rotation);

        // Enemy6 �ݶ��̴� ����
        gameObject.GetComponent<Collider2D>().enabled = false;

        // Enemy6 ���̵�ƿ�
        StartCoroutine(EffectFadeOut(_spriteRenderer));

        for (int count = 0; count < 5; count++)
        {
            // pooling �̿� baby ����
            GameObject babyEnemy = _poolManager.GetObject(
                _poolManager._babyPool,
                _poolManager._baby,
                transform.position,
                transform.rotation);

            yield return new WaitForSeconds(0.2f);
        }

        // ���� ȿ�� ���̵�ƿ� & ��ü setActive false
        StartCoroutine(DeadEffectFadeOut(deadEffect.GetComponent<SpriteRenderer>()));
        _firstLook = false;
    }

    IEnumerator EffectFadeOut(SpriteRenderer _sprite)
    {
        float _a = 1f;

        // ���̵� �ƿ� 
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

        // ���̵� �ƿ� 
        while (_a >= 0)
        {
            _a -= 0.05f;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // Enemy �θ�Ŭ������ die �ڷ�ƾ�� ������� ����
        // �� ���� �ڷ�ƾ���� e6 ���� ���� ���� �ʱ�ȭ & setActive false
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

        // ������ ��ũ ��
        if (GetComponent<SpawnerLink>() != null)
            GetComponent<SpawnerLink>()._isMinus = false;


        // ȿ�� destroy
        Destroy(_sprite.gameObject);
        gameObject.SetActive(false);
    }

}
