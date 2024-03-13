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


        // �� pool ����
        if (!GameManager.Instance._isGameOver && !GameManager.Instance._isReplay)
            InstaniatePlayerBulletPool();
    }

    // ������ ����
    public void ResetPrefab()
    {
        PlayerBullet playerBullet = _playerBulletPrefab.GetComponent<PlayerBullet>();

        // ������ �ʱ�ȭ _�Ѿ�
        _playerBulletPrefab.transform.localScale = new Vector2(0.7f, 0.7f);
        _playerBulletPrefab.GetComponent<PlayerBullet>()._damage = 10;

        // ������ �ʱ�ȭ _��ź
        _bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage = 20;

        // ������ �ʱ�ȭ _�̻���
        _guideMissilePrefab.GetComponent<GuideMissile>()._damage = 15;
    }


    //pool ���� ��� ���� & ����Ʈ ����
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


    // ���� ����۽� Ǯ �ʱ�ȭ
    public void ResetAllBullet()
    {
        // ������ ����
        ResetPrefab();
        // Ǯ����Ʈ ����
        ResetPoolList();
    }

    // Ǯ ����
    public void InstaniatePlayerBulletPool()
    {
        InstantiatePool(_playerBulletPool, _playerBulletPrefab, 10);
        InstantiatePool(_bombPool, _bombPrefab, 5);
        InstantiatePool(_guideMissilePool, _guideMissilePrefab, 5);
    }

    // ������ ���� Ǯ �缳��
    public void ItemUsePoolSet()
    {
        // Ǯ����Ʈ ����
        ResetPoolList();

        // �����
        InstaniatePlayerBulletPool();
    }


}
