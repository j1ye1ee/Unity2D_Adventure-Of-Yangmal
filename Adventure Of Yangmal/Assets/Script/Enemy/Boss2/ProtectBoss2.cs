using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBoss2 : MonoBehaviour
{

    // boss2 �� ������ ������ ���� �ݴ� �������� ƨ�ܳ�
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Boss2")
        {
            Rigidbody2D rigid;
            rigid = gameObject.GetComponent<Rigidbody2D>();
            Vector2 newPosition;
            newPosition = (other.gameObject.transform.position - gameObject.transform.position).normalized * -1f;

            rigid.AddForce(newPosition * 30f, ForceMode2D.Impulse);
        }
    }


}
