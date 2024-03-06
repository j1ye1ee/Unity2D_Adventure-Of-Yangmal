using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpUp : Item, IPlayerItem
{
    // �÷��̾��� ü���� ���̴� ������ Ŭ����

    float _heartUp = 10f;


    private void Start()
    {
        _effectText = "ü�� ȸ��!";
    }

    public void UseItem()
    {
        Debug.Log(_heartUp + "��ŭ ü�� ȸ��");

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // ��Ʈ�� ������ ���� �÷��̾��� �ִ� ü���� �Ѿ�ٸ� hp �� maxhp �� ����
        if (playerStatus._hp + _heartUp >= playerStatus._maxHp)
            playerStatus.PlayerHpSet(playerStatus._maxHp);


        // �ƴ� ����� ������ ���� ����
        else
            playerStatus.PlayerGetHP(_heartUp);

    }
}
