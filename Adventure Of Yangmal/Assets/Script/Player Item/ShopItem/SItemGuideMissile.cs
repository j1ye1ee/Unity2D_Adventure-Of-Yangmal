using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SItemGuideMissile : ShopItem, IPlayerItem
{
  
    void Start()
    {
        _shopItemNum = 8;
        _effectText = "SPECIAL!";
        
    }

    public void UseItem()
    {
        Debug.Log("���̵�̻��� ����");

        GameObject player = GameObject.FindWithTag("Player");

        // �����۸Ŵ����� _isGuide = true

        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();
        playerItemManager._isGuideMissile = true;

    }

    public override void SetIsMaxValue()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();
        if (playerItemManager._isGuideMissile)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }

}
