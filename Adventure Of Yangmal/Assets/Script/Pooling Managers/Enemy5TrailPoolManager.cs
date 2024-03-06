using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5TrailPoolManager : PoolManager
{
    public GameObject _t1;
    public GameObject _t2;
    public GameObject _t3;
    public GameObject _t4;
    public GameObject _t5;

    public List<GameObject> _t1Pool = new List<GameObject>();
    public List<GameObject> _t2Pool = new List<GameObject>();
    public List<GameObject> _t3Pool = new List<GameObject>();
    public List<GameObject> _t4Pool = new List<GameObject>();
    public List<GameObject> _t5Pool = new List<GameObject>();


    private void Awake()
    {
        InstantiatePool(_t1Pool, _t1, 5);
        InstantiatePool(_t2Pool, _t2, 5);
        InstantiatePool(_t3Pool, _t3, 5);
        InstantiatePool(_t4Pool, _t4, 5);
        InstantiatePool(_t5Pool, _t5, 5);
    }
}
