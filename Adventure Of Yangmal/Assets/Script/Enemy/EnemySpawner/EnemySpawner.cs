using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    EnemySpawnerPoolManager _poolManager;
    GameObject _poolObject;

    List<GameObject> _pool;

    public enum eSPAWNER_STATE
    {
        SPAWN,
        NOSPAWN,
        DIE
    }


    public eSPAWNER_STATE _eState = eSPAWNER_STATE.SPAWN;


    public List<GameObject> _enemyPrefab = new List<GameObject>();

    public float _spawnTime;
    public float _curTime;

    public int _countMax;
    public int _count;


    public List<GameObject> _enemyList = new List<GameObject>();

    Animator _myAni;

    private void Start()
    {
        _myAni = GetComponent<Animator>();
        _poolManager = GameObject.FindWithTag("enemySpawnerPool").GetComponent<EnemySpawnerPoolManager>();
    }


    void Update()
    {
        IsCanSpawn();

        switch (_eState)
        {
            case eSPAWNER_STATE.SPAWN:
                if (_curTime >= _spawnTime)
                    SpawnEnemy();
                _curTime += Time.deltaTime;
                break;

            case eSPAWNER_STATE.NOSPAWN:
                break;

            case eSPAWNER_STATE.DIE:
                break;
        }


    }

    public void SpawnEnemy()
    {
        // 스폰된 적이 없다면 리스트 초기화
        if (_count == 0)
            _enemyList.Clear();

        SetRandomEnemy();

        // pool 에서 적 대여 후 리스트 추가
        GameObject enemy = _poolManager.GetObject(
            _pool,
            _poolObject,
            transform.position,
            transform.rotation);

        _enemyList.Add(enemy);


        // enemy생성하며 SpawnerLink 활성화 & spawner 등록
        enemy.GetComponent<SpawnerLink>().enabled = true;
        enemy.GetComponent<SpawnerLink>()._spawner = this.gameObject;

        _count++;
        _curTime = 0;  
    }



    // 스폰가능여부 확인
    public void IsCanSpawn()
    {
        // 0) _count  = 0 일때 스폰 가능 상태 전환
        if (_count == 0 && _eState!= eSPAWNER_STATE.DIE)
            _eState = eSPAWNER_STATE.SPAWN;


        // 1) 최대 숫자까지 적이 스폰되었다면 스폰 불가 상태로 전환
        else if (_count == _countMax && _eState!= eSPAWNER_STATE.DIE)
            _eState = eSPAWNER_STATE.NOSPAWN;
    }

    // 플레이어 총알에 맞았을 때 stun animation 실행
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player Bullet")
        {
            _myAni.SetTrigger("stun");
        }
    }


    // 랜덤 적 고르기
    void SetRandomEnemy()
    {
        int randomType = 0;
        randomType = Random.Range(0, 7);

        switch(randomType)
        {
            case 0:
                _poolObject = _poolManager._e1;
                _pool = _poolManager._e1Pool;
                break;
            case 1:
                _poolObject = _poolManager._e2;
                _pool = _poolManager._e2Pool;
                break;
            case 2:
                _poolObject = _poolManager._e3;
                _pool = _poolManager._e3Pool;
                break;
            case 3:
                _poolObject = _poolManager._e4;
                _pool = _poolManager._e4Pool;
                break;
            case 4:
                _poolObject = _poolManager._e5;
                _pool = _poolManager._e5Pool;
                break;
            case 5:
                _poolObject = _poolManager._e6;
                _pool = _poolManager._e6Pool;
                break;
            case 6:
                _poolObject = _poolManager._e7;
                _pool = _poolManager._e7Pool;
                break;
        }
    }
}
