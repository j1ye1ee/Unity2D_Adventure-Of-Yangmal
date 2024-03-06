using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEneterRoom : Room
{
    // Start is called before the first frame update
    void Start()
    {
        // 생성과 동시에 방 위치 변경
        _roomInfo = SetRandomRoomPosition.instance._roomPositionQueue.Dequeue();
        transform.position = new Vector3(_roomInfo.realPosition.x, _roomInfo.realPosition.y, 0);

        _doors.Add(_leftDoor);
        _doors.Add(_rightDoor);
        _doors.Add(_topDoor);
        _doors.Add(_bottomDoor);

        // 불필요한 문 제거
        for (int i = 0; i < _doors.Count; i++)
            _doors[i].RemoveDoor();
    }

    
}
