using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1Move : MonoBehaviour
{
    // boss1_1의 움직임 관리 클래스


    // 움직임을 시작하는 position의 위치 상태 
    public enum _eBOSS_POSITION
    {
        POINT0,
        POINT1,
        POINT2,
        POINT3,
        POINT4,
        POINT5,
        POINT6,
        POINT7,
        POINT8,
        POINT9,
        POINT10,
    }


    public Boss1_1 _boss1_1;
    public _eBOSS_POSITION _startPosition = _eBOSS_POSITION.POINT0;

    public GameObject[] _movePoints;
    public Rigidbody2D _myRigid;

    public  float _rate = 0f;
    public float _moveSpeed;

    // 오른쪽 이동중인지 왼쪽 이동중인지 확인
    public bool _isGoingRight = true;



    void Start()
    {
        _boss1_1 = gameObject.GetComponent<Boss1_1>();
        _myRigid = gameObject.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        // 1) lerp 이동을 위한 _rate 증가
        _rate += Time.deltaTime* _moveSpeed;

        // 2) boss1_1이 STROLL 상태이거나 DIE 상태일때 포물선 이동을 계속한다.
        if(_boss1_1._iState == Boss1_1.eBOSS1_1STATE.STROLL
            || _boss1_1._iState == Boss1_1.eBOSS1_1STATE.DIE)
            MoveBoss(_startPosition);
    }


    // BOSS1_1 이동
    void MoveBoss(_eBOSS_POSITION position)
    {
        // 1) point15 에서 출발 && 오른쪽 이동 상태라면 --> 왼쪽 이동으로 체인지
        if (position == _eBOSS_POSITION.POINT10 && _isGoingRight == true)
            _isGoingRight = false;

        // 2) point0 에서 출발 && 왼쪽 이동 상태라면 --> 오른쪽 이동으로 체인지
        else if (position == _eBOSS_POSITION.POINT0 && _isGoingRight == false)
            _isGoingRight = true;

        // 3) 오른쪽 이동 상태라면
        if (_isGoingRight)
        {
            // p1 : 시작점 ~ 시작점+1 까지 lerp
            Vector2 p1 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition].transform.position,
                _movePoints[(int)_startPosition + 1].transform.position, _rate);

            // p2 : 시작점+1 ~ 시작점+2 까지 lerp
            Vector2 p2 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition + 1].transform.position,
                _movePoints[(int)_startPosition + 2].transform.position, _rate);

            // p3 : boss의 position
            Vector2 p3 = Vector2.Lerp(p1, p2, _rate);
            
            // 이동
            _myRigid.position = p3;
        }

        // 4) 왼쪽 이동 상태라면
        if (!_isGoingRight)
        {
            // p1 : 시작점 ~ 시작점-1 까지 lerp
            Vector2 p1 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition].transform.position,
                _movePoints[(int)_startPosition - 1].transform.position, _rate);

            // p2 : 시작점-1 ~ 시작점-2 까지 lerp
            Vector2 p2 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition - 1].transform.position,
                _movePoints[(int)_startPosition - 2].transform.position, _rate);

            // p3 : boss의 position
            Vector2 p3 = Vector2.Lerp(p1, p2, _rate);

            // 이동
            _myRigid.position = p3;
        }

        // 다음 POINT 에 도달했다면
        if (_rate > 1f)
        {
            // rate 초기화
            _rate = 0;

            // 오른쪽 진행중이라면 startPosition +2 
            if (_isGoingRight)
                _startPosition += 2;

            // 왼쪽 진행중이라면 startPosition -2 
            else if (!_isGoingRight)
                _startPosition -= 2;
        }//if (_rate > 1f)
    }//void MoveBoss(_eBOSS_POSITION position)
}
