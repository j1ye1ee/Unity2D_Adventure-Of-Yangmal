using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPoolManager : PoolManager
{
    public GameObject _e1;
    public GameObject _e2;
    public GameObject _e3;
    public GameObject _e4;
    public GameObject _e5;
    public GameObject _e6;
    public GameObject _e7;


    public List<GameObject> _e1Pool = new List<GameObject>();
    public List<GameObject> _e2Pool = new List<GameObject>();
    public List<GameObject> _e3Pool = new List<GameObject>();
    public List<GameObject> _e4Pool = new List<GameObject>();
    public List<GameObject> _e5Pool = new List<GameObject>();
    public List<GameObject> _e6Pool = new List<GameObject>();
    public List<GameObject> _e7Pool = new List<GameObject>();



    private void Awake()
    {
        InstantiatePool(_e1Pool, _e1, 3);
        InstantiatePool(_e2Pool, _e2, 3);
        InstantiatePool(_e3Pool, _e3, 3);
        InstantiatePool(_e4Pool, _e4, 3);
        InstantiatePool(_e5Pool, _e5, 3);
        InstantiatePool(_e6Pool, _e6, 3);
        InstantiatePool(_e7Pool, _e7, 3);

    }


}
