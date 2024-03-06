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
        // �÷��̾� �Ѿ˰� �浹�� �÷��̾��� ������ ��ŭ ������ hp ���� & ī�޶� ����ŷ
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
