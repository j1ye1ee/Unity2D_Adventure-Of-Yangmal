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
    Text _playerDef;
    [SerializeField]
    Text _playerGold;


    //  �÷��̾� �ӵ� 0 �� ������ ����
    public void StopPlayer(GameObject player)
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        _originSpeed = playerMove._moveSpeed;
        Debug.Log("�÷��̾� ���� �ӵ�" + _originSpeed);
        playerMove._moveSpeed = 0;

        player.GetComponent<Animator>().enabled = false;
    }

    //  �÷��̾� ���󺹱�
    public void ReturnPlayer(GameObject player)
    {
        PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        playerMove._moveSpeed = _originSpeed;

        Debug.Log("�÷��̾� �ӵ� ����");
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
        float def = PlayerStatus.Instance._def;
        _playerDef.text = "DEF "+def.ToString();
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
