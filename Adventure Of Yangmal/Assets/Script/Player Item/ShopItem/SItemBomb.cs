using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SItemBomb : ShopItem, IPlayerItem
{
    void Start()
    {
        _shopItemNum = 6;
        _effectText = "SPECIAL!";
    }

    public void UseItem()
    {

        GameObject player = GameObject.FindWithTag("Player");

        // 아이템매니저의 _isBomb = true
        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();
        playerItemManager._isBomb= true;

    }

    public override void SetIsMaxValue()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();
        if (playerItemManager._isBomb)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }
}
