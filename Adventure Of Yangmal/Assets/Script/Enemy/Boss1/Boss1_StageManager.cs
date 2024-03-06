using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_StageManager : BossStageManager
{
    // 스테이지 관리 클래스
    // Boss1_1, Boss1_2 모두 죽음, Spawner 모두 죽음시 --> 클리어

    public enum eSTAGE_STATE
    {
        NONE,
        STAGECLEAR
    }

    public eSTAGE_STATE _curState = eSTAGE_STATE.NONE;


    // 보스 1,2 참조
    public GameObject _boss1_1;
    public GameObject _boss1_2;
    public SpriteRenderer _boss1_1Sp;
    public SpriteRenderer _boss1_2Sp;

    // 보스 죽음 효과를 위함
    public float _deadTime;
    public float _checkDeadTime;

    // 생명 관련 변수
    public bool _isDead1 = false; // boss1_1 죽음
    public bool _isDead2 = false; // boss1_2 죽음
    public bool _isDoingDie = false; 
    public bool _isAllDead = false;

    // 스테이지클리어 ui
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
                // 1) 스포너 죽음 검사
                _isAllSpawnerDead = IsAllSpawnerDead();

                // 2) 스포너 모두 죽음 확인시 --> 모든 적의 죽음 검사
                if (_isAllSpawnerDead)
                    _isAllEnemyDead = IsAllEnemyDead();

                // 3) boos1_1, 1_2 죽음 검사 & 죽음 효과 이전에만 실시
                if (!_isAllDead)
                    IsBothDead();

                // 둘다 죽었을때
                else if (_isAllDead)
                {
                    // 모든 적 & 스포너가 죽었다면 stageClear = true
                    // _curState = stageClear 상태로 변경
                    if (_isAllSpawnerDead && _isAllEnemyDead)
                            _curState = eSTAGE_STATE.STAGECLEAR;
                }
                break;


            case eSTAGE_STATE.STAGECLEAR:
                // 죽음효과 코루틴 한 번만 실행을 위함
                if (!_isDoingDie)
                {
                    _checkDeadTime += Time.deltaTime;

                    // DeadTime 되었을 때 죽음 효과 실행
                    if (_checkDeadTime >= _deadTime)
                    {
                        _isDoingDie = true;

                        // 움직임 정지 
                        _boss1_1.GetComponent<Boss1_1Move>()._moveSpeed = 0;
                        _boss1_2.GetComponent<Boss1_2>()._followRate = 0;

                        // 죽음 코루틴
                        StartCoroutine(Die(_boss1_1Sp));
                        StartCoroutine(Die(_boss1_2Sp));
                    }// if(_checkDeadTime >= _deadTime)

                    // 스테이지 클리어 ui open
                    StageClear();
                }//if(!_isDoingDie)

                break;
        }//switch (_curState)
    }




    // boss1_1, 1_2 죽음검사
    void IsBothDead()
    {
        if (_isDead1 && _isDead2)
            _isAllDead = true;
    }

    // 죽음 효과 코루틴
    protected IEnumerator Die(SpriteRenderer sprite)
    {
        // 죽음 코루틴 한 번 실행을 위함
        _isAllDead = true;
        _isDoingDie = true;
        float _a = 1f;

        // 페이드 아웃 
        while (_a >= 0)
        {
            _a -= 0.05f;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // 코루틴 종료 후 오브젝트 삭제
        Destroy(sprite.gameObject);
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

