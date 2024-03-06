using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1BulletPoolManager : PoolManager
{
    public GameObject _enemy1BulletPrefab;
    public List<GameObject> _enemy1BulletPool = new List<GameObject>();

    void Awake()
    {
        InstantiatePool(_enemy1BulletPool, _enemy1BulletPrefab, 5);
    }
}
