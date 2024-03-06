using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMissile : PlayerBulletBase
{
    public enum eGUIDEMISSILE_STATE
    {
        DETACT,
        TRACE,
        DESTROY
    }

    public eGUIDEMISSILE_STATE _curState = eGUIDEMISSILE_STATE.DETACT;

    Rigidbody2D _myrigid;
    public float _range;

    public GameObject _target;
    public GameObject _missileEffectPrefab;

    public float _launchForce;
    public float _rotateSpeed;
    public float _moveSpeed;

    public bool _isCollision = false;

    Quaternion _rotateTarget;
    Color _destroyColor;
    Color _originColor;
    SpriteRenderer _sprite;

    bool _isDoingEffect = false;

    float _destroyTime = 5f;
    public float _destroyTimeCheck;



    private void Start()
    {
        // 적과 닿아도 다른 총알처럼 삭제되면 안된다 --> _isSItem = true
        _isSItem = true;
        _isMissile = true;

        _sprite = GetComponent<SpriteRenderer>();
        _originColor = _sprite.color;
        _destroyColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0 / 255f);
        _myrigid = GetComponent<Rigidbody2D>();
        _damage = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>()._damage + 5;
        _playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
    }



    private void FixedUpdate()
    {
        // 충돌 발생시 --> destroy 상태로 진입
        if (_isCollision)
            _curState = eGUIDEMISSILE_STATE.DESTROY;

        if (_destroyTimeCheck >= _destroyTime)
        {
            if (_curState != eGUIDEMISSILE_STATE.DESTROY)
                _curState = eGUIDEMISSILE_STATE.DESTROY;
        }

        switch (_curState)
        {
            case eGUIDEMISSILE_STATE.DETACT:
                DetactEnemy();
                break;
            case eGUIDEMISSILE_STATE.TRACE:
                TraceEnemy();
                break;
            case eGUIDEMISSILE_STATE.DESTROY:
                if (!_isDoingEffect)
                    StartCoroutine(MissileEffect());
                break;
        }

            _destroyTimeCheck += Time.deltaTime;


    }



    // 적 탐지
    void DetactEnemy()
    {
        RaycastHit2D ray;
        LayerMask mask = LayerMask.GetMask("Enemy");

        // _range 이내의 적 탐지
        ray = Physics2D.CircleCast(transform.position, _range, transform.forward,0f, mask);
        if (ray.collider== null)
            return;

        // circlaCast가 Enemy를 감지했다면 --> _isEnemy = true, 더이상 탐지하지 않음
        else if (ray.collider != null)
        {
            _target = ray.collider.gameObject;
            _curState = eGUIDEMISSILE_STATE.TRACE;
        }

    }

    // 적 추적
    void TraceEnemy()
    {
        // 1)_target이 존재한다면
        if (_target != null)
        {
            // 회전할 방향 설정
            Vector2 direction = (_target.gameObject.transform.position - _myrigid.transform.position).normalized;

            // 각도 계산 후 Quaternion 변환
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rotateTarget = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            // Slerp으로 미사일 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotateTarget, Time.fixedDeltaTime * _rotateSpeed);

            _myrigid.velocity = transform.up * _moveSpeed;
        }

        // 2)_target 이 존재하지 않는다면 다른 타겟을 재추적
        else if(_target == null)
        {
            _curState = eGUIDEMISSILE_STATE.DETACT;
        }
    }


    // 충돌시 이펙트 생성 --> 제거 코루틴
    IEnumerator MissileEffect()
    {
        _isDoingEffect = true;
        _sprite.color = _destroyColor;

        yield return null;

        // 이펙트 생성
        GameObject missileEffect = Instantiate(_missileEffectPrefab, transform.position, transform.rotation);
        Debug.Log("미사일 이펙트 생성");
        // 이펙트 페이드아웃
        yield return new WaitForSeconds(1f);
        StartCoroutine(EffectFadeOut(missileEffect));

        yield return new WaitUntil(() => missileEffect.GetComponent<SpriteRenderer>().color.a <= 0);

        // 관련 오브젝트 삭제
        Destroy(missileEffect);

        // 미사일 setActive(false) & 모든 설정 초기화
        gameObject.SetActive(false);
        _destroyTimeCheck = 0;
        _sprite.color = _originColor;
        _isDoingEffect = false;
        _isCollision = false;
        _curState = eGUIDEMISSILE_STATE.DETACT;


    }

    // 이펙트 페이드아웃
    IEnumerator EffectFadeOut(GameObject effect)
    {
        float a = 1f;
        SpriteRenderer sprite = effect.GetComponent<SpriteRenderer>();

        while (a >= 0)
        {
            a -= 0.05f;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
