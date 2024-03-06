using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SItemButterfly : ShopItem, IPlayerItem
{

    public GameObject _butterflyPrefab;

    void Start()
    {
        _shopItemNum = 7;
        _effectText = "SPECIAL!";
    }

    public void UseItem()
    {
        Debug.Log("���� ���� ������ ����");

        GameObject player = GameObject.FindWithTag("Player");

        // �����۸Ŵ����� _isGuide = true

        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();

        if (!playerItemManager._isButterfly)
        {
            playerItemManager._isButterfly = true;
            GameObject butterfly = Instantiate
                (_butterflyPrefab,
                playerItemManager.gameObject.transform.position,
                playerItemManager.gameObject.transform.rotation);
        }

        else
            return;

    }

    public override void SetIsMaxValue()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerItemManager playerItemManager = player.GetComponent<PlayerItemManager>();
        if (playerItemManager._isButterfly)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }
}
