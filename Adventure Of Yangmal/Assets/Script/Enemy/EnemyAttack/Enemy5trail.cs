using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5trail : MonoBehaviour
{
    public GameObject[] _enemy5Trail;
    public List<GameObject> _trail = new List<GameObject>();
    float _generateTime;
    float _time;
    Enemy _enemy;

    Enemy5TrailPoolManager _poolManager;
    List<GameObject> _pool;
    GameObject _poolObject;

    Color _originColor;


 

    void Start()
    {
        _originColor = GetComponent<SpriteRenderer>().color;
        _poolManager = GameObject.FindWithTag("enemy5TrailPool").GetComponent<Enemy5TrailPoolManager>();
        _enemy = GetComponent<Enemy>();
        // ���� ����Ÿ�� ����
        SetRandomTime();
    }

    private void LateUpdate()
    {
        if (_enemy._isDoingDie)
            StopAllCoroutines();
    }


    // Update is called once per frame
    void Update()
    {
        // ���� �ڷ�ƾ �������� �ʾҴٸ� trail ����
        if (!_enemy._isDoingDie)
        {
            _time += Time.deltaTime;
            if (_time >= _generateTime)
            {
                // ���� �ð��� �Ǿ��ٸ� Trail ������ �ı�
                StartCoroutine(TrailSetAndDestroy());
                // �ð� �ʱ�ȭ & ���� �ð� �缳��
                SetRandomTime();
                _time = 0;
            }
        }//if (gameObject.GetComponent<Enemy>()._isDoingDie)

        else
            return;
    }



    // Trail ���� & �ı� �ڷ�ƾ
    IEnumerator TrailSetAndDestroy()
    {
        // ���� Ÿ�� ����
        SetRandomType();

        // trail ����
        GameObject enemyTrail =
            _poolManager.GetObject(
                _pool, _poolObject, transform.position, transform.rotation);

        enemyTrail.GetComponent<SpriteRenderer>().color = _originColor;

        _trail.Add(enemyTrail);

        // 3�� �� �ı�
        yield return new WaitForSeconds(3f);

        if(enemyTrail != null  )
            StartCoroutine(TrailDestroy(enemyTrail.GetComponent<SpriteRenderer>()));
    }


    // ���� Ÿ�� ����
    void SetRandomTime()
    {
        _generateTime = Random.Range(0.15f, 0.3f);
    }

    // ���� pool ����
    
    void SetRandomType()
    {
        int randomType = 0;
        randomType = Random.Range(0, 5);

        switch (randomType)
        {
            case 0:
                _pool =  _poolManager._t1Pool;
                _poolObject = _poolManager._t1;
                break;
            case 1:
                _pool = _poolManager._t2Pool;
                _poolObject = _poolManager._t2;
                break;
            case 2:
                _pool = _poolManager._t3Pool;
                _poolObject = _poolManager._t3;
                break;
            case 3:
                _pool = _poolManager._t4Pool;
                _poolObject = _poolManager._t4;
                break;
            case 4:
                _pool = _poolManager._t5Pool;
                _poolObject = _poolManager._t5;
                break;
        }
    }

    IEnumerator TrailDestroy(SpriteRenderer sprite)
    {
        float _a = 1f;

        if (!_enemy._isDoingDie)
        {
            // ���̵� �ƿ� 
            while (_a >= 0)
            {
                _a -= 0.05f;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, _a);
                yield return null;

                if (_a <= 0)
                {
                    // trailList ���� ����
                    _trail.Remove(sprite.gameObject);
                    sprite.gameObject.SetActive(false);
                }
            }//while
        }//if
    }
       
   

}

