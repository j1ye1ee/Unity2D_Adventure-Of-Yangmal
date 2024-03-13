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
                // page2 ���Խ� ī�޶� ������ ����
                if (_stageManager._curState == Boss2_StageManager.eSTAGE_STATE.PAGE2 && !_isSizeUpStart)
                    StartCoroutine(CameraSizeUp());

                // ī�޶� ���ǵ� ����
                if (_cameraMoveSpeed != _originCameraSpeed)
                    _cameraMoveSpeed = _originCameraSpeed;

                // �븻 ����
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
        // ī�޶� ���ǵ� up
        _cameraMoveSpeed = 6f;
        _isSizeDownStart = true;

        Debug.Log("ī�޶� ������ �ٿ�");
        float size = GetComponent<Camera>().orthographicSize;
        while(size > 7.5f)
        {
            size -= 0.05f;
            GetComponent<Camera>().orthographicSize = size;
            yield return null;
        }

        // ī�޶� ����ũ ���� ����
        GetComponent<CameraShake>()._isCameraShaking = false;
        GetComponent<CameraShake>()._isCameraShakeStart = false;
        GetComponent<CameraShake>().enabled = false;


    }

    IEnumerator CameraSizeUp()
    {
        _isSizeUpStart = true;

        float size = GetComponent<Camera>().orthographicSize;


        Debug.Log("ī�޶� ������ ��");
        while (size < _originCameraSize)
        {
            size += 0.05f;
            GetComponent<Camera>().orthographicSize = size;
            yield return null;
        }
    }


}
