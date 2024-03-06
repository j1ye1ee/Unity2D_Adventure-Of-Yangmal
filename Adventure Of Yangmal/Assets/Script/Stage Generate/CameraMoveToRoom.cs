using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveToRoom : MonoBehaviour
{

    public bool _isStartmove = false;
    public Vector3 _moveToPosition;
    public float _moveSpeed;

    void Start()
    {
        _moveToPosition = transform.position;
    }

    void Update()
    {
        // ī�޶� ������ ����
        if (_isStartmove)
            MoveToRoom();
    }

    void MoveToRoom()
    {
        // �� ������ ������ �̵�
        if (transform.position == _moveToPosition)
            _isStartmove = false;

        else
            transform.position = Vector3.MoveTowards(transform.position, _moveToPosition, Time.deltaTime * _moveSpeed);
    }



}
