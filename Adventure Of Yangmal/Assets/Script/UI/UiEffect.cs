using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEffect : MonoBehaviour
{
    public  Image _image;
    public RectTransform _transform;
    public Text _text;

    public bool _isPrintEnd = false;
    public bool _isSpreadEnd = false;
    public bool _isFadeInEnd = false;
    public bool _isFadeOutEnd = false;

    // 반투명 배경 페이드인
    public IEnumerator FadeIn()
    {
        _image = GetComponent<Image>();

        float a = 0;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);

        while( a <=0.7f )
        {
            a += 0.05f;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
            yield return null;
        }

        _isFadeInEnd = true;
    }

    public IEnumerator FadeInAll()
    {
        _image = GetComponent<Image>();

        float a = 1;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);

        while (a >= 0)
        {
            a -= 0.05f;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
            yield return null;
        }

        _isFadeInEnd = true;
    }



    // 스프레드
    public IEnumerator Spread()
    {
        _transform = GetComponent<RectTransform>();

        float originScale = _transform.localScale.x;
        float x = 0;

        _transform.localScale = new Vector3(0f, _transform.localScale.y, _transform.localScale.z);

        while( x < originScale )
        {
            x += 0.05f;
            _transform.localScale = new Vector3(x, _transform.localScale.y, _transform.localScale.z);
            yield return null;
        }

        _isSpreadEnd = true;
    }

    // 텍스트프린트
   public IEnumerator TextPrint()
    {
        _text = GetComponent<Text>();

        string text = _text.text.ToString();
        _text.text = " ";

        for(int i = 0; i<text.Length; i++)
        {
            _text.text += text[i].ToString();
            yield return new WaitForSeconds(0.1f);
        }

        _isPrintEnd = true;
    }

    // 투명 배경 --> 검정으로 진해지도록
    public IEnumerator FadeOut()
    {
        _image = GetComponent<Image>();

        float a = 0;
        while (a <= 1 )
        {
            a += 0.05f;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
            yield return null;
        }

        _isFadeOutEnd = true;
    }


}
