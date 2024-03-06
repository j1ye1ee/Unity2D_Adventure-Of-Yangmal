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

        // �ؽ�Ʈ / �̹��� ����
        _starImage.SetActive(false);
        _text.gameObject.SetActive(false);

    }

    // �÷��̾� ��ġ ���� �̵� & ���� �������� ���̵���
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
        // �ð� �ʱ�ȭ
        _time = 0f;
        // �ؽ�Ʈ ����
        _text.text = _effectText;

        // �� �̹��� open
        _starImage.gameObject.SetActive(true);

        // �ؽ�Ʈ open
        _text.gameObject.SetActive(true);

        // �ؽ�Ʈ ���̵���&��� ȿ�� ����
        StartCoroutine(FadeIn());
        StartCoroutine(MoveToUp());

    }


}


