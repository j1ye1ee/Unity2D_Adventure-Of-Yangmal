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

        // ���ݷ� up ������ ���� �÷��̾��� �ִ� ���ݷ��� �Ѱ� �ȴٸ� maxDex�� ����
        if (playerStatus._damage + _damageUp >= _maxValue)
            playerStatus.DamageSet(_maxValue);

        // �ƴ� ����� ������ ���� ����
        else
        {
            playerStatus.DamageUp(_damageUp);

            PlayerBulletPoolManager poolManager = PlayerBulletPoolManager.Instance;
            
            // �Ѿ� pool ��� ��������
            for (int i = 0; i < poolManager._playerBulletPool.Count; i++)
                poolManager._playerBulletPool[i].GetComponent<PlayerBullet>()._damage += _damageUp;

            // ��ź pool ��� ��������
            for (int i = 0; i < poolManager._bombPool.Count; i++)
                poolManager._bombPool[i].GetComponent<Bomb>()._bombEffectPrefab.GetComponent<BombEffect>()._damage += _damageUp;

            // �̻��� pool ��� ��������
            for (int i = 0; i < poolManager._guideMissilePool.Count; i++)
                poolManager._guideMissilePool[i].GetComponent<GuideMissile>()._damage += _damageUp;

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
