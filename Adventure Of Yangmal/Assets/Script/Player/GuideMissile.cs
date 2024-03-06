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
        // ���� ��Ƶ� �ٸ� �Ѿ�ó�� �����Ǹ� �ȵȴ� --> _isSItem = true
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
        // �浹 �߻��� --> destroy ���·� ����
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



    // �� Ž��
    void DetactEnemy()
    {
        RaycastHit2D ray;
        LayerMask mask = LayerMask.GetMask("Enemy");

        // _range �̳��� �� Ž��
        ray = Physics2D.CircleCast(transform.position, _range, transform.forward,0f, mask);
        if (ray.collider== null)
            return;

        // circlaCast�� Enemy�� �����ߴٸ� --> _isEnemy = true, ���̻� Ž������ ����
        else if (ray.collider != null)
        {
            _target = ray.collider.gameObject;
            _curState = eGUIDEMISSILE_STATE.TRACE;
        }

    }

    // �� ����
    void TraceEnemy()
    {
        // 1)_target�� �����Ѵٸ�
        if (_target != null)
        {
            // ȸ���� ���� ����
            Vector2 direction = (_target.gameObject.transform.position - _myrigid.transform.position).normalized;

            // ���� ��� �� Quaternion ��ȯ
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rotateTarget = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            // Slerp���� �̻��� ȸ��
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotateTarget, Time.fixedDeltaTime * _rotateSpeed);

            _myrigid.velocity = transform.up * _moveSpeed;
        }

        // 2)_target �� �������� �ʴ´ٸ� �ٸ� Ÿ���� ������
        else if(_target == null)
        {
            _curState = eGUIDEMISSILE_STATE.DETACT;
        }
    }


    // �浹�� ����Ʈ ���� --> ���� �ڷ�ƾ
    IEnumerator MissileEffect()
    {
        _isDoingEffect = true;
        _sprite.color = _destroyColor;

        yield return null;

        // ����Ʈ ����
        GameObject missileEffect = Instantiate(_missileEffectPrefab, transform.position, transform.rotation);
        Debug.Log("�̻��� ����Ʈ ����");
        // ����Ʈ ���̵�ƿ�
        yield return new WaitForSeconds(1f);
        StartCoroutine(EffectFadeOut(missileEffect));

        yield return new WaitUntil(() => missileEffect.GetComponent<SpriteRenderer>().color.a <= 0);

        // ���� ������Ʈ ����
        Destroy(missileEffect);

        // �̻��� setActive(false) & ��� ���� �ʱ�ȭ
        gameObject.SetActive(false);
        _destroyTimeCheck = 0;
        _sprite.color = _originColor;
        _isDoingEffect = false;
        _isCollision = false;
        _curState = eGUIDEMISSILE_STATE.DETACT;


    }

    // ����Ʈ ���̵�ƿ�
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
