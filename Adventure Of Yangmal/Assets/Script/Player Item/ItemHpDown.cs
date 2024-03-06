using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpDown : Item, IPlayerItem
{
    float _heartDown = 5f;


    // 플레이어의 체력을 깎는 아이템 클래스
    private void Start()
    {
        _effectText = "체력 차감!";
    }

    public void UseItem()
    {
        Debug.Log(_heartDown + "만큼 체력 차감");

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // 하트다운 아이템 사용시 플레이어의 체력이 0 이하가 된다면 체력 0으로 처리
        if (playerStatus._hp - _heartDown <= 0)
            playerStatus.PlayerHpSet(0);

        // 아닌 경우라면 아이템 정상 적용
        else
            playerStatus.PlayerGetHP(-1 * _heartDown);

    }
}
