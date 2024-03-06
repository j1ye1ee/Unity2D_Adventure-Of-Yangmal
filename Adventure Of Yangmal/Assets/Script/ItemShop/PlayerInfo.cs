using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public PlayerCoinManager _playerCoin;
    public Text _coinText;


    private void Start()
    {
        _playerCoin = GameObject.FindWithTag("Player").GetComponent<PlayerCoinManager>();
        SetPlayerInfoText();
    }


    // ������ �޼��� ����
    public void SetPlayerInfoText()
    {

        string coinMessage = "������ : " + _playerCoin._playerCoin.ToString() + "G";
        _coinText.text = coinMessage;
    }



}
