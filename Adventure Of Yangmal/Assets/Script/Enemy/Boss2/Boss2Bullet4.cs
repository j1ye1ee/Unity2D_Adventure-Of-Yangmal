using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet4 : MonoBehaviour
{
    public float _damage;
    public PlayerStatus _playerStatus;
    Boss2Bullet4Count _parent;


    void Start()
    {
        // 게임오버 상태가 아니라면 캐싱
        // if(!_isGameOver)
        if (!GameManager.Instance._isGameOver)
        {
            _damage = FindObjectOfType<Boss2>()._damage;
            _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        }
        _parent = transform.GetComponentInParent<Boss2Bullet4Count>();
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

                _parent._count++;
                if (_parent._count == 2)
                {
                    _parent._count = 0;
                    _parent.gameObject.SetActive(false);
                }
            }
        }// if(!GameManager.Instance._isGameOver)


        if (other.tag == "BossBulletCollider")
        {
            _parent._count++;

            if (_parent._count == 2)
            {
                _parent._count = 0;
                _parent.gameObject.SetActive(false);
            }
        }

    }
}
