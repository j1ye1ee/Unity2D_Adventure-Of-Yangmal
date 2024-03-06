using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveInput : MonoBehaviour
{
    // 플레이어의 움직임 상태를 결정하는 INPUT 클래스

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


    void Update() // 두가지 방향키 중첩에 따른 방향 판정을 함.
    {
        // 아래 방향키
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow)) //+오른쪽 방향키
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
                return;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))//+왼쪽 방향키
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
                return;
            }
            _ePlayerMoveState = ePLAYER_MOVE_STATE.DOWNWALK;
        }

        // 위 방향키
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow)) //+오른쪽 방향키
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
                return;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))// +왼쪽 방향키
            {
                _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
                return;
            }
            _ePlayerMoveState = ePLAYER_MOVE_STATE.UPWALK;

        }


        else if (Input.GetKey(KeyCode.RightArrow)) // 오른쪽 방향키
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.RIGHTWALK;
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) // 왼쪽 방향키
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.LEFTWALK;
        }

        else if (Input.GetKeyUp(KeyCode.DownArrow) ||
           Input.GetKeyUp(KeyCode.UpArrow) ||
           Input.GetKeyUp(KeyCode.RightArrow) ||
           Input.GetKeyUp(KeyCode.LeftArrow)) // 모든 방향키 눌렀다가 떼면 MOVE 상태 NONE
        {
            _ePlayerMoveState = ePLAYER_MOVE_STATE.NONE;
        }


        if (_ePlayerMoveState != ePLAYER_MOVE_STATE.NONE)
            _isMove = true;

        else
            _isMove = false;


    }
}
