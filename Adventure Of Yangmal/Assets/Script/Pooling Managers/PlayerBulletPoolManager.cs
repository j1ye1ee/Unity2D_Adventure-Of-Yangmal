using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPoolManager : PoolManager
{
    private static PlayerBulletPoolManager instance = null;
    public static PlayerBulletPoolManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }

            return instance;
        }
    }

    public GameObject _playerBulletPrefab;
    public List<GameObject> _playerBulletPool = new List<GameObject>();

    public GameObject _bombPrefab;
    public List<GameObject> _bombPool = new List<GameObject>();

    public GameObject _guideMissilePrefab;
    public List<GameObject> _guideMissilePool = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }


        // 각 pool 생성
        InstantiatePool(_playerBulletPool, _playerBulletPrefab, 10);
        InstantiatePool(_bombPool, _bombPrefab, 5);
        InstantiatePool(_guideMissilePool, _guideMissilePrefab, 5);
    }

    // 프리팹 리셋
    public void ResetPrefab()
    {
        PlayerBullet playerBullet = _playerBulletPrefab.GetComponent<PlayerBullet>();

        // 프리팹 초기화 _총알
        _playerBulletPrefab.gameObject.transform.localScale
            = new Vector2(0.7f, 0.7f);
        _playerBulletPrefab.GetComponent<PlayerBullet>()._damage = 10;

        // 프리팹 초기화 _폭탄
        _bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage = 20;

        // 프리팹 초기화 _미사일
        _guideMissilePrefab.GetComponent<GuideMissile>()._damage = 15;
    }




    // 프리팹 리셋 및 게임 재시작시 풀 초기화
    public void ResetAllBullet()
    {
        ResetPrefab();

        // 총알 pool 모두 삭제
        for (int i = 0; i < _playerBulletPool.Count; i++)
        {
            Destroy(_playerBulletPool[i].gameObject);
        }


        // 폭탄 pool 모두 삭제
        for (int i = 0; i < _bombPool.Count; i++)
        {
            Destroy(_bombPool[i].gameObject);
        }


        // 미사일 pool 모두 삭제
        for (int i = 0; i < _guideMissilePool.Count; i++)
        {
            Destroy(_guideMissilePool[i].gameObject);
        }

        // 재생성

        ResetPool(_playerBulletPool, _playerBulletPrefab, 10);
        ResetPool(_bombPool, _bombPrefab, 5);
        ResetPool(_guideMissilePool, _guideMissilePrefab, 5);

    }
}
