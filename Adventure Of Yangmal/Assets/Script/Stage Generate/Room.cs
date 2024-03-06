using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // ���� ����
    public RoomPositionInfo _roomInfo;
    public GameObject _roomManager;


    // ���� ���� ũ��, ���� ũ��
    public int _width;
    public int _height;


    // ���� ���� ��
    public Door _leftDoor;
    public Door _rightDoor;
    public Door _topDoor;
    public Door _bottomDoor;

    public List<Door> _doors = new List<Door>();


    // ������ �� �׸���
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_width, _height, 0));
    }

    private void Start()
    {
        RoomPositionInfo info = new RoomPositionInfo();
        info.roomPosition = new Vector2Int(0, 0);
        info.realPosition = new Vector2Int(0, 0);

        _roomInfo = info;

        _doors.Add(_leftDoor);
        _doors.Add(_rightDoor);
        _doors.Add(_topDoor);
        _doors.Add(_bottomDoor);

        // ���ʿ��� �� ����
        for (int i = 0; i < _doors.Count; i++)
            _doors[i].RemoveDoor();
    }

    public Vector2Int GetRoomCenter()
    {
        return _roomInfo.realPosition;
    }

}
