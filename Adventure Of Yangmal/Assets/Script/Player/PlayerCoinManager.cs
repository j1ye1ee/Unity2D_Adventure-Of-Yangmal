using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoinManager : MonoBehaviour
{
    // 플레이어 코인 관리 클래스
    static PlayerCoinManager _instance = null;
    public static PlayerCoinManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerCoinManager>();

            return _instance;
        }
    }

    public int _playerCoin { get; private set; }


    private void Start()
    {
        _playerCoin = 0;
    }

    // 코인 얻을때
    public void GetCoin(int coin)
    {
        _playerCoin += coin;
        UiManager.Instance.SetPlayerGold();
    }

    // 코인 잃을때
    public void ConsumeCoin(int coin)
    {
        _playerCoin -= coin;
        UiManager.Instance.SetPlayerGold();
    }

    public void ResetCoin()
    {
        _playerCoin = 0;
    }
}
