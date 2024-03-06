using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6BabyPoolManager : PoolManager
{

    public GameObject _baby;
    public List<GameObject> _babyPool = new List<GameObject>();

    void Awake()
    {
        InstantiatePool(_babyPool, _baby, 5);
    }

}
