using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1Move : MonoBehaviour
{
    // boss1_1�� ������ ���� Ŭ����


    // �������� �����ϴ� position�� ��ġ ���� 
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

    // ������ �̵������� ���� �̵������� Ȯ��
    public bool _isGoingRight = true;



    void Start()
    {
        _boss1_1 = gameObject.GetComponent<Boss1_1>();
        _myRigid = gameObject.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        // 1) lerp �̵��� ���� _rate ����
        _rate += Time.deltaTime* _moveSpeed;

        // 2) boss1_1�� STROLL �����̰ų� DIE �����϶� ������ �̵��� ����Ѵ�.
        if(_boss1_1._iState == Boss1_1.eBOSS1_1STATE.STROLL
            || _boss1_1._iState == Boss1_1.eBOSS1_1STATE.DIE)
            MoveBoss(_startPosition);
    }


    // BOSS1_1 �̵�
    void MoveBoss(_eBOSS_POSITION position)
    {
        // 1) point15 ���� ��� && ������ �̵� ���¶�� --> ���� �̵����� ü����
        if (position == _eBOSS_POSITION.POINT10 && _isGoingRight == true)
            _isGoingRight = false;

        // 2) point0 ���� ��� && ���� �̵� ���¶�� --> ������ �̵����� ü����
        else if (position == _eBOSS_POSITION.POINT0 && _isGoingRight == false)
            _isGoingRight = true;

        // 3) ������ �̵� ���¶��
        if (_isGoingRight)
        {
            // p1 : ������ ~ ������+1 ���� lerp
            Vector2 p1 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition].transform.position,
                _movePoints[(int)_startPosition + 1].transform.position, _rate);

            // p2 : ������+1 ~ ������+2 ���� lerp
            Vector2 p2 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition + 1].transform.position,
                _movePoints[(int)_startPosition + 2].transform.position, _rate);

            // p3 : boss�� position
            Vector2 p3 = Vector2.Lerp(p1, p2, _rate);
            
            // �̵�
            _myRigid.position = p3;
        }

        // 4) ���� �̵� ���¶��
        if (!_isGoingRight)
        {
            // p1 : ������ ~ ������-1 ���� lerp
            Vector2 p1 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition].transform.position,
                _movePoints[(int)_startPosition - 1].transform.position, _rate);

            // p2 : ������-1 ~ ������-2 ���� lerp
            Vector2 p2 =
                Vector2.Lerp
                (_movePoints[(int)_startPosition - 1].transform.position,
                _movePoints[(int)_startPosition - 2].transform.position, _rate);

            // p3 : boss�� position
            Vector2 p3 = Vector2.Lerp(p1, p2, _rate);

            // �̵�
            _myRigid.position = p3;
        }

        // ���� POINT �� �����ߴٸ�
        if (_rate > 1f)
        {
            // rate �ʱ�ȭ
            _rate = 0;

            // ������ �������̶�� startPosition +2 
            if (_isGoingRight)
                _startPosition += 2;

            // ���� �������̶�� startPosition -2 
            else if (!_isGoingRight)
                _startPosition -= 2;
        }//if (_rate > 1f)
    }//void MoveBoss(_eBOSS_POSITION position)
}
