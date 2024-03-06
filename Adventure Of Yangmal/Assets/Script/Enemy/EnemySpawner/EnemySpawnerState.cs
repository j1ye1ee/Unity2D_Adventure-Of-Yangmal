using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerState : MonoBehaviour
{

    public float _hp;
    public SpriteRenderer _spriteRenderer;
    public EnemySpawner _spawner;


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌시 플레이어의 데미지 만큼 스포너 hp 차감 & 카메라 쉐이킹
        if (other.tag == "Player Bullet")
        {
            if (other.GetComponent<PlayerBullet>() != null)
            {
                GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>()._isCameraShakeStart = true;
                _hp -= other.GetComponent<PlayerBullet>()._damage;
            }
        }
    }

    public void Die()
    {
        _spawner._eState = EnemySpawner.eSPAWNER_STATE.DIE;
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
        GetComponent<Collider2D>().enabled = false;
    }
}
