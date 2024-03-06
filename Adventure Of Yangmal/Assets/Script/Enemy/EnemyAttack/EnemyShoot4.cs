using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShoot4 : MonoBehaviour, IEnemyShoot
{
    // enemy4 공격
    // 멈춰서 두두둥 연속 세발 공격

    public float _waitSeconds;
    public float _shootTerm; // 공속
    public float _bulletSpeed;
    public float _damage;
    public float _rayDistance;

    public bool _isAllWall = false;

    Enemy _enemy;
    Vector2 _shootDir;

    public List<Vector2> _Directions = new List<Vector2>();
    public List<RaycastHit2D> _hitData = new List<RaycastHit2D>();

    Enemy4BulletPoolManager _poolManager;


    void Start()
    {
        _poolManager = GameObject.FindWithTag("enemy4BulletPool").GetComponent<Enemy4BulletPoolManager>();
        _enemy = GetComponent<Enemy>();
        _damage = _enemy._damage;
        StartCoroutine(EnemyShoot());
    }

    private void Update()
    {
        // 사방으로 레이 그리기
        Debug.DrawRay(transform.position, Vector2.up.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.right.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.down.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.left.normalized * _rayDistance, Color.blue);

    }

    public IEnumerator EnemyShoot()
    {
        // 적 상태가 Die 가 아닐 때
        while (_enemy._curState != Enemy.eENEMY_STATE.DIE)
        {
            // 원래 속도 기억 후 적의 속도 0으로 만들기
            float originSpeed = _enemy.setSpeed;
            _enemy._speed = 0f;

            // 사방으로 벽 검사 후 슈팅 방향 정하기
             isWall();

            // 사방이 벽이 아니면 공격
            if (!_isAllWall)
            {
                // 다섯 번 공격
                for (int time = 0; time < 5; time++)
                {
                    GameObject bullet = _poolManager.GetObject(
                        _poolManager._enemy4BulletPool,
                        _poolManager._enemy4BulletPrefab,
                        transform.position,
                        transform.rotation);

                    Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

                    bulletRigid.AddForce(_shootDir * _bulletSpeed, ForceMode2D.Impulse);

                    // 공속 시간만큼 대기
                    yield return new WaitForSeconds(_shootTerm);

                }//for (int time...)

            }

            // 사방이 벽이면 공격하지않고 대기
            else if (_isAllWall)
            {
                yield return new WaitForSeconds(_shootTerm * 5);
                _isAllWall = false;
            }

            // 공격 종료 후 원래 속도로 되돌리기
            _enemy._speed = originSpeed;

            //공격 종료 후 리스트 초기화
            _Directions.Clear();
            _hitData.Clear();

            // 다음 공격 시간까지 대기
            yield return new WaitForSeconds(_waitSeconds);
        }//while(..)
    }//IEnumerator EnemyShoot



    // 공격 전 전방으로 충돌 검사
    void isWall()
    {
        // 위 방향 검사
        _hitData.Add(
            Physics2D.Raycast
            (transform.position, Vector2.up, _rayDistance, LayerMask.GetMask("World"))
            );

        // 오른쪽 방향 검사
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.right, _rayDistance, LayerMask.GetMask("World"))
           );

        // 아래쪽 방향 검사
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.down, _rayDistance, LayerMask.GetMask("World"))
           );

        // 왼쪽 방향 검사
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.left, _rayDistance, LayerMask.GetMask("World"))
           );

        for(int i =0; i<4; i++)
        {
            // 벽이 없을 경우방향을 저장
            if (_hitData[i].collider == null)
            {
                if (i == 0) { _Directions.Add(Vector2.up); }
                else if (i == 1) { _Directions.Add(Vector2.right); }
                else if (i == 2) { _Directions.Add(Vector2.down); }
                else if (i == 3) { _Directions.Add(Vector2.left); }
            }
        }// for(int i =0; i<4; i++)

        // 예비 방향에 저장된 것이 아무것도 없을 경우
        if (_Directions.Count == 0)
        {
            _isAllWall = true;
            return;
        }

        // 예비 방향에 뭔가 있을 경우
        else if (_Directions.Count != 0)
        {
            // 리스트 내부 요소에서 랜덤한 방향을 뽑아 _shootdir에 저장
            int random = Random.Range(0, _Directions.Count);
            _shootDir = _Directions[random];
            _isAllWall = false;
        }

    }


       
}
