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
        // 원활한 발사를 위해 콜라이더 끄기
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

    // 깜박이는 효과
    IEnumerator Blink()
    {
        float time = 1f;

        while (_blinkTime >= 0)
        {
            time = time / 2f;
            //  time 시간만큼 대기 후 투명도 낮추기 & 원복
            _sprite.color = _blinkColor;
            yield return new WaitForSeconds(time);
            _sprite.color = _originColor;
            yield return new WaitForSeconds(time);

        }

        _sprite.color = _destroyColor;
    }

    // 이펙트 페이드아웃
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

    // 폭탄 이펙트 효과
    public IEnumerator BombEffect()
    {

        yield return new WaitForSeconds(0.05f);
        _collider.enabled = true;

        yield return new WaitForSeconds(1f);

        // 정지
        _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;


        // 1) 폭탄 깜박이는 효과 5초 지속
        StartCoroutine(Blink());

        // 2) 대기
        yield return new WaitUntil(() => _blinkTime <= 0);
        yield return null;

        // 콜라이더 끄기
        _collider.enabled = false;

        // 3) 이펙트 생성
        GameObject bombEffect = Instantiate(_bombEffectPrefab, transform.position, transform.rotation); //이펙트생성


        // 4) 페이드아웃 후 콜라이더 끄기
        yield return new WaitForSeconds(1f);
        StartCoroutine(EffectFadeOut(bombEffect));

        yield return new WaitUntil(() => bombEffect.GetComponent<SpriteRenderer>().color.a <= 0);
        bombEffect.GetComponent<Collider2D>().enabled = false;

        // 6) 관련 오브젝트 삭제 & 설정 초기화
        Destroy(bombEffect);
        _sprite.color = _originColor;
        _blinkTime = 5f;
        _myRigid.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        // 월드 벽에 충돌시 정지 (벽 뚫고 나가기 방지)
        if(other.gameObject.tag == "World" || other.gameObject.tag == "wall")
            _myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // bomb SetActive 시 초기화
    public void BombSetActiveFasle()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerShooter>().StopAllCoroutines();
        _sprite.color = _originColor;
        _blinkTime = 5f;
        _myRigid.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }
}
