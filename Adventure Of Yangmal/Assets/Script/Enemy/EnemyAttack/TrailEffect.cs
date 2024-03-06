using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    float time;
    PlayerStatus _playerStatus;


    // Player�� �浹�� �÷��̾� hp 5 ��ŭ ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _playerStatus.PlayerGetDamage(5f);
        }
    }

    private void Start()
    {
        // �������ᰡ �ƴ� �ÿ��� ĳ��
        if (!GameManager.Instance._isGameOver)
            _playerStatus = PlayerStatus.Instance;
    }

    void Update()
    {
        time += Time.deltaTime;

        // �������� 3�ʰ� �����ٸ� �ݶ��̴� ����
        if(time >= 3f)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
