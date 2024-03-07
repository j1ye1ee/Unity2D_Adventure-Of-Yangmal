using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDexUp : ShopItem, IPlayerItem 
{
    // �÷��̾��� ������ �����ִ� ������



    float _dexUp = 0.1f;

    void Start()
    {
        _shopItemNum = 2;
        _maxValue = 0.5f;
        _effectText = "���� ����!";
    }


    public void UseItem()
    {
        Debug.Log(_dexUp + "��ŭ ���� ���");

        PlayerStatus playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

        // ���� up ������ ���� �÷��̾��� �ִ� ������ �Ѱ� �ȴٸ� maxDex�� ����
        if (playerStatus._def + _dexUp >= _maxValue)
            PlayerStatus.Instance.PlayerDexSet(_maxValue);

        // �ƴ� ����� ������ ���� ����
        else
            PlayerStatus.Instance.PlayerDexUp(_dexUp);

        // ui ����
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
