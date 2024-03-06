using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet1 : MonoBehaviour
{
    [SerializeField]
    float _damage;
    [SerializeField]
    PlayerStatus _playerStatus;
    [SerializeField]
    GameObject _player;

    private void Start()
    {
        // 게임오버 상태가 아니라면 캐싱
        if (!GameManager.Instance._isGameOver)
        {
            _damage = FindObjectOfType<EnemyShoot1>()._damage;
            _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 월드 벽 or 장애물에 충돌시 총알 삭제
        if (other.tag == "World" || other.tag == "wall")
            gameObject.SetActive(false);

        // 게임오버 상태가 아니라면 충돌효과 발현
        if (!GameManager.Instance._isGameOver)
        {
            // 살아있는 플레이어와 충돌시
            if (other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                // 플레이어 데미지 효과
                _playerStatus.PlayerGetDamage(_damage);
                // 총알 삭제
                gameObject.SetActive(false);
            }//else if
        }// if(!GameManager.Instance._isGameOver)
    }
}
