using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpUp : Item, IPlayerItem
{
    // 플레이어의 체력을 높이는 아이템 클래스

    float _heartUp = 10f;


    private void Start()
    {
        _effectText = "체력 회복!";
    }

    public void UseItem()
    {
        Debug.Log(_heartUp + "만큼 체력 회복");

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // 하트업 아이템 사용시 플레이어의 최대 체력을 넘어선다면 hp 를 maxhp 로 설정
        if (playerStatus._hp + _heartUp >= playerStatus._maxHp)
            playerStatus.PlayerHpSet(playerStatus._maxHp);


        // 아닌 경우라면 아이템 정상 적용
        else
            playerStatus.PlayerGetHP(_heartUp);

    }
}
