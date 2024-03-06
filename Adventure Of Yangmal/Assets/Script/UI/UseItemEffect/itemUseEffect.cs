using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemUseEffect : MonoBehaviour
{
    GameObject _player;
    public GameObject _starImage;
    public Text _text;
    public string _effectText;

    public bool _isDoingTimeCheck = false;
    public float _time;


    void Start()
    {
        _player = GameObject.FindWithTag("Player");

        // 텍스트 / 이미지 끄기
        _starImage.SetActive(false);
        _text.gameObject.SetActive(false);

    }

    // 플레이어 위치 따라서 이동 & 위로 떠오르며 페이드인
    void Update()
    {
        Vector3 position = new Vector3(_player.transform.position.x, _player.transform.position.y + 3.5f, _player.transform.position.z);
        transform.position =
            Camera.main.WorldToScreenPoint(position);

        if(_isDoingTimeCheck)
        {
            if (_time >= 3f)
                CloseEffect();

            _time += Time.deltaTime;
        }



    }

    public IEnumerator FadeIn()
    {
        float a = 0;

        while(a<=1)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, a);
            a += 0.05f;

            yield return null;
        }
    }

    public IEnumerator MoveToUp()
    {
        float y = 0f;
        
        while(y <= 3.5f)
        {
            Vector3 position = new Vector3(_player.transform.position.x, _player.transform.position.y + y, _player.transform.position.z);
            transform.position = Camera.main.WorldToScreenPoint(position);
            y += 0.08f;

            yield return null;
        }
    }

    void CloseEffect()
    {
        _isDoingTimeCheck = false;
        _time = 0f;

        _starImage.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
    }

    public void StartEffect()
    {
        // 시간 초기화
        _time = 0f;
        // 텍스트 세팅
        _text.text = _effectText;

        // 별 이미지 open
        _starImage.gameObject.SetActive(true);

        // 텍스트 open
        _text.gameObject.SetActive(true);

        // 텍스트 페이드인&상승 효과 시작
        StartCoroutine(FadeIn());
        StartCoroutine(MoveToUp());

    }


}


