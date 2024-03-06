using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Attack : MonoBehaviour
{
    // Boss2 �������� 5����
   
    public enum ATTACK_PATTERN
    {
        PATTERN1 = 1,
        PATTERN2,
        PATTERN3,
        PATTERN4,
        PATTERN5,
    }

    public ATTACK_PATTERN _attack;

    Boss2BulletPoolManager _poolManager;

    Boss2_StageManager _stageManager;

    public Boss2Move  _move;

    // ������ ��ȭ�ϴ� �ð�
    public float _patternChangeTerm;
    public float _patternChangeTermCheck;

    // Boss2Ball ���� ���� & �Ѿ˼ӵ�
    public float _shootTerm;
    public float _shootTermCheck;
    public float _bulletSpeed;


    // ���� �Լ� 1���� ȣ���� ����
    bool _isDoingAttack = false;
    bool _isPage2Start = false;

    // ȿ����
    public AudioSource _sound;


    void Start()
    {
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();
        _move = GetComponent<Boss2Move>();
        _attack = RandomPatternSet();
        _poolManager = GameObject.FindWithTag("boss2BulletPool").GetComponent<Boss2BulletPoolManager>();
    }


    void FixedUpdate()
    {
        _patternChangeTermCheck += Time.deltaTime;
        _shootTermCheck += Time.deltaTime;

        if (_stageManager._curState == Boss2_StageManager.eSTAGE_STATE.PAGE1CLEAR)
            StopAllCoroutines();

        else if (_stageManager._curState == Boss2_StageManager.eSTAGE_STATE.PAGE2)
            if (!_isPage2Start)
            {
                _isPage2Start = true;
                StartCoroutine(Pattern1());
            }


            switch (_attack)
            {
                case ATTACK_PATTERN.PATTERN1:
                    if (!_isDoingAttack)
                        StartCoroutine(Pattern1());
                    break;

                case ATTACK_PATTERN.PATTERN2:
                    if (!_isDoingAttack)
                        StartCoroutine(Pattern2());
                    break;

                case ATTACK_PATTERN.PATTERN3:
                    if (!_isDoingAttack)
                        StartCoroutine(Pattern3());
                    break;

                case ATTACK_PATTERN.PATTERN4:
                    if (!_isDoingAttack)
                        StartCoroutine(Pattern4());
                    break;

                case ATTACK_PATTERN.PATTERN5:
                    if (!_isDoingAttack)
                    {
                        if (transform.position.x <= 1 && transform.position.x >= -1)
                            StartCoroutine(Pattern5());
                    }
                    break;
            }
        

    }

    // ������ ������ ����
    ATTACK_PATTERN RandomPatternSet()
    {
        int random = Random.Range(1, 6);
        ATTACK_PATTERN attack = (ATTACK_PATTERN)random;
        return attack;
    }


    // �������� 1 : �Ʒ� �������� �������� ���
    IEnumerator Pattern1()
    {
        _sound.PlayOneShot(_sound.clip);

        _isDoingAttack = true;
        _shootTerm = 0.7f;

        // ���� ����ñ��� �ݺ�
        while (_patternChangeTermCheck < _patternChangeTerm)
        {
            // �߻� ���ݸ�ŭ�� �ð��� ������
            if (_shootTermCheck >= _shootTerm)
            {
                // 1) �Ѿ� ���� & �߻�
                GameObject bullet = _poolManager.GetObject(
                    _poolManager._boss2BulletPool,
                    _poolManager._boss2Bullet,
                    transform.position,
                    transform.rotation);

                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(Vector2.down * _bulletSpeed, ForceMode2D.Impulse);

                _shootTermCheck = 0f;
            }
            yield return null;
        }

        // while �� Ż�� �� ����üũ�ð� �ʱ�ȭ & ������
        _patternChangeTermCheck = 0f;
        _isDoingAttack = false;
        _attack = RandomPatternSet();
    }




    // �������� 2 : ���ɿ� 
    IEnumerator Pattern2()
    {
        _sound.PlayOneShot(_sound.clip);
        _isDoingAttack = true;
        _shootTerm = 1f;
        Vector2 direction = Vector2.up;

            // ���� ����ñ��� �ݺ�
        while (_patternChangeTermCheck < _patternChangeTerm)
        {
            // �߻� ���ݸ�ŭ�� �ð��� ������
            if (_shootTermCheck >= _shootTerm)
            {
                for (int count = 0; count<8; count++)
                {
                    GameObject bullet = _poolManager.GetObject(
                        _poolManager._boss2BulletPool,
                        _poolManager._boss2Bullet,
                        transform.position,
                        transform.rotation);

                    Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

                    if (count != 0)
                        direction = Quaternion.Euler(0, 0, -45) * direction.normalized;
                   
                    bulletRigid.AddForce(direction* _bulletSpeed, ForceMode2D.Impulse);

                }
                _shootTermCheck = 0;
            }//if (_shootTermCheck >= _shootTerm)

            yield return null;
        }//while (_patterChangeTermCheck < _patternChangeTerm)

        // while �� Ż�� �� ����üũ�ð� �ʱ�ȭ & ������
        _patternChangeTermCheck = 0f;
        _isDoingAttack = false;
        _attack = RandomPatternSet();
    }





    // �������� 3 : ���� �׸��� �߻�
    IEnumerator Pattern3()
    {
        _sound.PlayOneShot(_sound.clip);

        _isDoingAttack = true;
        _shootTerm = 0.1f;
        Vector2 direction = Vector2.up;

        // ���� ����ñ��� �ݺ�
        while (_patternChangeTermCheck < _patternChangeTerm)
        {
            // �߻� ���ݸ�ŭ�� �ð��� ������
            if (_shootTermCheck >= _shootTerm)
            {
                GameObject bullet = _poolManager.GetObject(
                    _poolManager._boss2BulletPool,
                    _poolManager._boss2Bullet,
                    transform.position,
                    transform.rotation);

                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(direction * _bulletSpeed, ForceMode2D.Impulse);

                direction = Quaternion.Euler(0, 0, -45) * direction.normalized;


                _shootTermCheck = 0;
            }//if (_shootTermCheck >= _shootTerm)

            yield return null;
        }//while (_patterChangeTermCheck < _patternChangeTerm)

        // while �� Ż�� �� ����üũ�ð� �ʱ�ȭ & ������
        _patternChangeTermCheck = 0f;
        _isDoingAttack = false;
        _attack = RandomPatternSet();
    }




    // �������� 4 : ���� �߻�
    IEnumerator Pattern4()
    {
        _sound.PlayOneShot(_sound.clip);

        _isDoingAttack = true;
        _shootTerm = 0.7f;

        // ���� ����ñ��� �ݺ�
        while (_patternChangeTermCheck < _patternChangeTerm)
        {
            // �߻� ���ݸ�ŭ�� �ð��� ������
            if (_shootTermCheck >= _shootTerm)
            {
                // 1) �Ѿ� ���� & �߻�
                GameObject bullet = _poolManager.GetObject(
                    _poolManager._boss2Bullet4Pool,
                    _poolManager._boss2Bullet4,
                    transform.position,
                    transform.rotation);

                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(Vector2.down * _bulletSpeed, ForceMode2D.Impulse);


                _shootTermCheck = 0f;
            }
            yield return null;
        }

        // while �� Ż�� �� ����üũ�ð� �ʱ�ȭ & ������
        _patternChangeTermCheck = 0f;
        _isDoingAttack = false;
        _attack = RandomPatternSet();
    }




    // �������� 5 : ���� �� ��ä��
    IEnumerator Pattern5()
    {
        _sound.PlayOneShot(_sound.clip);

        _isDoingAttack = true;
        _shootTerm = 0.3f;

        // ����
        _move._moveSpeed = 0;

        // �߻� ��ġ ����
        Vector2 position1 = new Vector2(transform.position.x - 1f, transform.position.y);
        Vector2 position2 = new Vector2(transform.position.x + 1f, transform.position.y);

        // �߻簢 ����
        Vector2 direction1 = Vector2.down; // ����
        Vector2 direction2 = Vector2.down; // ������

        // �Ʒ��� �߻�
        bool isDown = false;

        // ���� ����ñ��� �ݺ�
        while (_patternChangeTermCheck < _patternChangeTerm)
        {
            // �߻� ���ݸ�ŭ�� �ð��� ������
            if (_shootTermCheck >= _shootTerm)
            {
                if (direction1.y >= 0 && isDown == false)
                {
                    isDown = true;
                    direction1 = Vector2.left;
                    direction2 = Vector2.right;
                }


                else if (direction1.x >= 0 && isDown == true)
                {
                    isDown = false;
                    direction1 = Vector2.down;
                    direction2 = Vector2.down;
                }


                // 1) position1 �Ѿ� ���� & �߻�
                GameObject bullet1 = _poolManager.GetObject(
                    _poolManager._boss2BulletPool,
                    _poolManager._boss2Bullet,
                    transform.position,
                    transform.rotation);

                Rigidbody2D bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid1.AddForce(direction1 * _bulletSpeed, ForceMode2D.Impulse);

                if(isDown)
                    direction1 = Quaternion.Euler(0, 0, 15) * direction1;
                
                else
                    direction1 = Quaternion.Euler(0, 0, -15) * direction1;

                // 2) position2 �Ѿ� ���� & �߻�
                GameObject bullet2 = _poolManager.GetObject(
                    _poolManager._boss2BulletPool,
                    _poolManager._boss2Bullet,
                    transform.position,
                    transform.rotation);

                Rigidbody2D bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
                bulletRigid2.AddForce(direction2 * _bulletSpeed, ForceMode2D.Impulse);

                if (isDown)
                    direction2 = Quaternion.Euler(0, 0, -15) * direction2;
                else
                    direction2 = Quaternion.Euler(0, 0, 15) * direction2;



                _shootTermCheck = 0f;
            }
            yield return null;
        }

        // while �� Ż�� �� ����üũ�ð� �ʱ�ȭ & ���ǵ� �ʱ�ȭ & ������
        _patternChangeTermCheck = 0f;
        _move._moveSpeed = _move._originSpeed;
        _isDoingAttack = false;
        _attack = RandomPatternSet();

    }



}
