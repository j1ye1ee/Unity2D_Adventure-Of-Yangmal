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


// �����ϰ� �� ���� ��ġ�� �����ϴ� Ŭ����
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



    // enum ���⿡ ���� Vector2Int ����
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

        // (0,0) �� �������� �� ĭ�� �ű�� �� ��ġ�� �����Ѵ�.
        _randomRoomPositionList.Add(Vector2Int.zero);
        _curPos = _randomRoomPositionList[0];
        // ��ġ ����
        MakeRandomPositionList();
    }


    private void Start()
    {
        // ������ ��ġ���� �� ����
        StartCoroutine(LoadRoom());
    }


    // ������ �� ������ŭ�� �ε� ��ġ ���� & ����Ʈ �����
    public void MakeRandomPositionList()
    {
        // 1) ������ �� ���� ����
        int count = SetRandomRoomCount();

        // 2) �� ������ŭ ���� position List ����
        for (int i = 0; i < count; i++)
        {
            // ���ο� ���� ����
            eDIRECTION newDirection = (eDIRECTION)Random.Range(0, _position.Count);
           // ���� ��ǥ�� ���ο� ������ǥ�� ����
            _curPos += _position[newDirection];
            _isExistPosition = false;
           // ������ ��ǥ�� List�� �ִ��� Ȯ��
            IsExistPosition(_curPos);

            while (_isExistPosition)
            {
                // ��ǥ ����ġ
                _curPos -= _position[newDirection];
                // ���ο� ���� ����
                newDirection = (eDIRECTION)Random.Range(0, _position.Count);
                // ���� ��ǥ�� ���ο� ������ǥ�� ����
                _curPos += _position[newDirection];
                // ���� �ʱ�ȭ
                _isExistPosition = false;
                // ������ ��ǥ�� List�� �ִ��� Ȯ��
                IsExistPosition(_curPos);
                // �ߺ� �߻����� �ʾ��� �� break
                if (!_isExistPosition)
                    break;
            }

            // �ߺ� �߻����� �ʾ��� �� roomPositionList �� roomPosition ť�� ����
            if (!_isExistPosition)
            {
                _randomRoomPositionList.Add(_curPos);

                // RoomInfo ����
                RoomPositionInfo roomInfo = new RoomPositionInfo();
                roomInfo.roomPosition = new Vector2Int(_curPos.x, _curPos.y);
                roomInfo.realPosition = new Vector2Int(_curPos.x * _roomWidth, _curPos.y *_roomHeight);

                // ť ���ϱ�
                _roomPositionQueue.Enqueue(roomInfo);
            }
        }// for (int i = 0; i < count; i++)


    }

    // ������ �� ���� ��ȯ
    public int SetRandomRoomCount()
    {
        int count = Random.Range(_minRoomCount, _maxRoomCount+1);
        return count;
    }

    // ����Ʈ ���ο� ���� ��ǥ�� �ִ��� Ȯ��
    void IsExistPosition(Vector2Int pos)
    {
        foreach (Vector2Int position in _randomRoomPositionList)
        {
            if (position == pos)
                _isExistPosition = true;
        }
    }
    
    // bool�� ��ȯ�ϴ� ����Ʈ ���� �� Ȯ�� �޼���
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

    
    //  startScene�� �غ�� ���� ����
    public IEnumerator LoadRoom()
    {
        for(int i = 0; i < _randomRoomPositionList.Count-1; i++)
        {
            AsyncOperation loadRoom;

            // �� ��° ���� itemShop�� �ǵ���
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
