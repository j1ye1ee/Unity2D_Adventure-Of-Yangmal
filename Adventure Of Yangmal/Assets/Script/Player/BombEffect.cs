using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : PlayerBulletBase
{
    // ��ź ����Ʈ Ŭ����
    // ���� ��Ƶ� �ٸ� �Ѿ�ó�� �����Ǹ� �ȵȴ� --> _isSItem = true

    private void Start()
    {
        _isSItem = true;
        _damage = PlayerStatus.Instance._damage + 10;
    }


}
