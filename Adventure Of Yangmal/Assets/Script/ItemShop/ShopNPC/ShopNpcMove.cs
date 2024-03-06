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
            // 오른쪽 도착
            case eNPCMOVE.ARRIVE_RIGHT:
                ArriveRight();
                break;

            // 왼쪽 도착
            case eNPCMOVE.ARRIVE_LEFT:
                ArriveLeft();
                break;

            // 이동중
            case eNPCMOVE.STROLL:
                Stroll();
                break;

            case eNPCMOVE.TALK:
                Talk();
                break;
        }

    }

    // 양말과 충돌시
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

            // fleep 원상복구
            transform.localScale = _originScale;

            // 왼쪽 좌표 세팅
            _left = SetVector2Left();

            // _direction 세팅
            _direction = _left - _right;

            // 상태 = 움직임 & 시간리셋
            _curState = eNPCMOVE.STROLL;
            _time = 0f;

            // 이전 상태 설정
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

            // 뒤집기
            transform.localScale = _fleepScale;

            // 오른쪽 좌표 세팅
            _right = SetVector2Right();

            // _direction 세팅
            _direction = _right - _left;

            // 상태 = 움직임 & 시간리셋
            _curState = eNPCMOVE.STROLL;
            _time = 0f;

            // 이전 상태 설정
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



    // 왼쪽 랜덤벡터 지정
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

    // 오른쪽 랜덤벡터 지정
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
