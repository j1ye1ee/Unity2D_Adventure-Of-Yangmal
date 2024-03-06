using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLink : MonoBehaviour
{
    public GameObject _spawner;
    public bool _isMinus = false;
    Enemy _enemy;


    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }


    private void Update()
    {
        // 카운트 차감 하지 않았을 경우
        // 적 상태가 죽음 일 경우 --> spawner 에서 카운트 차감
        if (_enemy._curState == Enemy.eENEMY_STATE.DIE)
        {
            // 중복 차감을 막기 위함
            if (!_isMinus && _spawner !=null)
            {
                Debug.Log(gameObject.name + "spawnerLink couint--");
                _spawner.GetComponent<EnemySpawner>()._count--;
                _isMinus = true;
            }
        }
    }
}
