using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // �̱��� ���׸�

    private static T instance = null;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                // Ÿ�� ��ġ 
                instance = (T)FindObjectOfType(typeof(T));

                // ��ġ �Ŀ��� null �̶��
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
            // �θ� ������Ʈ or ���� ������Ʈ�� ������
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