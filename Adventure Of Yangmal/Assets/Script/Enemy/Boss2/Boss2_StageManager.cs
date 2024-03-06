using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss2_StageManager : BossStageManager
{
    // 스테이지 관리 클래스
    // Spawner, Enemy 모두 죽음시 --> 2페이즈
    // boss2 죽음시 --> 클리어

    public enum eSTAGE_STATE
    {
        NONE,
        ALLSPAWNERDEAD,
        PAGE1CLEAR,
        PAGE2,
        STAGECLEAR
    }

    public eSTAGE_STATE _curState = eSTAGE_STATE.NONE;

    // 생명 관련 변수
    public bool _isBoss2Dead = false;

    // 스테이지클리어 ui
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



    // spawner 죽음 검사
    void None()
    {
        if (!_isAllSpawnerDead)
            _isAllSpawnerDead = IsAllSpawnerDead();
        else
        {
            _curState = eSTAGE_STATE.ALLSPAWNERDEAD;
            Debug.Log("allSpawnerDead 진입");
        }

    }

    // normal enemy 죽음검사
    void AllSpawnerDead()
    {
        if (!_isAllEnemyDead)
            _isAllEnemyDead = IsAllEnemyDead();
        else
        {
            _curState = eSTAGE_STATE.PAGE1CLEAR;
            Debug.Log("Page1 Clear 진입");
        }
    }

    // boss2 죽음 검사
    void Page2Clear()
        {
            if (!_isBoss2Dead)
                return;
            else
            {
                _curState = eSTAGE_STATE.STAGECLEAR;
                Debug.Log("StageClear 진입");
            }
        }

    // 스테이지 클리어 ui open
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
