using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnerNum : MonoBehaviour
{
    // stageManager¿« SpawnerNum ºº∆√

    BossStageManager _stageManager;

    void Start()
    {
        _stageManager 
            = GameObject.FindWithTag("Stage Manager").GetComponent<BossStageManager>();

        _stageManager._spawnerNum = transform.childCount;
    }

}
