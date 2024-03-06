using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpcCollider : MonoBehaviour
{
    ShopNpcMove _npcMove;
    Collider2D _myCollider;

    void Start()
    {
        _npcMove = GameObject.FindWithTag("Shop").GetComponent<ShopNpcMove>();
        _myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shop")
        {
            Debug.Log("충돌");

            // 오른쪽에서 출발한 상태면 "왼쪽 도착"으로  & 콜라이더 끄기
            if (_npcMove._preState == ShopNpcMove.eNPCMOVE.ARRIVE_RIGHT)
            {
                _npcMove._curState = ShopNpcMove.eNPCMOVE.ARRIVE_LEFT;
                StartCoroutine(ColliderOn());
            }

            // 왼쪽에서 출발한 상태면 "오른쪽 도착"으로
            else if (_npcMove._preState == ShopNpcMove.eNPCMOVE.ARRIVE_LEFT)
            {
                _npcMove._curState = ShopNpcMove.eNPCMOVE.ARRIVE_RIGHT;
                StartCoroutine(ColliderOn());
            }
        }

        else
            return;
    }

    IEnumerator ColliderOn()
    {
        _myCollider.enabled = false;
        yield return new WaitForSeconds(0.05f);
        _myCollider.enabled = true;
    }
}
