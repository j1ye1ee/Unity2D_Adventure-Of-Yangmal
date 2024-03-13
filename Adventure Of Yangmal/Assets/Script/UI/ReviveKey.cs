using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveKey : MonoBehaviour
{
    [SerializeField]
    GameObject _failUi;

    void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Revive();
   
                for(int i = 0; i < _failUi.transform.childCount; i++)
                    _failUi.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void Revive()
    {
        PlayerStatus.Instance.PlayerReviveSet();

        if (PlayerStatus.Instance != null)
            PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;

        GameManager.Instance.SetIsGameOver(false);
    }
}
