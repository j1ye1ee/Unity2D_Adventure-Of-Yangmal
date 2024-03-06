using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum eDIRECTION
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public class RoomPositionInfo
{
    public Vector2Int roomPosition;
    public Vector2Int realPosition;
}


// 랜덤하게 방 생성 위치를 지정하는 클래스
public class SetRandomRoomPosition : MonoBehaviour
{
    public static SetRandomRoomPosition instance;

    public string _AdditiveStageName;
    public string _BossStageEnterRoom;
    public string _ShopRoomName;

    public int _minRoomCount;
    public int _maxRoomCount;

    public int _roomWidth;
    public int _roomHeight;

    public List<Vector2Int> _randomRoomPositionList = new List<Vector2Int>();
    public Queue<RoomPositionInfo> _roomPositionQueue = new Queue<RoomPositionInfo>();

    Vector2Int _curPos;
    public bool _isExistPosition = false;



    // enum 방향에 따른 Vector2Int 설정
    private static readonly Dictionary<eDIRECTION, Vector2Int> _position
        = new Dictionary<eDIRECTION, Vector2Int>
        {
            {eDIRECTION.UP, Vector2Int.up  },
            { eDIRECTION.RIGHT, Vector2Int.right },
            { eDIRECTION.DOWN, Vector2Int.down},
            {eDIRECTION.LEFT, Vector2Int.left }
        };


    private void Awake()
    {
        instance = this;

        // (0,0) 을 기준으로 한 칸씩 옮기며 방 위치를 지정한다.
        _randomRoomPositionList.Add(Vector2Int.zero);
        _curPos = _randomRoomPositionList[0];
        // 위치 생성
        MakeRandomPositionList();
    }


    private void Start()
    {
        // 생성한 위치별로 방 생성
        StartCoroutine(LoadRoom());
    }


    // 랜덤한 방 개수만큼의 로드 위치 설정 & 리스트 만들기
    public void MakeRandomPositionList()
    {
        // 1) 랜덤한 방 개수 설정
        int count = SetRandomRoomCount();

        // 2) 방 개수만큼 랜덤 position List 생성
        for (int i = 0; i < count; i++)
        {
            // 새로운 방향 설정
            eDIRECTION newDirection = (eDIRECTION)Random.Range(0, _position.Count);
           // 현재 좌표에 새로운 방향좌표를 더함
            _curPos += _position[newDirection];
            _isExistPosition = false;
           // 생성된 좌표가 List에 있는지 확인
            IsExistPosition(_curPos);

            while (_isExistPosition)
            {
                // 좌표 원위치
                _curPos -= _position[newDirection];
                // 새로운 방향 설정
                newDirection = (eDIRECTION)Random.Range(0, _position.Count);
                // 현재 좌표에 새로운 방향좌표를 더함
                _curPos += _position[newDirection];
                // 변수 초기화
                _isExistPosition = false;
                // 생성된 좌표가 List에 있는지 확인
                IsExistPosition(_curPos);
                // 중복 발생하지 않았을 때 break
                if (!_isExistPosition)
                    break;
            }

            // 중복 발생하지 않았을 때 roomPositionList 와 roomPosition 큐에 더함
            if (!_isExistPosition)
            {
                _randomRoomPositionList.Add(_curPos);

                // RoomInfo 정의
                RoomPositionInfo roomInfo = new RoomPositionInfo();
                roomInfo.roomPosition = new Vector2Int(_curPos.x, _curPos.y);
                roomInfo.realPosition = new Vector2Int(_curPos.x * _roomWidth, _curPos.y *_roomHeight);

                // 큐 더하기
                _roomPositionQueue.Enqueue(roomInfo);
            }
        }// for (int i = 0; i < count; i++)


    }

    // 랜덤한 방 개수 반환
    public int SetRandomRoomCount()
    {
        int count = Random.Range(_minRoomCount, _maxRoomCount+1);
        return count;
    }

    // 리스트 내부에 같은 좌표가 있는지 확인
    void IsExistPosition(Vector2Int pos)
    {
        foreach (Vector2Int position in _randomRoomPositionList)
        {
            if (position == pos)
                _isExistPosition = true;
        }
    }
    
    // bool형 반환하는 리스트 내부 방 확인 메서드
    public bool IsExistRoom(Vector2Int pos)
    {
        bool isExist = false;

        foreach (Vector2Int position in _randomRoomPositionList)
        {
            if (position == pos)
                isExist = true;
        }
        return isExist;
    }

    
    //  startScene에 준비된 방을 더함
    public IEnumerator LoadRoom()
    {
        for(int i = 0; i < _randomRoomPositionList.Count-1; i++)
        {
            AsyncOperation loadRoom;

            // 두 번째 방은 itemShop이 되도록
            if (i == 1)
                loadRoom = SceneManager.LoadSceneAsync(_ShopRoomName, LoadSceneMode.Additive);

            else if (i == _randomRoomPositionList.Count - 2)
                loadRoom = SceneManager.LoadSceneAsync(_BossStageEnterRoom, LoadSceneMode.Additive);

            else
                loadRoom = SceneManager.LoadSceneAsync(_AdditiveStageName, LoadSceneMode.Additive);
            

            while (loadRoom.isDone == false)
            {
                yield return null;
            }
        }
    }

    }
