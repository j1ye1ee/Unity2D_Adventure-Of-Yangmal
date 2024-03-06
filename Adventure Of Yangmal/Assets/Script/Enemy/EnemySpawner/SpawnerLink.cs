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
        // ī��Ʈ ���� ���� �ʾ��� ���
        // �� ���°� ���� �� ��� --> spawner ���� ī��Ʈ ����
        if (_enemy._curState == Enemy.eENEMY_STATE.DIE)
        {
            // �ߺ� ������ ���� ����
            if (!_isMinus && _spawner !=null)
            {
                Debug.Log(gameObject.name + "spawnerLink couint--");
                _spawner.GetComponent<EnemySpawner>()._count--;
                _isMinus = true;
            }
        }
    }
}
