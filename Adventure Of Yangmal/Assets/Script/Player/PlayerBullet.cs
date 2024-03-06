using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : PlayerBulletBase
{
    //  플레이어 기본 총알 클래스


    private void Start()
    {

        // 플레이어의 공격력(데미지) 캐싱
        _damage = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>()._damage;
        _playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
    }

}
