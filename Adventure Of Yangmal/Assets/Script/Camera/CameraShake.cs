using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    [SerializeField]
    float _shakePower;

    [SerializeField]
    float _shakeTime;

    float _curShakeTime;
    public bool _isCameraShakeStart = false;
    public bool _isCameraShaking = false;

    private void Start()
    {
        _curShakeTime = _shakeTime;

    }
    void FixedUpdate()
    {
        if(_isCameraShaking)
            _curShakeTime -= Time.deltaTime;

        if (_isCameraShakeStart)
        {
            _isCameraShaking = true;
            StartCoroutine(ShakeCamera());
        }
    }

    IEnumerator ShakeCamera()
    {
        _isCameraShakeStart = false;
        _curShakeTime = _shakeTime;


        while (_curShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * _shakePower + transform.position;
            yield return null;
        }

        _curShakeTime = _shakeTime;
    }
}
