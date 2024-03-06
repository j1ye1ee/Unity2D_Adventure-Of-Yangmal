using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBulletSizeUp : ShopItem, IPlayerItem
{
    // �÷��̾��� bullet ������� ������

    public GameObject _itemUseBullet;
    float _upScale = 1.2f;
    PlayerBulletPoolManager _poolManager;

    void Start()
    {
        _poolManager = GameObject.FindWithTag("playerBulletPool").GetComponent<PlayerBulletPoolManager>();
        _shopItemNum = 4;
        _maxValue = 2f;
        _effectText = "�� ũ�� ����!";
    }


    public void UseItem()
    {
        Debug.Log(_upScale + "��ŭ player  bullet �������");

        // PlayerbulletPool�� bullet ����
        _itemUseBullet = _poolManager._playerBulletPrefab;

        // ���� �÷��̾� bullet �� ũ�⿡ _upScale��ŭ ���ؼ� �غ�
        float changeScale = _itemUseBullet.transform.localScale.x * _upScale;

        // bullet ũ�Ⱑ max�� ���� �ʴ´ٸ� ȿ�� ����
        if (changeScale >= _maxValue)
        {
            changeScale = _maxValue;
            _itemUseBullet.transform.localScale = new Vector2(changeScale, changeScale);
        }


        // �ƴ� ����� ������ ���� ����
        else
            _itemUseBullet.transform.localScale = new Vector2(changeScale, changeScale);

        // poolManager �� playerbullet Prefab �ٲٱ� �Ϸ�
        _poolManager._playerBulletPrefab = _itemUseBullet;

        // poolManager  ������ playerbullet ������Ʈ ���� ��ü
        for(int i=0; i<_poolManager._playerBulletPool.Count; i++)
        {
            _poolManager._playerBulletPool[i] = _itemUseBullet;
        }

    }

    public override void SetIsMaxValue()
    {
        PlayerBulletPoolManager poolManager =
            GameObject.FindWithTag("playerBulletPool").GetComponent<PlayerBulletPoolManager>();

        // PlayerbulletPool�� bullet ����
        _itemUseBullet = poolManager._playerBulletPrefab;

        float scale = _itemUseBullet.transform.localScale.x;

        if (scale >= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }

}
