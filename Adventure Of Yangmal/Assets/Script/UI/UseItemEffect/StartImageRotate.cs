using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartImageRotate : MonoBehaviour
{
    GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        Vector3 position = new Vector3(_player.transform.position.x, _player.transform.position.y + 4f, _player.transform.position.z);
        transform.position =
            Camera.main.WorldToScreenPoint(position);

        RotateStar();

    }

    void RotateStar()
    {
        transform.rotation *= Quaternion.Euler(0, -2, 0);
    }
}
