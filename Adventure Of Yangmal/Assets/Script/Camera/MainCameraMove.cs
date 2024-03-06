
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    //�÷��̾ ���󰡴� ī�޶� �̵�
    //ī�޶� �� ���� ������ ���ϰ� ��

    // minTransf x = 0, y = -17
    // max Transf x = 0, y = 17

    public enum eCAMERA_MODE
    {
        NORMAL,
        BOSS2_FOLLOW
    }

    public eCAMERA_MODE _curMode = eCAMERA_MODE.NORMAL;
    public Vector2 _minTransf; //ī�޶� �ּ� ��ġ _ ���� ũ�⿡ ���� ����
    public Vector2 _maxTransf; //ī�޶� �ִ� ��ġ _ ���� ũ�⿡ ���� ����

    public GameObject _player;
    public float _cameraMoveSpeed;



    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");

    }

    void FixedUpdate()
    {
        //�⺻��
        CameraMove(_player.transform.position,MainCameraMove.eCAMERA_MODE.NORMAL);
    }

    public void CameraMove(Vector3 destination,MainCameraMove.eCAMERA_MODE mode)
    {
        //ī�޶� �̵����ѱ��� ��庰�� ���� & destination ����
        switch(mode)
        {
            case eCAMERA_MODE.NORMAL:
                destination = FixCameraTransform(destination, _minTransf.x, _maxTransf.x, _minTransf.y, _maxTransf.y);
                break;
            case eCAMERA_MODE.BOSS2_FOLLOW:
                destination = FixCameraTransform(destination, - 10f, 10f, -25f, _maxTransf.y);
                break;
        }

        //������ destination���� ī�޶� �̵�
        transform.position = 
            Vector3.Lerp(transform.position, destination, Time.deltaTime * _cameraMoveSpeed);
    }


    // ī�޶� �̵����� ���� �޼���
    Vector3 FixCameraTransform(Vector3 destination, float minX,float maxX, float minY, float maxY)
    {
        destination.x = Mathf.Clamp(destination.x, minX, maxX); // x�� �ּ�~�ִ�
        destination.y = Mathf.Clamp(destination.y, minY, maxY); //y�� �ּ�~ �ִ�
        destination.z = transform.position.z;

        return destination;
    }



}
