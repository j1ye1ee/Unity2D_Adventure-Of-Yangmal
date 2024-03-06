using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2BulletPoolManager : PoolManager
{
    public GameObject _boss2Bullet;
    public List<GameObject> _boss2BulletPool = new List<GameObject>();

    public GameObject _boss2Bullet4;
    public List<GameObject> _boss2Bullet4Pool = new List<GameObject>();


    private void Awake()
    {
        InstantiatePool(_boss2BulletPool, _boss2Bullet, 20);
        InstantiatePool(_boss2Bullet4Pool, _boss2Bullet4, 20);
    }
}
