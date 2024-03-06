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


        // �� pool ����
        InstantiatePool(_playerBulletPool, _playerBulletPrefab, 10);
        InstantiatePool(_bombPool, _bombPrefab, 5);
        InstantiatePool(_guideMissilePool, _guideMissilePrefab, 5);
    }

    // ������ ����
    public void ResetPrefab()
    {
        PlayerBullet playerBullet = _playerBulletPrefab.GetComponent<PlayerBullet>();

        // ������ �ʱ�ȭ _�Ѿ�
        _playerBulletPrefab.gameObject.transform.localScale
            = new Vector2(0.7f, 0.7f);
        _playerBulletPrefab.GetComponent<PlayerBullet>()._damage = 10;

        // ������ �ʱ�ȭ _��ź
        _bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage = 20;

        // ������ �ʱ�ȭ _�̻���
        _guideMissilePrefab.GetComponent<GuideMissile>()._damage = 15;
    }




    // ������ ���� �� ���� ����۽� Ǯ �ʱ�ȭ
    public void ResetAllBullet()
    {
        ResetPrefab();

        // �Ѿ� pool ��� ����
        for (int i = 0; i < _playerBulletPool.Count; i++)
        {
            Destroy(_playerBulletPool[i].gameObject);
        }


        // ��ź pool ��� ����
        for (int i = 0; i < _bombPool.Count; i++)
        {
            Destroy(_bombPool[i].gameObject);
        }


        // �̻��� pool ��� ����
        for (int i = 0; i < _guideMissilePool.Count; i++)
        {
            Destroy(_guideMissilePool[i].gameObject);
        }

        // �����

        ResetPool(_playerBulletPool, _playerBulletPrefab, 10);
        ResetPool(_bombPool, _bombPrefab, 5);
        ResetPool(_guideMissilePool, _guideMissilePrefab, 5);

    }
}
