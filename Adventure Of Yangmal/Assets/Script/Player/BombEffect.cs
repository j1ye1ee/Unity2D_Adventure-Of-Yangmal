using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : PlayerBulletBase
{
    // ÆøÅº ÀÌÆåÆ® Å¬·¡½º
    // Àû°ú ´ê¾Æµµ ´Ù¸¥ ÃÑ¾ËÃ³·³ »èÁ¦µÇ¸é ¾ÈµÈ´Ù --> _isSItem = true

    private void Start()
    {
        _isSItem = true;
        _damage = PlayerStatus.Instance._damage + 10;
    }


}
