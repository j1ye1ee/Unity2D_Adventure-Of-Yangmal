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
        // pool ������ ������Ʈ�� ��Ȱ�� ���¶�� �����´�.
        foreach(GameObject obj in pool)
        {
            if(!obj.gameObject.activeSelf)
            {
                gameObj = obj;
                break;
            }
        }

        // pool ������ ������Ʈ�� ���� Ȱ�� ���¿��� ������ �� ���ٸ� ����
        if(!gameObj)
        {
            gameObj = Instantiate(prefab);
            gameObj.transform.parent = this.transform;
            gameObj.gameObject.SetActive(false);
            pool.Add(gameObj);
        }

        //gameObj�� ���� ����
        gameObj.transform.position = position;
        gameObj.transform.rotation = rotation;
        gameObj.gameObject.SetActive(true);

        // gameObj ��ȯ
        return gameObj;
    }
}
