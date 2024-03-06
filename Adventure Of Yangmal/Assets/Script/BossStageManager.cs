using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{

    // spawner
    public int _deadSpawnerCount;
    public int _spawnerNum;
    public bool _isAllSpawnerDead = false;

    // enemy
    public int _deadEnemyCount;
    public bool _isAllEnemyDead = false;

    public bool _isStageClear = false;

    // 모든 spawner 죽음 검사
    public bool IsAllSpawnerDead()
    {
        if (_deadSpawnerCount == _spawnerNum)
            return true;

        else
            return false;
    }

    // 모든 enemy 죽음 검사
    public bool IsAllEnemyDead()
    {
        if (_deadEnemyCount == _spawnerNum)
            return true;
        else
            return false;
    }
}
