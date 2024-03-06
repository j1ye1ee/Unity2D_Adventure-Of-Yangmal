using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어의 움직임을 담당하는 클래스
    //플레이어의 MOVE INPUT에 따라 이미지를 바꾸고 움직이게 한다.

    PlayerMoveInput _playerMoveInput;
    PlayerShooter _playerShooter;
    float _scale;

    public bool _isFleep = false;

    public Vector2 _lookDirection;
    SpriteRenderer _spriteRenderer;
    
    public Sprite[] _mySprite; //바뀔 스프라이트의 이미지 묶음
    public Rigidbody2D _myRigid; 
    public float _moveSpeed;

    public Animator _ani;

    Vector2 _origin;
    Vector2 _fleep;

    public AudioSource _walkSound;



    void Start()
    {

        _ani = GetComponent<Animator>();
        _playerShooter = GetComponent<PlayerShooter>();
        _playerMoveInput = GetComponent<PlayerMoveInput>();
        _myRigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _scale = transform.localScale.x;

        _origin = new Vector2(_scale, _scale);
        _fleep = new Vector2(-1 * _scale, _scale);
    }


    void FixedUpdate()
    {

        // input클래스의 상태를 받아  switch문으로 sprite를 교체한다.
        // 플레이어가 슈팅 상태가 아닐 때에만 이미지를 교체한다.
        if (!_playerShooter._isShoot)
        {
            switch (_playerMoveInput._ePlayerMoveState)
            {

                case PlayerMoveInput.ePLAYER_MOVE_STATE.NONE: //이미지 교체 하지 않음
                    NoneAni();
                    break;
                case PlayerMoveInput.ePLAYER_MOVE_STATE.RIGHTWALK:
                    RightWalkAni();
                    break;
                case PlayerMoveInput.ePLAYER_MOVE_STATE.LEFTWALK:
                    LeftWalkAni();
                    break;
                case PlayerMoveInput.ePLAYER_MOVE_STATE.UPWALK:
                    BackWalkAni();
                    break;
                case PlayerMoveInput.ePLAYER_MOVE_STATE.DOWNWALK:
                    FrontWalkAni();
                    break;
            }
        }//if(!_playerShooter._isShoot)
        

        Move(); //이미지 교체 후 움직임

    }


    public void Move() // 이동
    {
        // GetAxisRaw를 이용하여 8방향의 움직임으로만 제어한다.
        float sideValue = Input.GetAxisRaw("Horizontal");
        float upValue = Input.GetAxisRaw("Vertical");

        _lookDirection = Vector2.right * sideValue + Vector2.up * upValue;
        // 대각선 속도도 같게 만들기 위해 Vector.normalized 에 속도를 곱한다.
        _myRigid.velocity = _lookDirection.normalized *_moveSpeed;

        if (_playerMoveInput._isMove)
            if (_walkSound.isPlaying)
                return;
            else
                _walkSound.PlayOneShot(_walkSound.clip);

    }



    public void RightWalkAni()
    {
        // 뒤집기가 안 된 상태라면
        if (!_isFleep)
        {
            transform.localScale = _fleep;
            _isFleep = true;
        }


        _ani.SetBool("rightWalk", true);

        _ani.SetBool("leftWalk", false);
        _ani.SetBool("frontWalk", false);
        _ani.SetBool("backWalk", false);
    }



    public void LeftWalkAni()
    {
        // 뒤집기가 된 상태라면
        if (_isFleep)
        {
            transform.localScale = _origin;
            _isFleep = false;
        }

        _ani.SetBool("leftWalk", true);

        _ani.SetBool("rightWalk", false);
        _ani.SetBool("frontWalk", false);
        _ani.SetBool("backWalk", false);
    }



    public void FrontWalkAni()
    {
        _ani.SetBool("frontWalk", true);

        _ani.SetBool("rightWalk", false);
        _ani.SetBool("leftWalk", false);
        _ani.SetBool("backWalk", false);

    }

    public void BackWalkAni()
    {
        _ani.SetBool("backWalk", true);

        _ani.SetBool("rightWalk", false);
        _ani.SetBool("leftWalk", false);
        _ani.SetBool("frontWalk", false);
    }

    public void NoneAni()
    {
        _ani.SetBool("rightWalk", false);
        _ani.SetBool("leftWalk", false);
        _ani.SetBool("frontWalk", false);
        _ani.SetBool("backWalk", false);
    }

    public void ResetMoveSpeed()
    {
        _moveSpeed = 10;
    }

}
