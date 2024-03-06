using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
// 모든 적들의 공통된 속성 클래스
{
    public enum eENEMY_STATE
    {
        STROLL, //플레이어 발견 전
        FOLLOW, //플레이어 발견
        STUN, //스턴
        DIE, // 죽음
    }

    public eENEMY_STATE _curState = eENEMY_STATE.STROLL;

    // 적 움직임 클래스 참조
    protected EnemyMoveAi _ai;
    public Rigidbody2D _myRigid;

    // 플레이어 추적을 위함
    public GameObject _player;

    // 플레이어에게 대미지를 주기 위함
    public PlayerStatus _playerStatus;

    // 죽음 코루틴을 위함
    public SpriteRenderer _spriteRenderer;
    public bool _isDoingDie = false;
    public Color _originColor;

    // 적 클래스 공통된 변수
    public bool _isDead = false;
    public float _hp = 100;
    public float _damage;
    public float _speed;
    public float _range;
    public float _stunSpeed;

    public bool _isStun = false;
    public bool _isDoingStun = false;

    //SetEnemy를 위한 변수

    public float setHp;
    public float setDamage;
    public float setSpeed;
    public float setRange;

    //enemy7의 특수 케이스
    public bool _isEnemy7 = false;

    public AudioSource _damageSound;


    // 적 생성시 다양한 스탯으로의 초기화를 위한 메서드
    public void SetEnemy(float hp, float damage, float speed, float range)
    {
        _hp = hp;
        _damage = damage;
        _speed = speed;
        _range = range;
        _stunSpeed = _speed - 3f;
    }

    // 움직임 
    public virtual void Move()
    { }

    // 기즈모 드로잉
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }


    // 플레이어&적과의 거리 체크 
    protected bool IsPlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, _player.transform.position) <= range;
    }

    // 죽음 검사
    protected void IsDead()
    {
        if(_hp <= 0)
        {
            // 죽음 처리
            _isDead = true;
            _curState = eENEMY_STATE.DIE;
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
        while(_a >= 0)
        {
            _a -= 0.05f;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);
            yield return new WaitForSeconds(0.05f);
        }

        // 아이템 드랍
        Quaternion itemRotation = Quaternion.Euler(0, 0, 0);
        EnemyItemDrop enemyItem = GetComponent<EnemyItemDrop>();

        if (enemyItem._dropItem != null)
        {
            GameObject Item =
                Instantiate(enemyItem._dropItem, transform.position, itemRotation);
        }

        // 코루틴 종료 후 pooling 위해 setActive = false & 변수 초기화
        gameObject.SetActive(false);

        // 컬러, hp, 상태 초기화
        if (_isEnemy7)
        {
            gameObject.GetComponent<Enemy7>()._isDoingFollow = false;
            gameObject.GetComponent<Enemy7>()._isDoingShake = false;
        }

        _spriteRenderer.color = _originColor;
        _hp = setHp;
        _curState = eENEMY_STATE.STROLL;
        _isDead = false;
        _isDoingDie = false;
        _isStun = false;
        _isDoingStun = false;
        _speed = setSpeed;
        GetComponent<Collider2D>().enabled = true;

        // 스포너 링크 시
        if (GetComponent<SpawnerLink>() != null)
            GetComponent<SpawnerLink>()._isMinus = false;
    }

    // 공격 받고 스턴상태 진입
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌시 & enemy7이 아니라면
        if (other.tag == "Player Bullet" && !_isEnemy7)
        {
            // 스턴상태 진입
            _isStun = true;

        }
    }

    // 플레이어와 충돌시 데미지 입히기
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" &&
             _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
        {
            // 플레이어에게 데미지 입힘
            _playerStatus.PlayerGetDamage(_damage);
        }
    }


    // 플레이어와 충돌 지속시  플레이어가 적을 밀지 못하도록 처리 , Enemy7 은 예외
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!_isEnemy7)
        {
            if (other.gameObject.tag == "Player" &&
              _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }


    // 플레이어와 충돌 끝날시 원상복구, Enemy7은 예외
    private void OnCollisionExit2D(Collision2D other)
    {
        if (!_isEnemy7)
        {
            if (other.gameObject.tag == "Player" &&
                _playerStatus._curState == PlayerStatus.ePLAYER_STATE.ALIVE)
            {
                _myRigid.constraints = RigidbodyConstraints2D.None;
                _myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }


    // 스턴 코루틴
    protected IEnumerator Stun()
    {

        // ai 스크립트의 모든 코루틴 중지
        _ai.StopAllCoroutines();

        // 스턴 코루틴 한번만 실행을 위함
        _isDoingStun = true;

        // 깜빡이는 효과 변수
        float _a = 0;

        // 원래 속도 기억 후 스턴 속도로 바꾸기
        float originSpeed = setSpeed;
        _speed = _stunSpeed;

        // 2번 깜빡임
        for(int count = 0; count <2; count++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _a);

            if (_a == 0)
                _a = 1;
            else if (_a == 1)
                _a = 0;

            yield return new WaitForSeconds(0.08f);
        }

        // 속도 원상복구
        _speed = originSpeed;
 

        // 스턴 코루틴 한 번만 실행을 위함
        _isDoingStun = false;


        // 스턴 상태 벗어남 && STROLL 상태 진입
        _isStun = false;
        _curState = eENEMY_STATE.STROLL;

        // 랜덤방향 선택할 수 있도록 false  처리
        _ai._chooseDir = false;

    }

    // 적 hp 차감
    public void EnemyGetDamage(float damage)
    {
        _damageSound.Play();
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>()._isCameraShakeStart = true;
        _hp -= damage;
    }
}
