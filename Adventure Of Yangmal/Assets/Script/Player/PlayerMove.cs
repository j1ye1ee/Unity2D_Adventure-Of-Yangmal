using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�÷��̾��� �������� ����ϴ� Ŭ����
    //�÷��̾��� MOVE INPUT�� ���� �̹����� �ٲٰ� �����̰� �Ѵ�.

    PlayerMoveInput _playerMoveInput;
    PlayerShooter _playerShooter;
    float _scale;

    public bool _isFleep = false;

    public Vector2 _lookDirection;
    SpriteRenderer _spriteRenderer;
    
    public Sprite[] _mySprite; //�ٲ� ��������Ʈ�� �̹��� ����
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

        // inputŬ������ ���¸� �޾�  switch������ sprite�� ��ü�Ѵ�.
        // �÷��̾ ���� ���°� �ƴ� ������ �̹����� ��ü�Ѵ�.
        if (!_playerShooter._isShoot)
        {
            switch (_playerMoveInput._ePlayerMoveState)
            {

                case PlayerMoveInput.ePLAYER_MOVE_STATE.NONE: //�̹��� ��ü ���� ����
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
        

        Move(); //�̹��� ��ü �� ������

    }


    public void Move() // �̵�
    {
        // GetAxisRaw�� �̿��Ͽ� 8������ ���������θ� �����Ѵ�.
        float sideValue = Input.GetAxisRaw("Horizontal");
        float upValue = Input.GetAxisRaw("Vertical");

        _lookDirection = Vector2.right * sideValue + Vector2.up * upValue;
        // �밢�� �ӵ��� ���� ����� ���� Vector.normalized �� �ӵ��� ���Ѵ�.
        _myRigid.velocity = _lookDirection.normalized *_moveSpeed;

        if (_playerMoveInput._isMove)
            if (_walkSound.isPlaying)
                return;
            else
                _walkSound.PlayOneShot(_walkSound.clip);

    }



    public void RightWalkAni()
    {
        // �����Ⱑ �� �� ���¶��
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
        // �����Ⱑ �� ���¶��
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
