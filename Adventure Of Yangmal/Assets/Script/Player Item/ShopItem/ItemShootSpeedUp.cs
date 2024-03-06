using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShootSpeedUp : ShopItem, IPlayerItem
{
    // 플레이어의 공격속도를 높여주는 아이템


    float _speedUp = 0.05f;
    float _forceUp = 2.5f;
    float _maxForce = 30f;

    void Start()
    {
        _shopItemNum = 3;
        _maxValue = 0.2f;
        _effectText = "공속 증가!";
    }


    public void UseItem()
    {
        Debug.Log(_speedUp + "만큼 공격속도 증가" + _forceUp + "만큼 foce 증가");

        PlayerShooter playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();

        // 아이템 적용시 공속이 최대 공속을 넘어서게 된다면 공속 = 최대공속으로 설정
        if (playerShooter._shootTerm - _speedUp <= _maxValue)
        {
            playerShooter._shootTerm = _maxValue;
            playerShooter._bulletSpeed = _maxForce;
        }


        // 아닐시 정상 적용
        else
        {
            playerShooter._shootTerm -= _speedUp;
            playerShooter._bulletSpeed += _forceUp;
        }


    }

    public override void SetIsMaxValue()
    {
        PlayerShooter playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
        if (playerShooter._shootTerm <= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }

}
