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


    //  플레이어 속도 0 및 움직임 정지
    public void StopPlayer(GameObject player)
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        _originSpeed = playerMove._moveSpeed;
        playerMove._moveSpeed = 0;

        player.GetComponent<Animator>().enabled = false;
    }

    //  플레이어 원상복구
    public void ReturnPlayer(GameObject player)
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        playerMove._moveSpeed = _originSpeed;

        player.GetComponent<Animator>().enabled = true;
    }

    // 메인 ui 의 stage 이름 변경
    public void SetStageText(string stage)
    {
        _stageName.text = stage;
    }

    // 메인 ui의 플레이어 공격력 변경
    public void SetPlayerDamage()
    {
        float damage = PlayerStatus.Instance._damage;
        _playerDamage.text = "DAMAGE "+damage.ToString();
    }

    // 메인 ui의 플레이어 방어력 변경
    public void SetPlayerDex()
    {
        float dex = PlayerStatus.Instance._dex;
        _playerDex.text = "DEX "+dex.ToString();
    }

    // 메인 ui의 플레이어 골드 변경
    public void SetPlayerGold()
    {
        int gold = PlayerCoinManager.Instance._playerCoin;
        _playerGold.text = gold.ToString() + "G";
    }

    //originSpeed 세팅
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
