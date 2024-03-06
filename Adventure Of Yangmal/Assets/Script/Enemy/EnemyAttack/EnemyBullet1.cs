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
        // ���ӿ��� ���°� �ƴ϶�� ĳ��
        if (!GameManager.Instance._isGameOver)
        {
            _damage = FindObjectOfType<EnemyShoot1>()._damage;
            _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� �� or ��ֹ��� �浹�� �Ѿ� ����
        if (other.tag == "World" || other.tag == "wall")
            gameObject.SetActive(false);

        // ���ӿ��� ���°� �ƴ϶�� �浹ȿ�� ����
        if (!GameManager.Instance._isGameOver)
        {
            // ����ִ� �÷��̾�� �浹��
            if (other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                // �÷��̾� ������ ȿ��
                _playerStatus.PlayerGetDamage(_damage);
                // �Ѿ� ����
                gameObject.SetActive(false);
            }//else if
        }// if(!GameManager.Instance._isGameOver)
    }
}
