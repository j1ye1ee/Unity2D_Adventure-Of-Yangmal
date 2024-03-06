using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot1 : MonoBehaviour, IEnemyShoot
{
    // enemy1 ����
    // ��� ������� ����

    public float _shootTerm; // ����
    public float _bulletSpeed;
    public float _damage;
    Enemy1BulletPoolManager _poolManager;

    Enemy _enemy;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _damage = _enemy._damage;
    }

    void Start()
    {
        _poolManager = GameObject.FindWithTag("enemy1BulletPool").GetComponent<Enemy1BulletPoolManager>();
    }

    // ���� �ڷ�ƾ ����
    public IEnumerator EnemyShoot()
    {
       // �� ���°� Die �� �ƴ� ��
        while (_enemy._curState !=Enemy.eENEMY_STATE.DIE)
        {
            // 2~3�� ���
            yield return new WaitForSeconds(Random.Range(2f, 3f));

            // 4�� ���� ����
            for (int time = 0; time < 4; time++)
            {
                // ���� �ð���ŭ ���
                yield return new WaitForSeconds(_shootTerm);
                // �Ѿ� ����_pool ���� �뿩
                GameObject bullet = _poolManager.GetObject(
                    _poolManager._enemy1BulletPool,
                    _poolManager._enemy1BulletPrefab,
                    transform.position,
                    transform.rotation
                    );

                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

                // ���� Ƚ���� ���� ���� ���� --> ��, ������, �Ʒ�, ���� ����
                if(time == 0)
                    bulletRigid.AddForce(Vector2.up * _bulletSpeed, ForceMode2D.Impulse);
                else if(time == 1)
                    bulletRigid.AddForce(Vector2.right * _bulletSpeed, ForceMode2D.Impulse);
                else if(time ==2)
                    bulletRigid.AddForce(Vector2.down * _bulletSpeed, ForceMode2D.Impulse);
                else if(time == 3)
                    bulletRigid.AddForce(Vector2.left * _bulletSpeed, ForceMode2D.Impulse);
            }//for(int time=0...)
        }//while(...)
    }//IEnumerator EnemyShoot


    
}
