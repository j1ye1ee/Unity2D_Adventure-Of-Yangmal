using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveInput : MonoBehaviour
{
    // �÷��̾��� ������ ���¸� �����ϴ� INPUT Ŭ����

    public bool _isMove = false;

    public enum ePLAYER_MOVE_STATE
    {
        NONE,
        DOWNWALK,
        RIGHTWALK,
        LEFTWALK,
        UPWALK
    }

    public ePLAYER_MOVE_STATE _ePlayerMoveState = ePLAYER_MOVE_STATE.NONE;


    void Update() // �ΰ��� ����Ű ��ø�� ���� ���� ������ ��.
    {
        // �Ʒ� ����Ű
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow)) //+������ ����Ű
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
                return;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))//+���� ����Ű
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
                return;
            }
            _ePlayerMoveState = ePLAYER_MOVE_STATE.DOWNWALK;
        }

        // �� ����Ű
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow)) //+������ ����Ű
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
                return;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))// +���� ����Ű
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
                return;
            }
            _ePlayerMoveState = ePLAYER_MOVE_STATE.UPWALK;

        }


        else if (Input.GetKey(KeyCode.RightArrow)) // ������ ����Ű
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) // ���� ����Ű
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
        }

        else if (Input.GetKeyUp(KeyCode.DownArrow) ||
           Input.GetKeyUp(KeyCode.UpArrow) ||
           Input.GetKeyUp(KeyCode.RightArrow) ||
           Input.GetKeyUp(KeyCode.LeftArrow)) // ��� ����Ű �����ٰ� ���� MOVE ���� NONE
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.NONE;
        }


        if (_ePlayerMoveState != ePLAYER_MOVE_STATE.NONE)
            _isMove = true;

        else
            _isMove = false;


    }
}
