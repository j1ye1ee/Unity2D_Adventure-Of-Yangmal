using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpDown : Item, IPlayerItem
{
    float _heartDown = 5f;


    // �÷��̾��� ü���� ��� ������ Ŭ����
    private void Start()
    {
        _effectText = "ü�� ����!";
    }

    public void UseItem()
    {
        Debug.Log(_heartDown + "��ŭ ü�� ����");

        PlayerStatus playerStatus = PlayerStatus.Instance;

        // ��Ʈ�ٿ� ������ ���� �÷��̾��� ü���� 0 ���ϰ� �ȴٸ� ü�� 0���� ó��
        if (playerStatus._hp - _heartDown <= 0)
            playerStatus.PlayerHpSet(0);

        // �ƴ� ����� ������ ���� ����
        else
            playerStatus.PlayerGetHP(-1 * _heartDown);

    }
}
