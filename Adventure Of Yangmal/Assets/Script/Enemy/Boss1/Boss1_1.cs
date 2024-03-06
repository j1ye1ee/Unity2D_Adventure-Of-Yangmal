using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1 : Boss
{
    // Boss 1_1 �� �ൿ&���� ���� Ŭ����


    public enum eBOSS1_1STATE
    {
        STROLL, // ���ƴٴϱ�
        STOP, // ��� ���߱�
        STEP, // ���� ��� ��
        DIE // ����
    }

    public eBOSS1_1STATE _iState = eBOSS1_1STATE.STROLL;

    // stage Manager ����
    Boss1_StageManager _stageManager;

    // boss 1_1 ������ Ŭ���� ����
    public Boss1_1Move _move;
    public float _originSpeed;


    // shake ȿ��
    bool _isDoingShake = false;
    public float _checkShakeTime;
    public float _shakeTime;

    // STEP  ����
    public float _checkBeforStepTime;
    public float _BeforeStepTime;
    public float _downPoint;
    public Vector2 _stepPoint;
    public bool _isDoingStep;
    public float _rate;
    public float _stepSpeed;
    public bool _rateCheck = true;

    // STROLL ����
    public float _checkStrollTime;
    public float _strollTime;

    // STOP ����
    public GameObject _shadowPrefab;
    GameObject _shadow;
    public float _shadowDistX;
    public float _shadowDistY;

    // DIE ����
    public bool _isAllDestroy = false;

    // ����Ʈ Ŭ���� ����
    public Boss1_1Effect _effect;

    // ȿ����
    public AudioSource _sound;

    private void Awake()
    {
        // �ݶ��̴��� STEP �����϶��� on
        GetComponent<Collider2D>().enabled = false;
        _effect = GetComponent<Boss1_1Effect>();
        _move = GetComponent<Boss1_1Move>();
        _myRigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss1_StageManager>();

    }


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerStatus = PlayerStatus.Instance;

        // move Ŭ�������� ���� �� �ٽ� �����϶��� ���� origin speed ����
        _originSpeed = _move._moveSpeed;
    }


    void FixedUpdate()
    {
        // 1) ���� �˻�
        IsDead();

        // 2) ���� �˻�
        if (_isStun && _curState != eBOSS_STATE.DIE)
          _curState = eBOSS_STATE.STUN;


        // 3) �÷��̾� ���� �˻� -- �÷��̾� ���� �����϶� STROLL ���� ����
        if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE || _player == null)
            _iState = eBOSS1_1STATE.STROLL;
        


        // 4) boss Ŭ������ ����
        switch(_curState)
        {
            case eBOSS_STATE.IDLE:
                break;

            case eBOSS_STATE.STUN:
                if (!_isDoingStun)
                    StartCoroutine(Stun());
                break;

            case eBOSS_STATE.DIE:
                _iState = eBOSS1_1STATE.DIE;
                break;
        }


        // 5) boss1_1�� ������ ���� iSTATE
        switch (_iState)
        {
            case eBOSS1_1STATE.STROLL:
                Stroll();
                break;

            case eBOSS1_1STATE.STOP:
                STOP();
                break;

            case eBOSS1_1STATE.STEP:
                Step();
                break;

            case eBOSS1_1STATE.DIE:
                if(!_isAllDestroy)
                {
                    // �Ʒ� �Լ��� �� ���� ȣ���� ����
                    _isAllDestroy = true;

                    // ���������� ����
                    _spriteRenderer.color = new Color(150 / 255f, 150 / 255f, 150 / 255f);

                    // ��� �ڷ�ƾ ����
                    StopAllCoroutines();

                    // �ڷ�ƾ ������ ���� moveSpeed = 0 �� ���¶�� originSpeed�� ����
                    _move._moveSpeed = _originSpeed;

                    // �׸���, ����Ʈ ���� ����
                    AllClearEffect();

                    _stageManager._isDead1 = true;
                }
                break;
        }
    }


    // DIE �����϶� �׸���, ����Ʈ ����
    void AllClearEffect()
    {
        // �׸��� ����
        if (_shadow != null)
            Destroy(_shadow);

        // ����Ʈ Ŭ���� ���� --> ����Ʈ ����
        _effect.DestroyEffect_DIE();
    }



    // STROLL ����
    void Stroll()
    {
        // 1) ������ �׸��� ����
        if (_shadow != null)
            Destroy(_shadow);

        // 2) �ӵ� ����
        _move._moveSpeed = _originSpeed;

        // �÷��̾ ���� ���¶�� ��� stroll ���� ������ ����
        if (_playerStatus._curState != PlayerStatus.ePLAYER_STATE.DIE)
        
        {
            // 3) Stroll Ÿ�� üũ
            _checkStrollTime += Time.deltaTime;

            // 4) Stroll �ð��� �Ѿ�ٸ� Stop ���� ���º�ȭ
            if (_checkStrollTime > _strollTime)
            {
                _iState = eBOSS1_1STATE.STOP;
                _checkStrollTime = 0f;
            }
        }
    }


    // STOP ����
    void STOP()
    {
        // 1) boss1 ���� ��Ű��
        _move._moveSpeed = 0f;

        // 2) stop & Shake �ð� üŷ
        _checkBeforStepTime += Time.deltaTime;
        _checkShakeTime += Time.deltaTime;

        // 3) shake ������ �ʴٸ� shake �ڷ�ƾ ����
        if (!_isDoingShake)
        {
            StartCoroutine(Shake());
        }

        // 4) Stop �ð��� �Ѿ�ٸ� STEP ���� ���º�ȭ
        //     ����  ������ �ʱ�ȭ
        if (_checkBeforStepTime > _BeforeStepTime)
        {
            _iState = eBOSS1_1STATE.STEP;
            _checkShakeTime = 0f;
            _checkBeforStepTime = 0f;
            _isDoingShake = false;
        }
    }



    // STEP ����
    void Step()
    {
        // 1) _rateCheck = true ��� _rate�� ��ġ ���� ++ 
        //      lerp �̵��� ����
        if (_rateCheck)
            _rate += Time.deltaTime * _stepSpeed;

        // 2) _rateCheck = false ��� _rate ��ġ = 0���� �ʱ�ȭ
        else if (!_rateCheck)
            _rate = 0f;

        // 3) �׸��� ���� �� �ڷ�ƾ �� ���� ����. ȿ���� ���
        if (!_isDoingStep)
        {
            StartCoroutine(StepGround());
            _sound.PlayOneShot(_sound.clip);
        }

    }


    // �¿� ��鸲 + �׸��� ȿ��
    IEnumerator Shake()
    {
        // shake �ڷ�ƾ �� ���� ������ ����
        _isDoingShake = true;

        // 1) �׸��� ȿ�� ����� _shadow �� ����
        GameObject Shadow =
            Instantiate(_shadowPrefab,
            new Vector2(transform.position.x + _shadowDistX, transform.position.y + _shadowDistY)
            , transform.rotation);

        _shadow = Shadow;
            
        // 2) �׸��� ���� �� boss1_1 �� �׸��� ���� ��鸮�� ȿ��

        Vector2 origin = _myRigid.position;
        Vector2 shadowOrigin = _shadow.transform.position;
        
        Vector2 random = new Vector2(0f, 0f);
        Vector2 proRandom = new Vector2(0f, 0f);

  
        // 3) shakeTime �ð� ���� �׸��ڿ� �Բ� shake
          while (_checkShakeTime < _shakeTime)
           {
              // Shake ���� ����
              // ������ �������� �������ٸ� ����������
              if (proRandom.x < 0)
                 random = new Vector2(Random.Range(0f, 0.25f), 0f);

              // ������ ���������� �������ٸ� ��������
              else if (proRandom.x > 0)
                 random = new Vector2(Random.Range(-0.25f, 0f), 0f);

              // ������ �������� �ʾҴٸ� �¿� �� ����������
              else if (proRandom.x == 0)
                 random = new Vector2(Random.Range(-0.25f, 0.25f), 0f);

              // Shake ���� ��ŭ �̵�
               _myRigid.position = Vector2.Lerp(origin, origin + random, 0.5f);
                if(_shadow!=null)
                    _shadow.transform.position = Vector2.Lerp(shadowOrigin, shadowOrigin + random, 0.5f); 

              // �̵��� ���� proRandom �� ����
              proRandom = random;

              // ���� �����ӱ��� ���
              yield return null;
        }//while (_checkShakeTime < _shakeTime)


        // 4) ������Ʈ ��ġ ���󺹱�
        _myRigid.position = origin;
        _shadow.transform.position = shadowOrigin;

    }// IEnumerator Shake()



    // boss1_1��  movePoint�� �ű�� �ڷ�ƾ
    IEnumerator MoveStep(Vector2 movePoint)
    {
        while(_myRigid.position.y != movePoint.y)
        {
            _myRigid.position = Vector2.Lerp(_myRigid.position, movePoint, _rate);
            yield return null;
        }
    }



    // �׸��� ��ġ�� �ϰ� -->  ���� ȿ�� --> ��� ������ �ڷ�ƾ 
    IEnumerator StepGround()
    {
        // ���� �ڷ�ƾ �� ����  ������ ����
        _isDoingStep = true;
        // boss1_1 ���� ��ġ
        _stepPoint = new Vector2(_myRigid.position.x, _myRigid.position.y + _downPoint);
        Vector2 origin = _myRigid.position;
        _rateCheck = true;


        // 1) �Ʒ��� �ϰ�
        StartCoroutine(MoveStep(_stepPoint));

        // 2) _stepPoint.y == _position.y �� �ȴٸ� �ڷ�ƾ ��� ����
        yield return new WaitUntil(()=>_stepPoint.y == _myRigid.position.y);
        StopCoroutine(MoveStep(_stepPoint));

        // 2.5) boss1_1 �ϰ� ���� �� �ݶ��̴� on
        GetComponent<Collider2D>().enabled = true;

        // 3) _rate �ʱ�ȭ �� üũ ����
        _rateCheck = false;

        // 4) ���� ���� ���� ȿ�� on 
        _effect.StartEffect();

        // 5) ���
        yield return new WaitForSeconds(3f);

        // 6) _rate üũ ���� �� ����Ʈ ���� �� ����ġ�� ���ư���
        _rateCheck = true;
        StartCoroutine(_effect.DestroyEffect(_effect._effectList));

        // 7) ����ġ�� ��� �ڷ�ƾ
        StartCoroutine(MoveStep(origin));

        // 8) _myRigid.position == origin �� �ȴٸ� �ڷ�ƾ ��� ����
        yield return new WaitUntil(() => _myRigid.position.y == origin.y);

        // 9) ���� ���°� �ƴ϶�� STROLL �� ȸ��
        if(_curState!= eBOSS_STATE.DIE)
            _iState = eBOSS1_1STATE.STROLL;

        // 10) ���� �ڷ�ƾ ���� _rateCheck, _isDoingStep false
        _rateCheck = false;
        _isDoingStep = false;
        GetComponent<Collider2D>().enabled = false;
    }


}


