using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaxHpDown : Item,  IPlayerItem
{
    // �÷��̾��� �ִ� ü���� �����ϴ� ������ Ŭ����

    float _maxHeartDown = 20f;
    float _min = 20;

    private void Start()
    {
        _effectText = "ü�� ����!";
    }


    public void UseItem()
    {

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // �÷��̾��� �ִ� ü�� 20���� �۰ԵǸ� ��ȿ
        if (playerStatus._maxHp - _maxHeartDown < _min)
            return;

        // �׷��� �ʴٸ� ����
        else
        {
            playerStatus.PlayerGetMaxHp(-1 * _maxHeartDown);
            playerStatus.PlayerGetHP(-1 * _maxHeartDown);
        }
    }
}
