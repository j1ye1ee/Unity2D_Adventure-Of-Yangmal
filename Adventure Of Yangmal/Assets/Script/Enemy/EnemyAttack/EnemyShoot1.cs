using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot1 : MonoBehaviour, IEnemyShoot
{
    // enemy1 공격
    // 사방 순서대로 공격

    public float _shootTerm; // 공속
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

    // 공격 코루틴 정의
    public IEnumerator EnemyShoot()
    {
       // 적 상태가 Die 가 아닐 때
        while (_enemy._curState !=Enemy.eENEMY_STATE.DIE)
        {
            // 2~3초 대기
            yield return new WaitForSeconds(Random.Range(2f, 3f));

            // 4번 연속 공격
            for (int time = 0; time < 4; time++)
            {
                // 공속 시간만큼 대기
                yield return new WaitForSeconds(_shootTerm);
                // 총알 생성_pool 에서 대여
                GameObject bullet = _poolManager.GetObject(
                    _poolManager._enemy1BulletPool,
                    _poolManager._enemy1BulletPrefab,
                    transform.position,
                    transform.rotation
                    );

                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

                // 공격 횟수에 따른 방향 설정 --> 위, 오른쪽, 아래, 왼쪽 순서
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
