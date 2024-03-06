
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    //플레이어를 따라가는 카메라 이동
    //카메라가 맵 밖을 비추지 못하게 함

    // minTransf x = 0, y = -17
    // max Transf x = 0, y = 17

    public enum eCAMERA_MODE
    {
        NORMAL,
        BOSS2_FOLLOW
    }

    public eCAMERA_MODE _curMode = eCAMERA_MODE.NORMAL;
    public Vector2 _minTransf; //카메라 최소 위치 _ 맵의 크기에 따라 가변
    public Vector2 _maxTransf; //카메라 최대 위치 _ 맵의 크기에 따라 가변

    public GameObject _player;
    public float _cameraMoveSpeed;



    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");

    }

    void FixedUpdate()
    {
        //기본값
        CameraMove(_player.transform.position,MainCameraMove.eCAMERA_MODE.NORMAL);
    }

    public void CameraMove(Vector3 destination,MainCameraMove.eCAMERA_MODE mode)
    {
        //카메라 이동제한구역 모드별로 설정 & destination 설정
        switch(mode)
        {
            case eCAMERA_MODE.NORMAL:
                destination = FixCameraTransform(destination, _minTransf.x, _maxTransf.x, _minTransf.y, _maxTransf.y);
                break;
            case eCAMERA_MODE.BOSS2_FOLLOW:
                destination = FixCameraTransform(destination, - 10f, 10f, -25f, _maxTransf.y);
                break;
        }

        //설정한 destination으로 카메라 이동
        transform.position = 
            Vector3.Lerp(transform.position, destination, Time.deltaTime * _cameraMoveSpeed);
    }


    // 카메라 이동구역 제한 메서드
    Vector3 FixCameraTransform(Vector3 destination, float minX,float maxX, float minY, float maxY)
    {
        destination.x = Mathf.Clamp(destination.x, minX, maxX); // x축 최소~최대
        destination.y = Mathf.Clamp(destination.y, minY, maxY); //y축 최소~ 최대
        destination.z = transform.position.z;

        return destination;
    }



}
