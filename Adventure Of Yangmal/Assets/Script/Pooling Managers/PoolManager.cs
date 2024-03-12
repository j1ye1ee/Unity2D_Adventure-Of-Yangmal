using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public void InstantiatePool(List<GameObject> pool, GameObject prefab, int count)
    {
        for(int i =0; i<count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.parent = transform;
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }

    }

    public GameObject GetObject(List<GameObject> pool, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject gameObj = null;
        // pool 내부의 오브젝트가 비활성 상태라면 빌려온다.
        foreach(GameObject obj in pool)
        {
            if(!obj.gameObject.activeSelf)
            {
                gameObj = obj;
                break;
            }
        }

        // pool 내부의 오브젝트가 전부 활성 상태여서 빌려올 수 없다면 생성
        if(!gameObj)
        {
            gameObj = Instantiate(prefab);
            gameObj.transform.parent = this.transform;
            gameObj.gameObject.SetActive(false);
            pool.Add(gameObj);
        }

        //gameObj에 대한 설정
        gameObj.transform.position = position;
        gameObj.transform.rotation = rotation;
        gameObj.gameObject.SetActive(true);

        // gameObj 반환
        return gameObj;
    }
}
