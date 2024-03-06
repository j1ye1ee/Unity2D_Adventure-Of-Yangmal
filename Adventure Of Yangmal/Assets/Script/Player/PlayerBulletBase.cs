using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    // �÷��̾��� �Ѿ� �θ�Ŭ����



    public PlayerShooter _playerShooter;
    Enemy _enemy;
    Boss _boss;
    public float _damage;
    public bool _isSItem = false;
    public bool _isMissile = false;


    // isTrigger on
    public void OnTriggerEnter2D(Collider2D other)
    {
        // 0) �̻����� ��� ���庮 �浹�ÿ��� �浹 ó��
        if(_isMissile)
            if(other.tag == "World")
                gameObject.GetComponent<GuideMissile>()._isCollision = true;

        // 1) ���� �� or ��ֹ��� �浹�� �Ϲ� �Ѿ� ����
        if (other.tag == "World" || other.tag == "wall")
        {
            // SItem �ƴҽ� setActive(fasle)
            if (!_isSItem)
                gameObject.SetActive(false);
        }

        
        // 2) �Ѿ˳��� �浹�� ���� �Ѿ˻��� & �÷��̾� �Ѿ� ����
        else if (other.gameObject.tag == "Enemy Bullet")
        {
            other.gameObject.SetActive(false);

            // SItem �ƴҽ� ����
            if (!_isSItem)
                gameObject.SetActive(false);

            // �̻����Ͻ� �̻��� Ŭ������ _isCollision = true;
            else if (_isMissile)
                gameObject.GetComponent<GuideMissile>()._isCollision = true;

        }


        // 3) enemy �� �浹&& ���� stun ���°� �ƴҽ�
        else if (other.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>()._curState != Enemy.eENEMY_STATE.STUN)
            {
                // enemy7�� ��� --> shake ���߿��� �Ѿ� ƨ�ܳ��� ���� ȿ���� ���� �ʴ� �̺�Ʈ �߻�
                if (other.gameObject.GetComponent<Enemy>()._isEnemy7)
                {
                    // shake �ڷ�ƾ ���� ���� ���& SItem �ƴ� ���
                    if (other.gameObject.GetComponent<Enemy7>()._isDoingShake && !_isSItem)
                    {
                        //  �Ѿ� ƨ��� / ���� ȿ�� ���� ����
                        gameObject.GetComponent<Rigidbody2D>().AddForce(_playerShooter._shootDir * -30f, ForceMode2D.Impulse);
                        StartCoroutine(WaitSetActive());
                        return;
                    }
                }

                // �� ���� ��� --> �� Ŭ���� ĳ��
                _enemy = other.gameObject.GetComponent<Enemy>();

                // �� hp ����
                _enemy.EnemyGetDamage(_damage);

                // SItem �ƴҽ� ����
                if (!_isSItem)
                    gameObject.SetActive(false);

                // �̻����Ͻ� �̻��� Ŭ������ _isCollision = true;
                else if (_isMissile)
                    gameObject.GetComponent<GuideMissile>()._isCollision = true;

            }

        }


        // ���� �浹�� & ���� ���� �ƴҽ�
        else if (other.gameObject.GetComponent<Boss>() != null)
        {
            if (other.gameObject.GetComponent<Boss>()._curState != Boss.eBOSS_STATE.STUN)
            {
                _boss = other.gameObject.GetComponent<Boss>();
                _boss.BossGetDamage(_damage);

                // SItem �ƴҽ� ����
                if (!_isSItem)
                    gameObject.SetActive(false);

                // �̻����Ͻ� �̻��� Ŭ������ _isCollision = true;
                else if (_isMissile)
                    gameObject.GetComponent<GuideMissile>()._isCollision = true;
            }

        }

    }//    public void OnTriggerEnter2D(Collider2D other)

    IEnumerator WaitSetActive()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
