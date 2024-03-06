using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class FireLightEffect : MonoBehaviour
{
    Light2D _light;
    public float _changeTime;
    float _curTime;

    void Start()
    {
        _light = GetComponent<Light2D>();
    }


    void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime > _changeTime)
        {
            SetRandomSize();
            _curTime = 0f;
            SetChangeTime();
        }

    }

    void SetRandomSize()
    {
        float random = Random.Range(0.9f, 1.1f);
        _light.intensity = random;
        
    }

    void SetChangeTime()
    {
        float random = Random.Range(0.05f, 0.2f);
        _changeTime = random;
    }
}
