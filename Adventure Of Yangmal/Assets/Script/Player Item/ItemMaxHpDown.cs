using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaxHpDown : Item,  IPlayerItem
{
    // 플레이어의 최대 체력을 차감하는 아이템 클래스

    float _maxHeartDown = 20f;
    float _min = 20;

    private void Start()
    {
        _effectText = "체력 차감!";
    }


    public void UseItem()
    {

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // 플레이어의 최대 체력 20보다 작게되면 무효
        if (playerStatus._maxHp - _maxHeartDown < _min)
            return;

        // 그렇지 않다면 차감
        else
        {
            playerStatus.PlayerGetMaxHp(-1 * _maxHeartDown);
            playerStatus.PlayerGetHP(-1 * _maxHeartDown);
        }
    }
}
