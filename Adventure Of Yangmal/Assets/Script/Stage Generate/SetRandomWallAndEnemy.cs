using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomWallAndEnemy : MonoBehaviour
{
    public List<GameObject> _walls = new List<GameObject>();

    public GameObject SetRoomWalll()
    {
        int random = Random.Range(0, 10);
        return InstantiateWall(random);
    }

    GameObject InstantiateWall(int num)
    {
         GameObject walls = Instantiate(_walls[num], transform.position, transform.rotation);

        return walls;
    }

}
