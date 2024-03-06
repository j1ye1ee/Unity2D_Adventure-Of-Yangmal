using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Boss
{
    Boss2Move _move;
    Boss2_StageManager _StageManager;

    void Start()
    {
        _StageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();
        _myRigid = GetComponent<Rigidbody2D>();
        _move = GetComponent<Boss2Move>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
        _playerStatus = _player.GetComponent<PlayerStatus>();
    }

    void FixedUpdate()
    {
        // 1) ���� �˻�
        IsDead();

        // 2) ���� �˻�
        if (_isStun && _curState != eBOSS_STATE.DIE)
            _curState = eBOSS_STATE.STUN;


        // 3) �÷��̾� ���� �˻� -- �÷��̾� ���� �����϶� IDLE ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE || _player == null)
            _curState = eBOSS_STATE.IDLE;


        // 4) boss Ŭ������ ����
        switch (_curState)
        {
            case eBOSS_STATE.IDLE:
                break;

            case eBOSS_STATE.STUN:
                if (!_isDoingStun)
                    StartCoroutine(Stun());
                break;

            case eBOSS_STATE.DIE:
                if (!_isDoingDie)
                {
                    _StageManager._isBoss2Dead = true;
                    StopAllCoroutines();
                    _move._moveSpeed = 0;
                    StartCoroutine(Die());
                }
                break;
        }

    }

}
