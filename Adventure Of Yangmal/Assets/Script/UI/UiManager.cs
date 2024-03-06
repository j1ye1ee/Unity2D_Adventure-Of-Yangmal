using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{ 
   [SerializeField]
   GameObject _player;
   [SerializeField]
    float _originSpeed;

    [SerializeField]
    Text _stageName;
    [SerializeField]
    Text _playerDamage;
    [SerializeField]
    Text _playerDex;
    [SerializeField]
    Text _playerGold;


    //  �÷��̾� �ӵ� 0 �� ������ ����
    public void StopPlayer(GameObject player)
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        _originSpeed = playerMove._moveSpeed;
        playerMove._moveSpeed = 0;

        player.GetComponent<Animator>().enabled = false;
    }

    //  �÷��̾� ���󺹱�
    public void ReturnPlayer(GameObject player)
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        playerMove._moveSpeed = _originSpeed;

        player.GetComponent<Animator>().enabled = true;
    }

    // ���� ui �� stage �̸� ����
    public void SetStageText(string stage)
    {
        _stageName.text = stage;
    }

    // ���� ui�� �÷��̾� ���ݷ� ����
    public void SetPlayerDamage()
    {
        float damage = PlayerStatus.Instance._damage;
        _playerDamage.text = "DAMAGE "+damage.ToString();
    }

    // ���� ui�� �÷��̾� ���� ����
    public void SetPlayerDex()
    {
        float dex = PlayerStatus.Instance._dex;
        _playerDex.text = "DEX "+dex.ToString();
    }

    // ���� ui�� �÷��̾� ��� ����
    public void SetPlayerGold()
    {
        int gold = PlayerCoinManager.Instance._playerCoin;
        _playerGold.text = gold.ToString() + "G";
    }

    //originSpeed ����
    public void SetOriginSpeed(float speed)
    {
        _originSpeed = speed;
    }

    // originSpeed retirn
    public float GetOriginSpeed()
    {
        return _originSpeed;
    }
}
