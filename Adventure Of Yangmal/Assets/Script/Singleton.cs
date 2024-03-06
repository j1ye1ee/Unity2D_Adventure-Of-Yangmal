using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 제네릭

    private static T instance = null;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                // 타입 서치 
                instance = (T)FindObjectOfType(typeof(T));

                // 서치 후에도 null 이라면
                if( instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // 부모 오브젝트 or 하위 오브젝트가 있을시
            if (transform.parent != null && transform.root.gameObject != null)
            {
                DontDestroyOnLoad(this.transform.root.gameObject);
            }

            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        else
            Destroy(this.gameObject);

    }
}