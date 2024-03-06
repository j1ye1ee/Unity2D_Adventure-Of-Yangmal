using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_2EffectPoolManager : PoolManager
{
    public GameObject _boss1_2Eft;
    public List<GameObject> _boss1_2EftPool = new List<GameObject>();

    public GameObject _shadow;
    public List<GameObject> _shadowPool = new List<GameObject>();

    public GameObject _parabolaSpot;
    public List<GameObject> _parabolaSpotPool = new List<GameObject>();

    private void Awake()
    {
        InstantiatePool(_boss1_2EftPool, _boss1_2Eft, 20);
        InstantiatePool(_shadowPool, _shadow, 5);
        InstantiatePool(_parabolaSpotPool, _parabolaSpot, 5);
    }

}
