using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpcMove : MonoBehaviour
{

    public enum eNPCMOVE
    { 
        STROLL,
        ARRIVE_RIGHT,
        ARRIVE_LEFT,
        TALK

    }

     Vector2 _left;
     Vector2 _right;
     Vector2 _direction;

    public GameObject _center;

    public eNPCMOVE _curState = eNPCMOVE.ARRIVE_RIGHT;
    public eNPCMOVE _preState;

    Rigidbody2D _myRigid;
    public float _moveSpeed;
    public float _originSpeed;

    float _time;
    float _timeLimit = 2f;

    Vector2 _originScale;
    Vector2 _fleepScale;

    private void Start()
    {
        _originSpeed = _moveSpeed;
        _myRigid = GetComponent<Rigidbody2D>();
        _right = _myRigid.position;

        _originScale = transform.localScale;
        _fleepScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        
    }



    private void Update()
    {
        _time += Time.deltaTime;
        switch( _curState)
        {
            // ������ ����
            case eNPCMOVE.ARRIVE_RIGHT:
                ArriveRight();
                break;

            // ���� ����
            case eNPCMOVE.ARRIVE_LEFT:
                ArriveLeft();
                break;

            // �̵���
            case eNPCMOVE.STROLL:
                Stroll();
                break;

            case eNPCMOVE.TALK:
                Talk();
                break;
        }

    }

    // �縻�� �浹��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            _curState = eNPCMOVE.TALK;
    }

    void Talk()
    {
        _myRigid.velocity = new Vector2(0, 0);
    }

    void ArriveRight()
    {
        if (_time > _timeLimit)
        {
            _moveSpeed = 0f;

            // fleep ���󺹱�
            transform.localScale = _originScale;

            // ���� ��ǥ ����
            _left = SetVector2Left();

            // _direction ����
            _direction = _left - _right;

            // ���� = ������ & �ð�����
            _curState = eNPCMOVE.STROLL;
            _time = 0f;

            // ���� ���� ����
           _preState = eNPCMOVE.ARRIVE_RIGHT;

        }
        else
            return;
    }

    void ArriveLeft()
    {
        if (_time > _timeLimit)
        {
            _moveSpeed = 0f;

            // ������
            transform.localScale = _fleepScale;

            // ������ ��ǥ ����
            _right = SetVector2Right();

            // _direction ����
            _direction = _right - _left;

            // ���� = ������ & �ð�����
            _curState = eNPCMOVE.STROLL;
            _time = 0f;

            // ���� ���� ����
            _preState = eNPCMOVE.ARRIVE_LEFT;
        }
        else
            return;
    }

   
    void Stroll()
    {
        _moveSpeed = _originSpeed;
        _myRigid.velocity = _moveSpeed * _direction.normalized;
    }



    // ���� �������� ����
    Vector2 SetVector2Left()
    {

        float x = _center.transform.position.x - 5;
        float y;
        Vector2 left;
        y = Random.Range(-2f, 2f);

        left.x = x;
        left.y = y;

        return left;
    }

    // ������ �������� ����
    Vector2 SetVector2Right()
    {
        float x = _center.transform.position.x + 5;
        float y;
        Vector2 right;
        y = Random.Range(-2f, 2f);

        right.x = x;
        right.y = y;

        return right;
    }




}
