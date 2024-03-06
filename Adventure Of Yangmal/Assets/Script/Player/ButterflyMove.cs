using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMove : MonoBehaviour
{
    //  나비 아이템의 원운동 클래스

    public float _turnSpeed;
    GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        Turn();
        transform.position = _player.GetComponent<Rigidbody2D>().transform.position;
    }

    void Turn()
    {
        transform.rotation =  Quaternion.Euler(0, 0, _turnSpeed) * transform.rotation;
    }

}
