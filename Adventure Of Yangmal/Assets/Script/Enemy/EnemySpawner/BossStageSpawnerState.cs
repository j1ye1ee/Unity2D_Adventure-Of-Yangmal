using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossStageSpawnerState : EnemySpawnerState
{
    //Boss ���������� ���� enemySpawner ����Ŭ����

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
        
        // spawner �� �ִ� �� ���� ������ŭ ����Ʈ�� false ����
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

    // ������ ���� �˻�
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


    // ������ ���� ���� -- > �����ʿ��� ������ �� ���� �˻�
    void IsAllEnemyDead()
    {
        if (_isAllEnemyDead)
            _curState = eBOSS_SPAWNER.ALL_ENEMYDEAD;

        else
        {
            EnemySpawner spawner;
            spawner = GetComponent<EnemySpawner>();


            // spawner���� ������ enemy�� �׾��� ��� isAllDead�� true �� �ֱ�
    
            for (int count = 0; count < spawner._enemyList.Count; count++)
            {
                if (spawner._enemyList[count].gameObject.activeSelf == false)
                {
                    // �ش� ����Ʈ �ڸ��� false��� true�� �ٲٱ�
                    if (_isEnemyDead[count] != true)
                        _isEnemyDead[count] = true;
                }
            }

            // EnemyDead ����Ʈ�� true ������ EnemyList.count�� ���ٸ� _isAllEnemyDead = true 
            int deadCount = _isEnemyDead.Where(dead => dead.Equals(true)).Count();

            if(deadCount == _spawner._enemyList.Count)
                _isAllEnemyDead = true;
                

        }
    }



}
