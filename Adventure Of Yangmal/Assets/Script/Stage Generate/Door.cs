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

    // ���� Ÿ�Կ� �´� ���� room �� ����
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
        // ��� �� ������ Ȯ��
        // ��� �� ���̸� true ��ȯ
        _isEnableDoor = IsExistRoom(_doorType);

        // ��� �� ���� �ƴ϶�� setActive false;
        if (!_isEnableDoor)
            gameObject.SetActive(false);

    }

    bool IsExistRoom(eDOORTYPE doorType)
    {
        // �븮��Ʈ ������ ����
        SetRandomRoomPosition roomManager;
        roomManager = GameObject.FindWithTag("roomManager").GetComponent<SetRandomRoomPosition>();

        Vector2Int confirmPosition;

        bool isExist = false;

        switch(_doorType)
        {
            case eDOORTYPE.RIGHT:
                // �����ʿ� ���� ������ true ��ȯ
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x + 1,
                        _curRoom._roomInfo.roomPosition.y);
                isExist = roomManager.IsExistRoom(confirmPosition);

                // �÷��̾� �±� �� �̵��� ī�޶� �����ǿ� ����
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x + 1) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y) * _curRoom._height);
                    
                    // �÷��̾� �±� �� �÷��̾��� �̵� ������ ����
                    _playerMovePosition = new Vector2(
                    transform.position.x + _playerMoveAmountSide,
                    transform.position.y);
                }
                break;


            case eDOORTYPE.LEFT:
                // ���ʿ� ���� ������ true ��ȯ & ī�޶� �����ǿ� ����
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x - 1,
                        _curRoom._roomInfo.roomPosition.y);
                isExist = roomManager.IsExistRoom(confirmPosition);

                // �÷��̾� �±� �� �̵��� ī�޶� �����ǿ� ����
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x - 1) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y) * _curRoom._height);

                    // �÷��̾� �±� �� �÷��̾��� �̵� ������ ����
                    _playerMovePosition = new Vector2(
                    transform.position.x - _playerMoveAmountSide,
                    transform.position.y);
                }
                break;

            case eDOORTYPE.TOP:
                // ���ʿ� ���� ������ true ��ȯ & ī�޶� �����ǿ� ����
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x,
                        _curRoom._roomInfo.roomPosition.y +1);
                isExist = roomManager.IsExistRoom(confirmPosition);
                // �÷��̾� �±� �� �̵��� ī�޶� �����ǿ� ����
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y + 1) * _curRoom._height);

                    // �÷��̾� �±� �� �÷��̾��� �̵� ������ ����
                    _playerMovePosition = new Vector2(
                    transform.position.x,
                    transform.position.y + _playerMoveAmountTopDown);
                }
                break;

            case eDOORTYPE.BOTTOM:
                // �Ʒ��ʿ� ���� ������ true ��ȯ & ī�޶� �����ǿ� ����
                confirmPosition =
                    new Vector2Int(
                        _curRoom._roomInfo.roomPosition.x,
                        _curRoom._roomInfo.roomPosition.y -1 );
                isExist = roomManager.IsExistRoom(confirmPosition);
                // �÷��̾� �±� �� �̵��� ī�޶� �����ǿ� ����
                if (isExist)
                {
                    _moveToCameraPosition = new Vector2(
                    (_curRoom._roomInfo.roomPosition.x) * _curRoom._width,
                    (_curRoom._roomInfo.roomPosition.y - 1) * _curRoom._height);

                    // �÷��̾� �±� �� �÷��̾��� �̵� ������ ����
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
        // �÷��̾� �±׽� ī�޶� ����Ī
        if(other.tag == "Player")
        {
            CameraMoveToRoom camera = 
                GameObject.FindWithTag("MainCamera").GetComponent<CameraMoveToRoom>();

            // ī�޶� ������ ��ġ ����
            camera._moveToPosition = 
                new Vector3( _moveToCameraPosition.x,
                _moveToCameraPosition.y,
                camera.transform.position.z);
            // ī�޶� ������ ����
            camera._isStartmove = true;
            // �÷��̾� �̵�
            other.transform.position = _playerMovePosition;


        }
    }
}
