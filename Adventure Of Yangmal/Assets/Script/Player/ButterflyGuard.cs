using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGuard : PlayerBulletBase
{
    // ���� �������� ��� Ŭ���� = �������� �÷��̾��� ���ݷ� / 2 
    // ���� ��Ƶ� �ٸ� �Ѿ�ó�� �����Ǹ� �ȵȴ� --> _isSItem = true


    private void Start()
    {
        _playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
        _isSItem = true;
        _damage = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>()._damage / 2f;
    }


}
