using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    SpriteRenderer _sprite;
    float _blinkTime = 5f;
    public GameObject _bombEffectPrefab;
    Rigidbody2D _myRigid;
    Collider2D _collider;

    Color _destroyColor;
    Color _originColor;
    Color _blinkColor;



    private void Awake()
    {
        // ��Ȱ�� �߻縦 ���� �ݶ��̴� ����
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }


    void Start()
    {
        _myRigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _destroyColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0 / 255f);
        _originColor = _sprite.color;
        _blinkColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 100 / 255f);

    }

    // Update is called once per frame
    void Update()
    {
        _blinkTime -= Time.deltaTime;
    }

    // �����̴� ȿ��
    IEnumerator Blink()
    {
        float time = 1f;

        while (_blinkTime >= 0)
        {
            time = time / 2f;
            //  time �ð���ŭ ��� �� ���� ���߱� & ����
            _sprite.color = _blinkColor;
            yield return new WaitForSeconds(time);
            _sprite.color = _originColor;
            yield return new WaitForSeconds(time);

        }

        _sprite.color = _destroyColor;
    }

    // ����Ʈ ���̵�ƿ�
    IEnumerator EffectFadeOut(GameObject effect)
    {
        float a = 1f;
        SpriteRenderer sprite = effect.GetComponent<SpriteRenderer>();

        while( a >= 0)
        {
            a -= 0.05f;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // ��ź ����Ʈ ȿ��
    public IEnumerator BombEffect()
    {

        yield return new WaitForSeconds(0.05f);
        _collider.enabled = true;

        yield return new WaitForSeconds(1f);

        // ����
        _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;


        // 1) ��ź �����̴� ȿ�� 5�� ����
        StartCoroutine(Blink());

        // 2) ���
        yield return new WaitUntil(() => _blinkTime <= 0);
        yield return null;

        // �ݶ��̴� ����
        _collider.enabled = false;

        // 3) ����Ʈ ����
        GameObject bombEffect = Instantiate(_bombEffectPrefab, transform.position, transform.rotation); //����Ʈ����


        // 4) ���̵�ƿ� �� �ݶ��̴� ����
        yield return new WaitForSeconds(1f);
        StartCoroutine(EffectFadeOut(bombEffect));

        yield return new WaitUntil(() => bombEffect.GetComponent<SpriteRenderer>().color.a <= 0);
        bombEffect.GetComponent<Collider2D>().enabled = false;

        // 6) ���� ������Ʈ ���� & ���� �ʱ�ȭ
        Destroy(bombEffect);
        _sprite.color = _originColor;
        _blinkTime = 5f;
        _myRigid.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        // ���� ���� �浹�� ���� (�� �հ� ������ ����)
        if(other.gameObject.tag == "World" || other.gameObject.tag == "wall")
            _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // bomb SetActive �� �ʱ�ȭ
    public void BombSetActiveFasle()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerShooter>().StopAllCoroutines();
        _sprite.color = _originColor;
        _blinkTime = 5f;
        _myRigid.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }
}
