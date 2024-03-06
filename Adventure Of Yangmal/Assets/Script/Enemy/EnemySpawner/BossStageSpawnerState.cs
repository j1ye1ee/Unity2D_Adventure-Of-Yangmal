using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossStageSpawnerState : EnemySpawnerState
{
    //Boss 스테이지에 들어가는 enemySpawner 상태클래스

   public enum eBOSS_SPAWNER
    {
        NONE,
        DIE,
        ALL_ENEMYDEAD
    }

    public eBOSS_SPAWNER _curState = eBOSS_SPAWNER.NONE;

    BossStageManager _stageManager;
    bool _isDead = false;
    bool _isAllEnemyDead = false;
    bool _isCountEnd = false;
    public List<bool> _isEnemyDead = new List<bool>();

    private void Start()
    {
        _spawner = GetComponent<EnemySpawner>();
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<BossStageManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // spawner 의 최대 적 생성 갯수만큼 리스트에 false 저장
        for(int count = 0; count< _spawner._countMax; count++)
        {
            _isEnemyDead.Add(false);
        }
        
    }

    private void Update()
    {
        switch(_curState)
        {
            case eBOSS_SPAWNER.NONE:
                None();
                break;
            case eBOSS_SPAWNER.DIE:
                    IsAllEnemyDead();
                break;
            case eBOSS_SPAWNER.ALL_ENEMYDEAD:
                if(!_isCountEnd)
                {
                    _isCountEnd = true;
                    _stageManager._deadEnemyCount++;
                }    
                break;

        }


    }

    // 스포너 죽음 검사
    void None()
    {
        if(_hp<=0)
            if(!_isDead)
            {
                _isDead = true;
                _stageManager._deadSpawnerCount++;
                Die();
                _curState = eBOSS_SPAWNER.DIE;
            }
    }


    // 스포너 죽음 상태 -- > 스포너에서 생성한 적 죽음 검사
    void IsAllEnemyDead()
    {
        if (_isAllEnemyDead)
            _curState = eBOSS_SPAWNER.ALL_ENEMYDEAD;

        else
        {
            EnemySpawner spawner;
            spawner = GetComponent<EnemySpawner>();


            // spawner에서 생성한 enemy가 죽었을 경우 isAllDead에 true 값 넣기
    
            for (int count = 0; count < spawner._enemyList.Count; count++)
            {
                if (spawner._enemyList[count].gameObject.activeSelf == false)
                {
                    // 해당 리스트 자리가 false라면 true로 바꾸기
                    if (_isEnemyDead[count] != true)
                        _isEnemyDead[count] = true;
                }
            }

            // EnemyDead 리스트의 true 개수가 EnemyList.count와 같다면 _isAllEnemyDead = true 
            int deadCount = _isEnemyDead.Where(dead => dead.Equals(true)).Count();

            if(deadCount == _spawner._enemyList.Count)
                _isAllEnemyDead = true;
                

        }
    }



}
