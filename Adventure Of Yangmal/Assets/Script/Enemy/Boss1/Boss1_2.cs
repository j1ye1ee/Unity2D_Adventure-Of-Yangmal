using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_2 : Boss
{
    // Boss1_2 �� �ൿ&���� ���� Ŭ����


    // StageManager ����
    Boss1_StageManager _stageManager;

    // �׸��� ������ �տ� ���� �ִ��� Ȯ��
    public enum eISWALL
    {
        RIGHTWALL,
        LEFTWALL,
        NOWALL
    }

    // ������ �ൿ ���� 4����
    public enum eBOSS1_2STATE
    {
        FOLLOW,
        STOP,
        STEP,
        DIE
    }

    public eBOSS1_2STATE _iState = eBOSS1_2STATE.FOLLOW;
    public eISWALL _wall = eISWALL.NOWALL;
    public Boss1_1 _boss1_1;

    // ������
    public float _distance; // boss1_1 ���� �Ÿ����� �α� ����
    public float _rayDistance;
    public float _followRate;

    // �� ����
    public bool _isRightWall;
    public bool _isLeftWall;


    // �׸��� ����
    public List<GameObject> _shadows = new List<GameObject>();


    Vector2 _dist;
    public float _downPoint;
    public int _shadowCountMin;
    public int _shadowCountMax;
    public float _xDistMin;
    public float _xDistMax;
    public float _yDistMin;
    public float _yDistMax;


    // �׸��� ���� �� �������� ����
    public List<GameObject> _parabolaSpots = new List<GameObject>();
    public float _upPoint;



    // �׸��� ���� -> �������� ���� -> ���� ��� ������
    public int _stepCount;
    public bool _isDoingBeforeStep = false;
    public bool _isDoingStep = false;
    public float _moveSpeed;
    public float _rate;

    // Ǯ�� ���� ����
    Boss1_2EffectPoolManager _poolManager;

    // ����Ʈ ���� Ŭ���� ����
    public Boss1_2Effect _boss1_2Effect;

    // boss1_1  ����� ������ �������� ���� ����
    public float _stopTime;
    public float _checkStopTime;

    // ����� ����Ʈ ������ ����
    public bool _isAllDestroy = false;

    // ȿ����
    public AudioSource _sound;


    void Start()
    {
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss1_StageManager>();

        // �߹ٴ� ��� ���� collider on
        GetComponent<Collider2D>().enabled = false;

        _boss1_1 = FindObjectOfType<Boss1_1>();
        _myRigid = gameObject.GetComponent<Rigidbody2D>();
        _boss1_2Effect = GetComponent<Boss1_2Effect>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
        _playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        _poolManager = GameObject.FindWithTag("boss1_2EffectPool").GetComponent<Boss1_2EffectPoolManager>();

    }


    // boss1_1 �� ���¿� ���� �ൿ�� �޶����Ƿ� LateUpdate
    void LateUpdate()
    {
        // 1) ���� �˻�
        IsDead();

        // 2) ���� �˻�
        if (_isStun && _curState != eBOSS_STATE.DIE)
            _curState = eBOSS_STATE.STUN;


        // 3) boss Ŭ������ ����
        switch (_curState)
        {
            case eBOSS_STATE.IDLE:
                break;

            case eBOSS_STATE.STUN:
                if (!_isDoingStun)
                    StartCoroutine(Stun());
                break;

            case eBOSS_STATE.DIE:
                _iState = eBOSS1_2STATE.DIE;
                break;
        }


        // 4) �׸��ڻ����̳� ���� ��⸦ ���� �ʰ� ���� ��쿡�� iSTATE ���º�ȭ�� ��
        // boss1_1�� �����ϸ� iSTATE == STOP
        if (!_isDoingStep && !_isDoingBeforeStep)
        {
            // boss 1_1 ��������� --> boss1�� �������� ����
            if (_boss1_1._iState != Boss1_1.eBOSS1_1STATE.DIE)
            {
                if (_boss1_1._iState == Boss1_1.eBOSS1_1STATE.STOP)
                    _iState = eBOSS1_2STATE.STOP;
            }

        }

        // 5) boss 1_1 �׾������� &  follow �����϶� --> ���������� �����̱�
        if (_boss1_1._iState == Boss1_1.eBOSS1_1STATE.DIE 
            && _iState == eBOSS1_2STATE.FOLLOW)
        {
            // ���������� ���º�ȭ �ð� üũ
            _checkStopTime += Time.deltaTime;

            // �������� ���� �ð��� �Ǿ��ٸ� iSTATE --> STOP
            if (_checkStopTime >= _stopTime)
            {
                _iState = eBOSS1_2STATE.STOP;
                _checkStopTime = 0;
            }

        }


        //// 5.5) �÷��̾� ���� �˻� -- �÷��̾� ���� �����϶� FOLLOW ���� ����
        //if (_playerStatus._curState == PlayerStatus.ePLAYER_STATE.DIE || _player == null)
        //{
        //    _iState = eBOSS1_2STATE.FOLLOW;
        //}




            // 6) boss1_2 Ŭ������ ������ ���� iSTATE
            switch (_iState)
        {
            case eBOSS1_2STATE.FOLLOW:
                Boss1_2Follow(_boss1_1._myRigid.position); // boss1_1���� �̵�
                break;

            case eBOSS1_2STATE.STOP:
                _followRate = 0f;
                Boss1_2Stop();  //���� ���� �׸��� ����
                break;

            case eBOSS1_2STATE.STEP:
                Boss1_2Step(); // ������ �׸��� ���� ������ �̵�
                break;

            case eBOSS1_2STATE.DIE:
                if(!_isAllDestroy) // ������ ����Ʈ ����
                { 
                    // ���������� ����
                    _spriteRenderer.color = new Color(150 / 255f, 150 / 255f, 150 / 255f);

                    // ��� �ڷ�ƾ ����
                    StopAllCoroutines();

                    // �׸���, ����Ʈ ���� ����
                    AllClearEffect();

                    _stageManager._isDead2 = true;
                }

                Boss1_2Follow(_boss1_1._myRigid.position);
                break;

        }
    }






    // FOLLOW
    void Boss1_2Follow(Vector2 direction)
    {
        // Lerp�� �����̹Ƿ� followRate�� ���� ũ�� �����
        // --> _distance �̻��� �Ǹ� 0���� �ʱ�ȭ
        if (_followRate >= _distance)
            _followRate = 0f;

        _followRate = Mathf.Clamp(_followRate, 0f, _distance);
        _followRate += Time.deltaTime * 0.5f;

        // newDirection  = Lerp�� �̵��ϰ��� �ϴ� ������
        Vector2 newDirection;
        newDirection = new Vector2(direction.x, direction.y - 7f);
        _myRigid.position = Vector2.Lerp(_myRigid.position,newDirection , _followRate);
    }


    // STOP
    void Boss1_2Stop()
    {
        // �ڷ�ƾ �� ���������� ����
        if (!_isDoingBeforeStep)
            StartCoroutine(InstantiateShadows());
        
    }

    // STEP
    void Boss1_2Step()
    {
        // �ڿ������� lerp �̵��� ���� _rate �� ũ�⸦ ���������� �÷��ش�.
        _rate += Time.deltaTime * _moveSpeed;

        // �ڷ�ƾ �� ���� ������ ����
        if (!_isDoingStep)
            StartCoroutine(Step());
            


        
    }



    // ������ ����Ʈ & �׸��� ����
    void AllClearEffect()
    {
        // ����Ʈ Ŭ���� ���� --> ����
        _boss1_2Effect.AllDestroy();

        // �׸��� ����Ʈ ���� --> ����

        for (int listNum = 0; listNum < _shadows.Count; listNum++)
            _shadows[listNum].gameObject.SetActive(false);

        _shadows.Clear();

    }




    //  �׸��� ������ �� ���� --> eISWALL ���¸� ����
    void IsWall(Vector2 objectPosition)
    {
        // 1) ������Ʈ�� ������ ��ġ���� ������ �� �浹 ���� �˻�
        RaycastHit2D hitDataRight;
        hitDataRight = Physics2D.Raycast
            (objectPosition, Vector2.right, _rayDistance, LayerMask.GetMask("World"));

        // 2) ������ ���� �浹 ������ --> RIGHTWALL ����
        if (hitDataRight.collider != null)
        {
            _isRightWall = true;
            _wall = eISWALL.RIGHTWALL;
        }

        // 3) ������ �� �浹 �������� �ʾ��� ��
        else 
        {
            _isRightWall = false;

            // 4) ���� �˻�
            RaycastHit2D hitDataLeft;
            hitDataLeft = Physics2D.Raycast
                (objectPosition, Vector2.left, _rayDistance, LayerMask.GetMask("World"));

            //5) ���� �� �浹 ������ --> LEFTWALL ����
            if (hitDataLeft.collider != null)
            {
                _isLeftWall = true;
                _wall = eISWALL.LEFTWALL;
            }

            //6) ���� �� �浹 �������� �ʾ��� ��
            else
                _isLeftWall = false;
        }

        // 7) ������, ���� �˻� �� �Ѵ� ���� ���� �� --> NOWALL ����
        if (!_isRightWall && !_isLeftWall)
        {
            _wall = eISWALL.NOWALL;
        }
    }




    // �׸��� ���� ��ġ / �� ����
    Vector2 Position(Vector2 origin, Vector2 dist)
    {
        // �� ��ġ + Dist ��ŭ�� ����
        // _downPoint��ŭ y��ǥ �ϰ�

        Vector2 position = origin + dist;
        return position;
    }




    // eISWALL == RIGHTWALL / eISWALL == LEFTWALL �� ���
    // Dist(���� �׸��� ������ ���� ���� �׸��� ��ġ�� ���ϴ� Vecto2 ��)�� ���� ��
    Vector2 XYDist()
    {
        Vector2 Dist;
        float xDist = 0f;
        float yDist = 0f;

        // 1) xDist ����
            // �����ʿ� ���� ������
        if (_wall == eISWALL.RIGHTWALL)
        {
            // xDist = ���� ���� ���� �� ������ ����
            xDist = -1f * Random.Range(_xDistMin, _xDistMax);
        }

            // ���ʿ� ���� ������
        else if (_wall == eISWALL.LEFTWALL)
        {
            // xDist = ���� ���� ���� �� ����� ����
            xDist = Random.Range(_xDistMin, _xDistMax);
        }


        // 2) yDist ����
        yDist = Random.Range(_yDistMin, _yDistMax);

        // 3) �� ��ȯ
        Dist = new Vector2(xDist, yDist);
        return Dist;
    }




    // �׸��� ������ eISWALL == NOWALL �� ���
    // ����, ������ ���� ������ Dist �� ���� ��
    Vector2 RandomXYDist()
    {
        Vector2 Dist;
        float xDist = 0f;
        float yDist = 0f;

        
        // xDist = 50%�� Ȯ���� ���� or ���
          int isNegative = Random.Range(1, 101);

        if (isNegative > 50f)
            xDist = -1f * Random.Range(_xDistMin, _xDistMax);
       
        else
            xDist = Random.Range(_xDistMin, _xDistMax);

        Dist = new Vector2(xDist, yDist);
        return Dist;
    }




    // �׸��� ���� �ڷ�ƾ
    IEnumerator InstantiateShadows()
    {
        // �ڷ�ƾ �� ���� ������ ����
        _isDoingBeforeStep = true;

        // 1) �׸��� ���� ���� ����
        _stepCount = Random.Range(_shadowCountMin, _shadowCountMax + 1);
       

        // 2) ���� �������� ������ �׸��� ���� ī��Ʈ �ʱ�ȭ
        int sameDirCount = 0;

        // 3) ������ �׸����� ������ ������ ���� ����
        bool isRight = false;

        // 4) boss1_2�� ��ġ���� �� �˻� 
        IsWall(transform.position);

        // 5) �� �˻翡 ���� _dist �� ����

            // �������̳� ���� ���� ���� ��� XYDist()
        if (_wall == eISWALL.RIGHTWALL || _wall == eISWALL.LEFTWALL)
            _dist = XYDist();

            // ���� ���� ��� RandomXYDist()
        else if(_wall == eISWALL.NOWALL)
            _dist = RandomXYDist();

        // 6) ù��° �׸��� pool ���� �뿩
        // Position �޼��� Ȱ���Ͽ� �׸��� ������ ��ġ Vector2 ����(���� ��ġ + dist)  
        GameObject firstShadow = _poolManager.GetObject(
            _poolManager._shadowPool,
            _poolManager._shadow,
            Position(new Vector2(transform.position.x, transform.position.y + _downPoint), _dist),
            transform.rotation);

            //  _shadows ����Ʈ�� �׸��� ����
        _shadows.Add(firstShadow);


        // 7) ������ �׸��� ������� ����
        if (_dist.x > 0)
                isRight = true;

        else if (_dist.x < 0)
                isRight = false;

        // 8) ù��° �׸��� ���� �� 0.5�� ��� 
        yield return new WaitForSeconds(0.5f);


        // 9) �׸��� ���� -1 ��ŭ �ݺ��Ͽ� ������ ����(ù° �׸��� ���������Ƿ�)
        for (int num = 0; num <  _stepCount-1; num ++)
        {
            // ���� ������ �׸����� iswall �˻�
            IsWall(_shadows[num].transform.position);


            // 1.  ���� ������ �׸����� iswall ����� NOWALL �� ���
            //          _shadowCountMin ��ŭ ���� �������� �׸��� ����
            if(_wall == eISWALL.NOWALL && sameDirCount < _shadowCountMin)
            {
                // ù��° ������ �׸��ڰ� ���������� �����Ǿ��ٸ� ���������� ���� ����
                if (isRight)
                    _wall = eISWALL.LEFTWALL;

                // ���� ������ �׸��ڰ� �������� �����Ǿ��ٸ� ���������� ���� ����
                else if (!isRight)
                    _wall = eISWALL.RIGHTWALL;

                // ����� ���� ������ ��ġ�� �׸��� pool ���� ����
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, XYDist()),
                    transform.rotation);

                // ����Ʈ�� �߰� �� NOWALL�� ���� �ʱ�ȭ
                _wall = eISWALL.NOWALL;
                _shadows.Add(shadow);
                sameDirCount++;
            }



            // 2. ���� ������ �׸����� iswall ����� NOWALL�� �ƴ� ��� 
            else if(_wall != eISWALL.NOWALL)
            {
                // eISWALL ���¿� ���� ������ ��ġ�� �׸��� pool���� ����
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, XYDist()),
                    transform.rotation);

                // _dist �� ���� isRight(�׸��� ���� ����) ����
                _dist = XYDist();
                if (_dist.x > 0)
                    isRight = true;

                else if (_dist.x < 0)
                    isRight = false;


                // sameDirCount �ʱ�ȭ
                sameDirCount = 0;

                // ����Ʈ�� �߰�
                _shadows.Add(shadow);
            }



            // 3.  ���� ������ �׸��� ������ ��� �̷���� & NOWALL �� �ƴ� ���
            else if(_wall == eISWALL.NOWALL && sameDirCount >= _shadowCountMin)
            {
                // ����, ������ �������� ������ ��ġ�� �׸��� pool ���� ����
                GameObject shadow = _poolManager.GetObject(
                    _poolManager._shadowPool,
                    _poolManager._shadow,
                    Position(_shadows[num].transform.position, RandomXYDist()),
                    transform.rotation);

                // sameDirCount �ʱ�ȭ
                sameDirCount = 0;
                // ����Ʈ�� �߰�
                _shadows.Add(shadow);
            }

            yield return new WaitForSeconds(0.5f);
        }// for (int num = 0; num <  count-1; num ++)


        // 10) �׸��� ���� �Ϸ� �� ������ �̵��� ���� �� ����
        MakeParabolaSpots();

        // 11) �׸��ڿ� �� ���� �� step ���� �Ѿ���� ���� ����
        _iState = eBOSS1_2STATE.STEP;

    }




    // ������ �̵��� ���� �� ����
    void MakeParabolaSpots()
    {
        
        Vector2 makePoint;
        float x;
        float y;

        // 1) ������ �׸����� ������ŭ �ݺ��Ͽ� �� ����
        for (int count = 0; count < _shadows.Count; count ++)
        {
             // ù��° makePoint �� boss1_2 �� ù ��° �׸��� ���̿� ����
            if (count == 0)
            {
                if (_myRigid.transform.position.y >= _shadows[count].transform.position.y)
                    y = _myRigid.transform.position.y + _upPoint;

                else
                    y = _shadows[count].transform.position.y + _upPoint;

                // x �� ���� = �� x ���� ���
                x = (_myRigid.transform.position.x + _shadows[count].transform.position.x) * 0.5f;
            }


            // �ι�° makePoint ���ʹ� �׸��ڿ� �׸��� ���̿� �� ����
            else
            {
                // n ��° �׸����� y ���� n+1 ��° �׸����� y���� ��
                // n ��° �׸����� y ���� �� ũ�ٸ�
                if (_shadows[count-1].transform.position.y >= _shadows[count].transform.position.y)
                    y = _shadows[count].transform.position.y + _upPoint;

                // n ��° �׸����� y ���� �� �۴ٸ�
                else
                    y = _shadows[count -1].transform.position.y + _upPoint;
                // x �� ���� = �� x ���� ���
                x = (_shadows[count-1].transform.position.x + _shadows[count].transform.position.x) * 0.5f;
            }

           
            // n ��° makePoint ���� & ���� & ����Ʈ�� �߰�
            makePoint = new Vector2(x, y);
            GameObject spot = _poolManager.GetObject(
                _poolManager._parabolaSpotPool,
                _poolManager._parabolaSpot,
                makePoint,
                transform.rotation);

            _parabolaSpots.Add(spot);
        }

    }





    // �׸��ڿ� �� ���� ���� ���������� ��� �ڷ�ƾ
    IEnumerator Step()
    {
        _rate = 0f;
        _isDoingStep = true;
        // step �����϶��� �ݶ��̴� on
        GetComponent<Collider2D>().enabled = true;
        Vector2 origin = _myRigid.position;



        // 1) boss1_2 ��ġ ~ shadow[0] ���� �̵���Ű��
        while (_myRigid.transform.position != _shadows[0].transform.position)
         {
            Vector2 point1 =
                        Vector2.Lerp(_myRigid.transform.position,
                           _parabolaSpots[0].transform.position, _rate);
            Vector2 point2 =
                        Vector2.Lerp(_parabolaSpots[0].transform.position,
                                        _shadows[0].transform.position, _rate);
            Vector2 position =
                        Vector2.Lerp(point1, point2, _rate);

            _myRigid.position = position;

            yield return null;
         }

        _rate = 0;
                // �̵� �Ϸ� �� ũ�� ����Ʈ ����
        StartCoroutine(_boss1_2Effect.CrackEffect());


        // 2) s[count] ~ s[count+1] ���� �̵� �ݺ�
        for(int count = 1; count<=_parabolaSpots.Count-1; count++)
        {
            while(_myRigid.transform.position != _shadows[count].transform.position)
            {
                Vector2 point1 =
                    Vector2.Lerp(_myRigid.transform.position,
                                        _parabolaSpots[count].transform.position, _rate);
                Vector2 point2 =
                        Vector2.Lerp(_parabolaSpots[count].transform.position,
                                        _shadows[count].transform.position, _rate);
                Vector2 position =
                        Vector2.Lerp(point1, point2, _rate);

                _myRigid.position = position;

                yield return null;
            }

                // ���� s �� �����ϸ� rate �ʱ�ȭ , count++
            if (_rate > 1f)
            {
                _rate = 0;
                // ũ�� ����Ʈ ����
                StartCoroutine(_boss1_2Effect.CrackEffect());
                _sound.PlayOneShot(_sound.clip);
            }
        }//for(int count = 1; count<=_parabolaSpots.Count; count++)



        // 3) �׸��� ���� ~���� ������ ��ƾ ��ģ �� �׸���, �������� ���ӿ�����Ʈ ����
        for (int listNum = 0; listNum < _stepCount; listNum++)
        {
            _shadows[listNum].gameObject.SetActive(false);
            _parabolaSpots[listNum].gameObject.SetActive(false);
        }

               // ����Ʈ ������Ʈ �ı�
        StartCoroutine(_boss1_2Effect.DestroyEffect(_boss1_2Effect._effectList));


        // 4) shadows , parabolaSpots ��� �ı��ɶ����� ��ٷȴٰ� ���� ����
        yield return new WaitUntil
            (() => !_shadows[_stepCount - 1].gameObject.activeSelf && ! _parabolaSpots[_stepCount - 1].gameObject.activeSelf);



        // 5) ���� ��ƾ�� ���� ���� �ʱ�ȭ & boss1_2 ���ڸ� �� ����Ʈ �ʱ�ȭ
        while (_myRigid.position != origin)
        {
            _myRigid.position = Vector2.Lerp(_myRigid.position, origin, _rate / _moveSpeed * 0.05f) ;
            yield return null;
        }

        _rate = 0f;
        _iState = eBOSS1_2STATE.FOLLOW;
        GetComponent<Collider2D>().enabled = false;
        _isDoingBeforeStep = false;
        _isDoingStep = false;
        _shadows.Clear();
        _parabolaSpots.Clear();
        _boss1_2Effect._allEffect.Clear();

    }


    
}
