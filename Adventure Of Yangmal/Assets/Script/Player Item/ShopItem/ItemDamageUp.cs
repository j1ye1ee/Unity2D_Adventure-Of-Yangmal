using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamageUp : ShopItem, IPlayerItem
{
    // 플레이어의 공격력을 높여주는 아이템


    float _damageUp = 3f;

    void Start()
    {
        _shopItemNum = 5;
        _maxValue = 70f;
        _effectText = "공격력 증가!";
    }


    public void UseItem()
    {
        Debug.Log(_damageUp + "만큼 공격력 상승");

        PlayerStatus playerStatus = PlayerStatus.Instance;
        PlayerBulletPoolManager poolManager = PlayerBulletPoolManager.Instance;

        // 공격력 up 아이템 사용시 플레이어의 최대 공격력을 넘게 된다면 maxdamage로 설정
        if (playerStatus._damage + _damageUp >= _maxValue)
            playerStatus.DamageSet(_maxValue);

        // 아닌 경우라면 아이템 정상 적용
        else
        {
            playerStatus.DamageUp(_damageUp);

            // 총알 프리팹 데미지 업
            poolManager._playerBulletPrefab.GetComponent<PlayerBullet>()._damage += _damageUp;

            poolManager._bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage += _damageUp;

            poolManager._guideMissilePrefab.GetComponent<GuideMissile>()._damage += _damageUp;


            // 총알 풀 리셋
            poolManager.ItemUsePoolSet();

            // ui 세팅
            UiManager.Instance.SetPlayerDamage();
        }
    }

    public override void SetIsMaxValue()
    {
        PlayerStatus playerStatus = PlayerStatus.Instance;
        if (playerStatus._damage >= _maxValue)
            _isMaxValue = true;
        else
            _isMaxValue = false;
    }
}
