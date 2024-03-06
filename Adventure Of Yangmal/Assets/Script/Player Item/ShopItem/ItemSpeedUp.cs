using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeedUp : ShopItem, IPlayerItem
{
    // �÷��̾��� �̵� ���ǵ带 ���� �ִ� ������


    float _speedUp = 2f;


    void Start()
    {
        _shopItemNum = 1;
        _maxValue = 25f;
        _effectText = "�ӵ� ����!";
    }



    public void UseItem()
    {
        Debug.Log(_speedUp + "��ŭ �̵��ӵ� ���");

        PlayerMove playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        // �̵� ���ǵ� ���

        if (playerMove._moveSpeed + _speedUp >= _maxValue)
            playerMove._moveSpeed = _maxValue;

        else
            playerMove._moveSpeed += _speedUp;

    }


    // NPC ���� �������� ������ ���
    public void BuySpeedItem()
    {
        float tempSpeed;
        Debug.Log(_speedUp + "��ŭ �̵��ӵ� ���");

        PlayerMove playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        tempSpeed = UiManager.Instance.GetOriginSpeed();

        // �̵� ���ǵ� ���

        if (tempSpeed + _speedUp >= _maxValue)
            tempSpeed = _maxValue;

        else
            tempSpeed += _speedUp;

        UiManager.Instance.SetOriginSpeed(tempSpeed);
    }

    public override void SetIsMaxValue()
    {
        float playerSpeed = UiManager.Instance.GetOriginSpeed();

        if (playerSpeed >= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }
}
