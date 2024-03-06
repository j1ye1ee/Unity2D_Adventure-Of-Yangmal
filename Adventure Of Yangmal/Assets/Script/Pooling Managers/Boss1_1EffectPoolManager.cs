using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1EffectPoolManager : PoolManager
{
    public GameObject _boss1_1Eft;
    public List<GameObject> _boss1_1EftPool = new List<GameObject>();

    private void Awake()
    {
        InstantiatePool(_boss1_1EftPool, _boss1_1Eft, 20);
    }
}
