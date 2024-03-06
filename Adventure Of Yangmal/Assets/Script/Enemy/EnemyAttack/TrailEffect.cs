using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    float time;
    PlayerStatus _playerStatus;


    // Player과 충돌시 플레이어 hp 5 만큼 차감
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _playerStatus.PlayerGetDamage(5f);
        }
    }

    private void Start()
    {
        // 게임종료가 아닐 시에만 캐싱
        if (!GameManager.Instance._isGameOver)
            _playerStatus = PlayerStatus.Instance;
    }

    void Update()
    {
        time += Time.deltaTime;

        // 생성된지 3초가 지났다면 콜라이더 해제
        if(time >= 3f)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
