using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaxHpUp : ShopItem, IPlayerItem
{
    // 플레이어의 최대체력을 높이는 아이템 클래스


    float _maxHeartUp = 20f;


    void Start()
    {
        _shopItemNum = 0;
        _maxValue = 280f;
        _effectText = "최대체력 증가!";
    }


    public void UseItem()
    {
        PlayerStatus playerStatus = PlayerStatus.Instance;

        if (playerStatus._maxHp + _maxHeartUp > _maxValue)
            playerStatus.PlayerMaxHpSet(_maxValue);

        else
        {
            // 플레이어의 최대 체력 증가
            playerStatus.PlayerGetMaxHp(_maxHeartUp);
            // 플레이어의 체력 = 최대 체력으로 회복
            playerStatus.PlayerHpSet(playerStatus._maxHp);
        }
    }

    public override void SetIsMaxValue()
    {
        PlayerStatus playerStatus = PlayerStatus.Instance;
        if (playerStatus._maxHp >= _maxValue)
        {
            _isMaxValue = true;
        }

        else
            _isMaxValue = false;
    }
}
