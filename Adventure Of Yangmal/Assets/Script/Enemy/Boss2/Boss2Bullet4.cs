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
        // ���ӿ��� ���°� �ƴ϶�� ĳ��
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
        //���ӿ��� ���°� �ƴ϶�� �浹ȿ�� ����

        if (!GameManager.Instance._isGameOver)
        {
            // ����ִ� �÷��̾�� �浹��
            if (other.tag == "Player" && _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                // �÷��̾� ������ ȿ��
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
