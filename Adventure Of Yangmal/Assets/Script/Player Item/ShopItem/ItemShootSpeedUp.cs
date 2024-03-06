using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShootSpeedUp : ShopItem, IPlayerItem
{
    // �÷��̾��� ���ݼӵ��� �����ִ� ������


    float _speedUp = 0.05f;
    float _forceUp = 2.5f;
    float _maxForce = 30f;

    void Start()
    {
        _shopItemNum = 3;
        _maxValue = 0.2f;
        _effectText = "���� ����!";
    }


    public void UseItem()
    {
        Debug.Log(_speedUp + "��ŭ ���ݼӵ� ����" + _forceUp + "��ŭ foce ����");

        PlayerShooter playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();

        // ������ ����� ������ �ִ� ������ �Ѿ�� �ȴٸ� ���� = �ִ�������� ����
        if (playerShooter._shootTerm - _speedUp <= _maxValue)
        {
            playerShooter._shootTerm = _maxValue;
            playerShooter._bulletSpeed = _maxForce;
        }


        // �ƴҽ� ���� ����
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
