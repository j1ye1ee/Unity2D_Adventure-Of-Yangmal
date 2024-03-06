using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaxHpUp : ShopItem, IPlayerItem
{
    // �÷��̾��� �ִ�ü���� ���̴� ������ Ŭ����


    float _maxHeartUp = 20f;


    void Start()
    {
        _shopItemNum = 0;
        _maxValue = 280f;
        _effectText = "�ִ�ü�� ����!";
    }


    public void UseItem()
    {
        PlayerStatus playerStatus = PlayerStatus.Instance;

        if (playerStatus._maxHp + _maxHeartUp > _maxValue)
            playerStatus.PlayerMaxHpSet(_maxValue);

        else
        {
            // �÷��̾��� �ִ� ü�� ����
            playerStatus.PlayerGetMaxHp(_maxHeartUp);
            // �÷��̾��� ü�� = �ִ� ü������ ȸ��
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
