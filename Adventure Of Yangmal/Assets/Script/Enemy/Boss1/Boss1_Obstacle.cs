using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Obstacle : MonoBehaviour
{
    // boss1 의 이펙트 장애물의 효과 클래스


    public GameObject _player;
    public PlayerStatus _playerStatus;
    public float _damage;

    void Start()
    {
        // 플레이어가 존재한다면 캐싱
        if (GameObject.FindWithTag("Player") != null)
        {
            _player = GameObject.FindWithTag("Player");
            _playerStatus = PlayerStatus.Instance;
        }
    }


    // 플레이어 통과시 데미지 입히기 _Trigger
    // 이펙트랑 플레이어 총알 충돌시 삭제

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // 플레이어에게 데미지 입힘
            _playerStatus.PlayerGetDamage(_damage);
        }

        else if (other.gameObject.tag == "Player Bullet")
            this.gameObject.SetActive(false);

    }

}
