using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet : MonoBehaviour
{
    public float _damage;
    public PlayerStatus _playerStatus;

    void Start()
    {
        // 게임오버 상태가 아니라면 캐싱
        // if(!_isGameOver)
        if (!GameManager.Instance._isGameOver)
        {
            _damage = FindObjectOfType<Boss2>()._damage;
            _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       //게임오버 상태가 아니라면 충돌효과 발현

        if (!GameManager.Instance._isGameOver)
        {
            // 살아있는 플레이어와 충돌시
            if (other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                // 플레이어 데미지 효과
                _playerStatus.PlayerGetDamage(_damage);

                gameObject.SetActive(false);
            }
        }// if(!GameManager.Instance._isGameOver)


        if (other.tag == "BossBulletCollider")
        {
            gameObject.SetActive(false);
        }

    }

}
