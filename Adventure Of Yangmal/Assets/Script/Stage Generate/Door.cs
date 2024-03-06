using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Room _curRoom;

    public enum eDOORTYPE
    {
        RIGHT,
        LEFT,
        TOP,
        BOTTOM
    }

    public eDOORTYPE _doorType;
    public bool _isEnableDoor = true;
    public Vector2 _moveToCameraPosition;
    public Vector2 _playerMovePosition;
    float _playerMoveAmountSide = 27;
    float _playerMoveAmountTopDown = 18;

    // 도어 타입에 맞는 도어 room 에 참조
    private void Awake()
    {
        _curRoom = GetComponentInParent<Room>();

        switch(_doorType)
        {
            case eDOORTYPE.RIGHT:
                _curRoom._rightDoor = this;
                break;
            case eDOORTYPE.LEFT:
                _curRoom._leftDoor = this;
                break;
            case eDOORTYPE.TOP:
                _curRoom._topDoor = this;
                break;
            case eDOORTYPE.BOTTOM:
                _curRoom._bottomDoor = this;
                break;

        }
    }

    public void RemoveDoor()
    {
        // 써야 할 문인지 확인
        // 써야 할 문이면 true 반환
        _isEnableDoor = IsExistRoom(_doorType);

        // 써야 할 문이 아니라면 setActive false;
        if (!_isEnableDoor)
            gameObject.SetActive(false);

    }

    bool IsExistRoom(eDOORTYPE doorType)
    {
        // 룸리스트 참조를 위함
        SetRandomRoomPosition roomManager;
        roomManager = GameObject.FindWithTag("roomManager").GetComponent<SetRandomRoomPosition>();

        Vector2Int confirmPosition;

        bool isExist = false;

        switch(_doorType)
        {
            case eDOORTYPE.RIGHT:
                // 오른쪽에 방이 있으면 true 반환
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x + 1,
                        _curRoom._roomInfo.roomPosition.y);
                isExist = roomManager.IsExistRoom(confirmPosition);

                // 플레이어 태깅 후 이동할 카메라 포지션에 저장
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x + 1) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y) * _curRoom._height);
                    
                    // 플레이어 태깅 후 플레이어의 이동 포지션 저장
                    _playerMovePosition = new Vector2(
                    transform.position.x + _playerMoveAmountSide,
                    transform.position.y);
                }
                break;


            case eDOORTYPE.LEFT:
                // 왼쪽에 방이 있으면 true 반환 & 카메라 포지션에 저장
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x - 1,
                        _curRoom._roomInfo.roomPosition.y);
                isExist = roomManager.IsExistRoom(confirmPosition);

                // 플레이어 태깅 후 이동할 카메라 포지션에 저장
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x - 1) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y) * _curRoom._height);

                    // 플레이어 태깅 후 플레이어의 이동 포지션 저장
                    _playerMovePosition = new Vector2(
                    transform.position.x - _playerMoveAmountSide,
                    transform.position.y);
                }
                break;

            case eDOORTYPE.TOP:
                // 위쪽에 방이 있으면 true 반환 & 카메라 포지션에 저장
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x,
                        _curRoom._roomInfo.roomPosition.y +1);
                isExist = roomManager.IsExistRoom(confirmPosition);
                // 플레이어 태깅 후 이동할 카메라 포지션에 저장
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y + 1) * _curRoom._height);

                    // 플레이어 태깅 후 플레이어의 이동 포지션 저장
                    _playerMovePosition = new Vector2(
                    transform.position.x,
                    transform.position.y + _playerMoveAmountTopDown);
                }
                break;

            case eDOORTYPE.BOTTOM:
                // 아래쪽에 방이 있으면 true 반환 & 카메라 포지션에 저장
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x,
                        _curRoom._roomInfo.roomPosition.y -1 );
                isExist = roomManager.IsExistRoom(confirmPosition);
                // 플레이어 태깅 후 이동할 카메라 포지션에 저장
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y - 1) * _curRoom._height);

                    // 플레이어 태깅 후 플레이어의 이동 포지션 저장
                    _playerMovePosition = new Vector2(
                    transform.position.x,
                    transform.position.y - _playerMoveAmountTopDown);
                }
                break;
        }

        return isExist;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그시 카메라 스위칭
        if(other.tag == "Player")
        {
            CameraMoveToRoom camera = 
                GameObject.FindWithTag("MainCamera").GetComponent<CameraMoveToRoom>();

            // 카메라 움직일 위치 전달
            camera._moveToPosition = 
                new Vector3( _moveToCameraPosition.x,
                _moveToCameraPosition.y,
                camera.transform.position.z);
            // 카메라 움직임 시작
            camera._isStartmove = true;
            // 플레이어 이동
            other.transform.position = _playerMovePosition;


        }
    }
}
