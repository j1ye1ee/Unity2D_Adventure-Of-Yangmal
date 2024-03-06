using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
    // 모든 보스의 공통된 속성 클래스
{
    public enum eBOSS_STATE
    {
        IDLE,
        STUN,
        DIE
    }

    public eBOSS_STATE _curState = eBOSS_STATE.IDLE;

    // 보스의 죽음 효과를 위함
    public bool _isDead = false;
    public SpriteRenderer _spriteRenderer;
    public bool _isDoingDie = false;

    // 보스의 능력치
    public float _hp;
    public float _damage;

    // 스턴
    public bool _isStun = false;
    public bool _isDoingStun = false;

    // 플레이어에게 대미지를 주기 위함
    public PlayerStatus _playerStatus;
    public GameObject _player;

    public Rigidbody2D _myRigid;


    // 죽음 검사
    protected void IsDead()
    {
        if (_hp <= 0)
        {
            // 죽음 처리
            _isDead = true;
            _curState = eBOSS_STATE.DIE;
            _hp = 0;

            // 충돌로 인한 플레이어 데미지 차단
            gameObject.GetComponent<Collider2D>().enabled = false;

        }
    }

    // 적 죽음 코루틴
    protected IEnumerator Die()
    {
        // 죽음 코루틴 한 번 실행을 위함
        _isDoingDie = true;

        float _a = 1f;

        // 페이드 아웃 
        while (_a >= 0)
        {
            _a -= 0.05f;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // 코루틴 종료 후 오브젝트 삭제
        Destroy(gameObject);
    }


    // 공격 받고 스턴상태 진입 & 플레이어와 충돌시 데미지 입히기(boss isTrigger OO)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌시
        if (other.tag == "Player Bullet" )
        {
            // 스턴상태 진입
            _isStun = true;
        }

        // 플레이어와 충돌시 데미지 입힘
        if (other.gameObject.tag == "Player" &&
            _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _playerStatus.PlayerGetDamage(_damage);
        }

    }


    // 플레이어와 충돌시 데미지 입히기(boss isTrigger XX)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // 플레이어에게 데미지 입힘
            _playerStatus.PlayerGetDamage(_damage);
        }
    }


    // 플레이어와 충돌 지속시  플레이어가 적을 밀지 못하도록 처리 (boss isTrigger XX)
    private void OnCollisionStay2D(Collision2D other)
    {
       if (other.gameObject.tag == "Player" &&
              _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        
    }

    // 플레이어와 충돌 끝날시 원상복구(boss isTrigger XX)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            _myRigid.constraints = RigidbodyConstraints2D.None;
             _myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
         }
        
    }



    // 스턴 코루틴
    protected IEnumerator Stun()
    {

        // 스턴 코루틴 한번만 실행을 위함
        _isDoingStun = true;

        // 깜빡이는 효과 변수
        float _a = 0;


        // 2번 깜빡임
        for (int count = 0; count < 2; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.08f);
        }

        // 투명도 원위치
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);


        // 스턴 코루틴 한 번만 실행을 위함
        _isDoingStun = false;


        // 스턴 상태 벗어남 && STROLL 상태 진입
        _isStun = false;
        _curState = eBOSS_STATE.IDLE;


    }


    // 공격받았을시 보스의 hp 차감 & 카메라쉐이킹
    public void BossGetDamage(float damage)
    {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>()._isCameraShakeStart = true;
        _hp -= damage;
    }


}
