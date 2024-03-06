using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_StageManager : BossStageManager
{
    // �������� ���� Ŭ����
    // Boss1_1, Boss1_2 ��� ����, Spawner ��� ������ --> Ŭ����

    public enum eSTAGE_STATE
    {
        NONE,
        STAGECLEAR
    }

    public eSTAGE_STATE _curState = eSTAGE_STATE.NONE;


    // ���� 1,2 ����
    public GameObject _boss1_1;
    public GameObject _boss1_2;
    public SpriteRenderer _boss1_1Sp;
    public SpriteRenderer _boss1_2Sp;

    // ���� ���� ȿ���� ����
    public float _deadTime;
    public float _checkDeadTime;

    // ���� ���� ����
    public bool _isDead1 = false; // boss1_1 ����
    public bool _isDead2 = false; // boss1_2 ����
    public bool _isDoingDie = false; 
    public bool _isAllDead = false;

    // ��������Ŭ���� ui
    bool _isUiStart = false;
    GameObject _stageClearUi;

    void Awake()
    {
        _boss1_1 = GameObject.FindWithTag("Boss1_1");
        _boss1_1Sp = _boss1_1.GetComponent<SpriteRenderer>();

        _boss1_2 = GameObject.FindWithTag("Boss1_2");
        _boss1_2Sp = _boss1_2.GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        _stageClearUi = GameObject.FindWithTag("Stage Success Ui");
    }

    void Update()
    {
        switch (_curState)
        {
            case eSTAGE_STATE.NONE:
                // 1) ������ ���� �˻�
                _isAllSpawnerDead = IsAllSpawnerDead();

                // 2) ������ ��� ���� Ȯ�ν� --> ��� ���� ���� �˻�
                if (_isAllSpawnerDead)
                    _isAllEnemyDead = IsAllEnemyDead();

                // 3) boos1_1, 1_2 ���� �˻� & ���� ȿ�� �������� �ǽ�
                if (!_isAllDead)
                    IsBothDead();

                // �Ѵ� �׾�����
                else if (_isAllDead)
                {
                    // ��� �� & �����ʰ� �׾��ٸ� stageClear = true
                    // _curState = stageClear ���·� ����
                    if (_isAllSpawnerDead && _isAllEnemyDead)
                            _curState = eSTAGE_STATE.STAGECLEAR;
                }
                break;


            case eSTAGE_STATE.STAGECLEAR:
                // ����ȿ�� �ڷ�ƾ �� ���� ������ ����
                if (!_isDoingDie)
                {
                    _checkDeadTime += Time.deltaTime;

                    // DeadTime �Ǿ��� �� ���� ȿ�� ����
                    if (_checkDeadTime >= _deadTime)
                    {
                        _isDoingDie = true;

                        // ������ ���� 
                        _boss1_1.GetComponent<Boss1_1Move>()._moveSpeed = 0;
                        _boss1_2.GetComponent<Boss1_2>()._followRate = 0;

                        // ���� �ڷ�ƾ
                        StartCoroutine(Die(_boss1_1Sp));
                        StartCoroutine(Die(_boss1_2Sp));
                    }// if(_checkDeadTime >= _deadTime)

                    // �������� Ŭ���� ui open
                    StageClear();
                }//if(!_isDoingDie)

                break;
        }//switch (_curState)
    }




    // boss1_1, 1_2 �����˻�
    void IsBothDead()
    {
        if (_isDead1 && _isDead2)
            _isAllDead = true;
    }

    // ���� ȿ�� �ڷ�ƾ
    protected IEnumerator Die(SpriteRenderer sprite)
    {
        // ���� �ڷ�ƾ �� �� ������ ����
        _isAllDead = true;
        _isDoingDie = true;
        float _a = 1f;

        // ���̵� �ƿ� 
        while (_a >= 0)
        {
            _a -= 0.05f;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // �ڷ�ƾ ���� �� ������Ʈ ����
        Destroy(sprite.gameObject);
    }

    // �������� Ŭ���� ui open
    void StageClear()
    {
        if (!_isUiStart)
        {
            _isStageClear = true;
            _isUiStart = true;
            StartCoroutine(_stageClearUi.GetComponent<EndStageUi>().Flow());
        }
    }



}

