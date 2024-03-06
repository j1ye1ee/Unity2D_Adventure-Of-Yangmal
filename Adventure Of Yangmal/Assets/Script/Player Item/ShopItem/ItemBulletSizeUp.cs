using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBulletSizeUp : ShopItem, IPlayerItem
{
    // 플레이어의 bullet 사이즈업 아이템

    public GameObject _itemUseBullet;
    float _upScale = 1.2f;
    PlayerBulletPoolManager _poolManager;

    void Start()
    {
        _poolManager = GameObject.FindWithTag("playerBulletPool").GetComponent<PlayerBulletPoolManager>();
        _shopItemNum = 4;
        _maxValue = 2f;
        _effectText = "헤어볼 크기 증가!";
    }


    public void UseItem()
    {
        Debug.Log(_upScale + "만큼 player  bullet 사이즈업");

        // PlayerbulletPool의 bullet 복사
        _itemUseBullet = _poolManager._playerBulletPrefab;

        // 현재 플레이어 bullet 의 크기에 _upScale만큼 곱해서 준비
        float changeScale = _itemUseBullet.transform.localScale.x * _upScale;

        // bullet 크기가 max를 넘지 않는다면 효과 적용
        if (changeScale >= _maxValue)
        {
            changeScale = _maxValue;
            _itemUseBullet.transform.localScale = new Vector2(changeScale, changeScale);
        }


        // 아닌 경우라면 아이템 정상 적용
        else
            _itemUseBullet.transform.localScale = new Vector2(changeScale, changeScale);

        // poolManager 의 playerbullet Prefab 바꾸기 완료
        _poolManager._playerBulletPrefab = _itemUseBullet;

        // poolManager  내부의 playerbullet 오브젝트 전부 교체
        for(int i=0; i<_poolManager._playerBulletPool.Count; i++)
        {
            _poolManager._playerBulletPool[i] = _itemUseBullet;
        }

    }

    public override void SetIsMaxValue()
    {
        PlayerBulletPoolManager poolManager =
            GameObject.FindWithTag("playerBulletPool").GetComponent<PlayerBulletPoolManager>();

        // PlayerbulletPool의 bullet 복사
        _itemUseBullet = poolManager._playerBulletPrefab;

        float scale = _itemUseBullet.transform.localScale.x;

        if (scale >= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }

}
