using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShoot4 : MonoBehaviour, IEnemyShoot
{
    // enemy4 ����
    // ���缭 �εε� ���� ���� ����

    public float _waitSeconds;
    public float _shootTerm; // ����
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
        // ������� ���� �׸���
        Debug.DrawRay(transform.position, Vector2.up.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.right.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.down.normalized * _rayDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.left.normalized * _rayDistance, Color.blue);

    }

    public IEnumerator EnemyShoot()
    {
        // �� ���°� Die �� �ƴ� ��
        while (_enemy._curState != Enemy.eENEMY_STATE.DIE)
        {
            // ���� �ӵ� ��� �� ���� �ӵ� 0���� �����
            float originSpeed = _enemy.setSpeed;
            _enemy._speed = 0f;

            // ������� �� �˻� �� ���� ���� ���ϱ�
             isWall();

            // ����� ���� �ƴϸ� ����
            if (!_isAllWall)
            {
                // �ټ� �� ����
                for (int time = 0; time < 5; time++)
                {
                    GameObject bullet = _poolManager.GetObject(
                        _poolManager._enemy4BulletPool,
                        _poolManager._enemy4BulletPrefab,
                        transform.position,
                        transform.rotation);

                    Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

                    bulletRigid.AddForce(_shootDir * _bulletSpeed, ForceMode2D.Impulse);

                    // ���� �ð���ŭ ���
                    yield return new WaitForSeconds(_shootTerm);

                }//for (int time...)

            }

            // ����� ���̸� ���������ʰ� ���
            else if (_isAllWall)
            {
                yield return new WaitForSeconds(_shootTerm * 5);
                _isAllWall = false;
            }

            // ���� ���� �� ���� �ӵ��� �ǵ�����
            _enemy._speed = originSpeed;

            //���� ���� �� ����Ʈ �ʱ�ȭ
            _Directions.Clear();
            _hitData.Clear();

            // ���� ���� �ð����� ���
            yield return new WaitForSeconds(_waitSeconds);
        }//while(..)
    }//IEnumerator EnemyShoot



    // ���� �� �������� �浹 �˻�
    void isWall()
    {
        // �� ���� �˻�
        _hitData.Add(
            Physics2D.Raycast
            (transform.position, Vector2.up, _rayDistance, LayerMask.GetMask("World"))
            );

        // ������ ���� �˻�
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.right, _rayDistance, LayerMask.GetMask("World"))
           );

        // �Ʒ��� ���� �˻�
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.down, _rayDistance, LayerMask.GetMask("World"))
           );

        // ���� ���� �˻�
        _hitData.Add(
           Physics2D.Raycast
           (transform.position, Vector2.left, _rayDistance, LayerMask.GetMask("World"))
           );

        for(int i =0; i<4; i++)
        {
            // ���� ���� �������� ����
            if (_hitData[i].collider == null)
            {
                if (i == 0) { _Directions.Add(Vector2.up); }
                else if (i == 1) { _Directions.Add(Vector2.right); }
                else if (i == 2) { _Directions.Add(Vector2.down); }
                else if (i == 3) { _Directions.Add(Vector2.left); }
            }
        }// for(int i =0; i<4; i++)

        // ���� ���⿡ ����� ���� �ƹ��͵� ���� ���
        if (_Directions.Count == 0)
        {
            _isAllWall = true;
            return;
        }

        // ���� ���⿡ ���� ���� ���
        else if (_Directions.Count != 0)
        {
            // ����Ʈ ���� ��ҿ��� ������ ������ �̾� _shootdir�� ����
            int random = Random.Range(0, _Directions.Count);
            _shootDir = _Directions[random];
            _isAllWall = false;
        }

    }


       
}
