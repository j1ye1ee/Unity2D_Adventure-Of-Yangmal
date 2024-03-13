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

            else
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
        if (!GameManager.Instance._isGameOver && !GameManager.Instance._isReplay)
            InstaniatePlayerBulletPool();
    }

    // 프리팹 리셋
    public void ResetPrefab()
    {
        PlayerBullet playerBullet = _playerBulletPrefab.GetComponent<PlayerBullet>();

        // 프리팹 초기화 _총알
        _playerBulletPrefab.transform.localScale = new Vector2(0.7f, 0.7f);
        _playerBulletPrefab.GetComponent<PlayerBullet>()._damage = 10;

        // 프리팹 초기화 _폭탄
        _bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage = 20;

        // 프리팹 초기화 _미사일
        _guideMissilePrefab.GetComponent<GuideMissile>()._damage = 15;
    }


    //pool 내부 모두 삭제 & 리스트 삭제
    public void ResetPoolList()
    {
        for(int i =0; i<gameObject.transform.childCount; i++)
        {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            Destroy(obj);
        }
        _playerBulletPool.Clear();
        _bombPool.Clear();
        _guideMissilePool.Clear();
    }


    // 게임 재시작시 풀 초기화
    public void ResetAllBullet()
    {
        // 프리팹 리셋
        ResetPrefab();
        // 풀리스트 리셋
        ResetPoolList();
    }

    // 풀 생성
    public void InstaniatePlayerBulletPool()
    {
        InstantiatePool(_playerBulletPool, _playerBulletPrefab, 10);
        InstantiatePool(_bombPool, _bombPrefab, 5);
        InstantiatePool(_guideMissilePool, _guideMissilePrefab, 5);
    }

    // 아이템 사용시 풀 재설정
    public void ItemUsePoolSet()
    {
        // 풀리스트 리셋
        ResetPoolList();

        // 재생성
        InstaniatePlayerBulletPool();
    }


}
