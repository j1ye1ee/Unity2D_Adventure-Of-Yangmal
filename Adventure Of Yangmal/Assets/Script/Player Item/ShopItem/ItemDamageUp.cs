using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamageUp : ShopItem, IPlayerItem
{
    // �÷��̾��� ���ݷ��� �����ִ� ������


    float _damageUp = 3f;

    void Start()
    {
        _shopItemNum = 5;
        _maxValue = 70f;
        _effectText = "���ݷ� ����!";
    }


    public void UseItem()
    {
        Debug.Log(_damageUp + "��ŭ ���ݷ� ���");

        PlayerStatus playerStatus = PlayerStatus.Instance;
        PlayerBulletPoolManager poolManager = PlayerBulletPoolManager.Instance;

        // ���ݷ� up ������ ���� �÷��̾��� �ִ� ���ݷ��� �Ѱ� �ȴٸ� maxdamage�� ����
        if (playerStatus._damage + _damageUp >= _maxValue)
            playerStatus.DamageSet(_maxValue);

        // �ƴ� ����� ������ ���� ����
        else
        {
            playerStatus.DamageUp(_damageUp);

            // �Ѿ� ������ ������ ��
            poolManager._playerBulletPrefab.GetComponent<PlayerBullet>()._damage += _damageUp;

            poolManager._bombPrefab.GetComponent<Bomb>()._bombEffectPrefab.GetComponent<PlayerBulletBase>()._damage += _damageUp;

            poolManager._guideMissilePrefab.GetComponent<GuideMissile>()._damage += _damageUp;


            // �Ѿ� Ǯ ����
            poolManager.ItemUsePoolSet();

            // ui ����
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
