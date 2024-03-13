using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss2CameraController : MainCameraMove
{

    GameObject _boss2;
    Boss2_StageManager _stageManager;
    bool _isSizeDownStart = false;
    bool _isSizeUpStart = false;
    float _originCameraSize;
    float _originCameraSpeed;

    void Start()
    {
        _originCameraSpeed = _cameraMoveSpeed;
        _originCameraSize = GetComponent<Camera>().orthographicSize;
        _boss2 = GameObject.FindWithTag("Boss2");
        _player = GameObject.FindWithTag("Player");
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();
    }

    private void FixedUpdate()
    {
        switch(_curMode)
        {
            case eCAMERA_MODE.NORMAL:
                // page2 진입시 카메라 사이즈 원복
                if (_stageManager._curState == Boss2_StageManager.eSTAGE_STATE.PAGE2 && !_isSizeUpStart)
                    StartCoroutine(CameraSizeUp());

                // 카메라 스피드 원복
                if (_cameraMoveSpeed != _originCameraSpeed)
                    _cameraMoveSpeed = _originCameraSpeed;

                // 노말 무빙
                CameraMove(_player.transform.position, MainCameraMove.eCAMERA_MODE.NORMAL);
                break;

            case eCAMERA_MODE.BOSS2_FOLLOW:
                if (!_isSizeDownStart)
                    StartCoroutine(CameraSizeDown());

                // boss2 follow
                CameraMove(_boss2.transform.position, MainCameraMove.eCAMERA_MODE.BOSS2_FOLLOW);

                break;
        }

    }




    IEnumerator CameraSizeDown()
    {
        // 카메라 스피드 up
        _cameraMoveSpeed = 6f;
        _isSizeDownStart = true;

        Debug.Log("카메라 사이즈 다운");
        float size = GetComponent<Camera>().orthographicSize;
        while(size > 7.5f)
        {
            size -= 0.05f;
            GetComponent<Camera>().orthographicSize = size;
            yield return null;
        }

        // 카메라 쉐이크 관련 정지
        GetComponent<CameraShake>()._isCameraShaking = false;
        GetComponent<CameraShake>()._isCameraShakeStart = false;
        GetComponent<CameraShake>().enabled = false;


    }

    IEnumerator CameraSizeUp()
    {
        _isSizeUpStart = true;

        float size = GetComponent<Camera>().orthographicSize;


        Debug.Log("카메라 사이즈 업");
        while (size < _originCameraSize)
        {
            size += 0.05f;
            GetComponent<Camera>().orthographicSize = size;
            yield return null;
        }
    }


}
