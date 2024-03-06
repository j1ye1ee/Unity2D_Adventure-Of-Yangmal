using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : Item,IPlayerItem
{
    public int _upCoin;

    void Start()
    {
        _upCoin = SetRandomCoin();
    }

    int SetRandomCoin()
    {
        int random = Random.Range(100, 510);
        double preCoin = random / 100;
        int coin = 10*(int)(System.Math.Truncate(preCoin));
        //������ 10�� ~ 50������ 10�� ������ ������ ����

        return coin;
    }

    public void UseItem()
    {
        _effectText = _upCoin + "G!";
        Debug.Log(_upCoin + "��ŭ ���� ����");

        PlayerCoinManager playerCoinManager = GameObject.FindWithTag("Player").GetComponent<PlayerCoinManager>();

        playerCoinManager.GetCoin(_upCoin);

    }
}
