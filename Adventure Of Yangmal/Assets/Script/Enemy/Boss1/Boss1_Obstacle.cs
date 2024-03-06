using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Obstacle : MonoBehaviour
{
    // boss1 �� ����Ʈ ��ֹ��� ȿ�� Ŭ����


    public GameObject _player;
    public PlayerStatus _playerStatus;
    public float _damage;

    void Start()
    {
        // �÷��̾ �����Ѵٸ� ĳ��
        if (GameObject.FindWithTag("Player") != null)
        {
            _player = GameObject.FindWithTag("Player");
            _playerStatus = PlayerStatus.Instance;
        }
    }


    // �÷��̾� ����� ������ ������ _Trigger
    // ����Ʈ�� �÷��̾� �Ѿ� �浹�� ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // �÷��̾�� ������ ����
            _playerStatus.PlayerGetDamage(_damage);
        }

        else if (other.gameObject.tag == "Player Bullet")
            this.gameObject.SetActive(false);

    }

}
