using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4BulletPoolManager : PoolManager
{
    public GameObject _enemy4BulletPrefab;
    public List<GameObject> _enemy4BulletPool = new List<GameObject>();

    void Awake()
    {
        InstantiatePool(_enemy4BulletPool, _enemy4BulletPrefab, 5);
    }
}
