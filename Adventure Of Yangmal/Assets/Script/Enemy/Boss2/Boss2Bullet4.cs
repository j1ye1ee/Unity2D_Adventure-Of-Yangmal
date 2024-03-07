using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet4 : MonoBehaviour
{
    public float _damage;
    Boss2Bullet4Count _parent;


    void Start()
    {
        // ���ӿ��� ���°� �ƴ϶�� ĳ��
        // if(!_isGameOver)
        if (!GameManager.Instance._isGameOver)
        {
            _damage = FindObjectOfType<Boss2>()._damage;
        }
        _parent = transform.GetComponentInParent<Boss2Bullet4Count>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //���ӿ��� ���°� �ƴ϶�� �浹ȿ�� ����

        if (!GameManager.Instance._isGameOver)
        {
            // ����ִ� �÷��̾�� �浹��
            if (other.tag == "Player" && PlayerStatus.Instance._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                // �÷��̾� ������ ȿ��
                PlayerStatus.Instance.PlayerGetDamage(_damage);

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
