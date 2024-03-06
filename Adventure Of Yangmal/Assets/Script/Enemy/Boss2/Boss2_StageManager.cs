using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss2_StageManager : BossStageManager
{
    // �������� ���� Ŭ����
    // Spawner, Enemy ��� ������ --> 2������
    // boss2 ������ --> Ŭ����

    public enum eSTAGE_STATE
    {
        NONE,
        ALLSPAWNERDEAD,
        PAGE1CLEAR,
        PAGE2,
        STAGECLEAR
    }

    public eSTAGE_STATE _curState = eSTAGE_STATE.NONE;

    // ���� ���� ����
    public bool _isBoss2Dead = false;

    // ��������Ŭ���� ui
    bool _isUiStart = false;
    GameObject _stageClearUi;

    private void Start()
    {
        _stageClearUi = GameObject.FindWithTag("Stage Success Ui");
    }

    void Update()
    {
        switch(_curState)
        {
            case eSTAGE_STATE.NONE:
                None();
                break;
            case eSTAGE_STATE.ALLSPAWNERDEAD:
                AllSpawnerDead();
                break;
            case eSTAGE_STATE.PAGE1CLEAR:
                break;
            case eSTAGE_STATE.PAGE2:
                Page2Clear();
                break;
            case eSTAGE_STATE.STAGECLEAR:
                StageClear();
                break;
        }
    }



    // spawner ���� �˻�
    void None()
    {
        if (!_isAllSpawnerDead)
            _isAllSpawnerDead = IsAllSpawnerDead();
        else
        {
            _curState = eSTAGE_STATE.ALLSPAWNERDEAD;
            Debug.Log("allSpawnerDead ����");
        }

    }

    // normal enemy �����˻�
    void AllSpawnerDead()
    {
        if (!_isAllEnemyDead)
            _isAllEnemyDead = IsAllEnemyDead();
        else
        {
            _curState = eSTAGE_STATE.PAGE1CLEAR;
            Debug.Log("Page1 Clear ����");
        }
    }

    // boss2 ���� �˻�
    void Page2Clear()
        {
            if (!_isBoss2Dead)
                return;
            else
            {
                _curState = eSTAGE_STATE.STAGECLEAR;
                Debug.Log("StageClear ����");
            }
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
