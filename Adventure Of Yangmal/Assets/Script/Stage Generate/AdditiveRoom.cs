using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveRoom : Room
{
    // 더해지는 방 + 벽 + 적이 생긴다.
    public GameObject _walls;
    EnemySpawnerPoolManager _poolManager;



    private void Start()
    {
        _poolManager = GameObject.FindWithTag("enemySpawnerPool").GetComponent<EnemySpawnerPoolManager>();

        // 생성과 동시에 방 위치 변경
        _roomInfo = SetRandomRoomPosition.instance._roomPositionQueue.Dequeue(); 
        transform.position = new Vector3(_roomInfo.realPosition.x, _roomInfo.realPosition.y, 0);

        _doors.Add(_leftDoor);
        _doors.Add(_rightDoor);
        _doors.Add(_topDoor);
        _doors.Add(_bottomDoor);

        // 불필요한 문 제거
        for (int i = 0; i < _doors.Count; i++)
            _doors[i].RemoveDoor();

        // 벽 생성
        _roomManager = SetRandomRoomPosition.instance.gameObject;
        _walls = _roomManager.GetComponent<SetRandomWallAndEnemy>().SetRoomWalll();
        _walls.transform.position = new Vector3(GetRoomCenter().x, GetRoomCenter().y,0);

        // 적 생성
        InstantiateRandomEnemy();
    }

    void InstantiateRandomEnemy()
    {
        // walls 의 적 생성 위치 리스트 참조
        List<GameObject> enemySpots = _walls.GetComponent<Walls>()._enemySpots;
        for (int i=0; i<enemySpots.Count; i++)
        {
            enemySpots[i] = SetRandomEnemyAndInstantiate(enemySpots[i].gameObject.transform.position);
        }
    }

     GameObject SetRandomEnemyAndInstantiate(Vector3 position)
    {
        int randomType = 0;
        randomType = Random.Range(0, 7);

        
        GameObject enemy = null;
        List<GameObject> pool = null;

        switch (randomType)
        {
            case 0:
                pool = _poolManager._e1Pool;
                enemy = _poolManager._e1;
                break;
            case 1:
                pool = _poolManager._e2Pool;
                enemy = _poolManager._e2;
                break;
            case 2:
                pool = _poolManager._e3Pool;
                enemy = _poolManager._e3;
                break;
            case 3:
                pool = _poolManager._e4Pool;
                enemy = _poolManager._e4;
                break;
            case 4:
                pool = _poolManager._e5Pool;
                enemy = _poolManager._e5;
                break;
            case 5:
                pool = _poolManager._e6Pool;
                enemy = _poolManager._e6;
                break;
            case 6:
                pool = _poolManager._e7Pool;
                enemy = _poolManager._e7;
                break;
        }

        GameObject randomEnemy = _poolManager.GetObject(
            pool, enemy, transform.position, transform.rotation);

        return randomEnemy;
    }

}
