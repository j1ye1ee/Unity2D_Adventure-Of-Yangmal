using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDexUp : ShopItem, IPlayerItem 
{
    // 플레이어의 방어력을 높여주는 아이템



    float _dexUp = 0.1f;

    void Start()
    {
        _shopItemNum = 2;
        _maxValue = 0.5f;
        _effectText = "방어력 증가!";
    }


    public void UseItem()
    {
        Debug.Log(_dexUp + "만큼 방어력 상승");

        PlayerStatus playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

        // 방어력 up 아이템 사용시 플레이어의 최대 방어력을 넘게 된다면 maxDex로 설정
        if (playerStatus._def + _dexUp >= _maxValue)
            PlayerStatus.Instance.PlayerDexSet(_maxValue);

        // 아닌 경우라면 아이템 정상 적용
        else
            PlayerStatus.Instance.PlayerDexUp(_dexUp);

        // ui 적용
        UiManager.Instance.SetPlayerDex();
    }

    public override void SetIsMaxValue()
    {
        PlayerStatus playerStatus = PlayerStatus.Instance;

        if (playerStatus._def >= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }
}
