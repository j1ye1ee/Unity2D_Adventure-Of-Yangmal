using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeedUp : ShopItem, IPlayerItem
{
    // 플레이어의 이동 스피드를 높여 주는 아이템


    float _speedUp = 2f;


    void Start()
    {
        _shopItemNum = 1;
        _maxValue = 25f;
        _effectText = "속도 증가!";
    }



    public void UseItem()
    {
        Debug.Log(_speedUp + "만큼 이동속도 상승");

        PlayerMove playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        // 이동 스피드 상승

        if (playerMove._moveSpeed + _speedUp >= _maxValue)
            playerMove._moveSpeed = _maxValue;

        else
            playerMove._moveSpeed += _speedUp;

    }


    // NPC 에게 아이템을 구매할 경우
    public void BuySpeedItem()
    {
        float tempSpeed;
        Debug.Log(_speedUp + "만큼 이동속도 상승");

        PlayerMove playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        tempSpeed = UiManager.Instance.GetOriginSpeed();

        // 이동 스피드 상승

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
