using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGuard : PlayerBulletBase
{
    // 나비 아이템의 방어 클래스 = 데미지는 플레이어의 공격력 / 2 
    // 적과 닿아도 다른 총알처럼 삭제되면 안된다 --> _isSItem = true


    private void Start()
    {
        _playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
        _isSItem = true;
        _damage = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>()._damage / 2f;
    }


}
