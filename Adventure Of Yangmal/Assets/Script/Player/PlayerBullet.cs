using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : PlayerBulletBase
{
    //  �÷��̾� �⺻ �Ѿ� Ŭ����


    private void Start()
    {

        // �÷��̾��� ���ݷ�(������) ĳ��
        _damage = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>()._damage;
        _playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
    }

}
