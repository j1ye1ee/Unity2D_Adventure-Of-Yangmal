using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpManager : MonoBehaviour
{
    public List<GameObject> _hearts= new List<GameObject>();
    enum eSET_STATE
    {
        UpMaxHeart,
        DownMaxHeart,
        KeepMaxHeart
    }

    eSET_STATE eHeartSetState = eSET_STATE.KeepMaxHeart;

    PlayerStatus _player;
    GameObject _curHeart;
    public int _hpCount { get; private set; }

    // 하트1개의 hp = 20
    float _maxHeart = 20;


    private void Start()
    {
        _player = PlayerStatus.Instance;
        _hpCount = 3;

        // 4번째 하트부터 setActive false
        for(int count = 0; count < _hearts.Count-3; count ++)
            _hearts[count + 3].gameObject.SetActive(false);

        // 3번째 fillHeart _curHeart로 설정
        _curHeart = _hearts[_hpCount -1].transform.GetChild(0).gameObject;
    }

    public void HeartSet()
    {
        float maxHp = _player._maxHp;
        float hp = _player._hp;
        int curCount = (int)System.Math.Truncate( maxHp / _maxHeart );

        if (curCount > _hpCount)
            eHeartSetState = eSET_STATE.UpMaxHeart;
        else if (curCount < _hpCount)
            eHeartSetState = eSET_STATE.DownMaxHeart;
        else
            eHeartSetState = eSET_STATE.KeepMaxHeart;

        switch(eHeartSetState)
        {
            case eSET_STATE.UpMaxHeart:
                UpMaxHeart();
                break;
            case eSET_STATE.DownMaxHeart:
                DownMaxHeart();
                break;
            case eSET_STATE.KeepMaxHeart:
                KeepMaxHeart();
                break;
        }
           


    }

    void SetfillAmount(int listNum, float fillAmount)
    {
        _hearts[listNum].transform.GetChild(0).GetComponent<Image>().fillAmount = fillAmount;
    }

    void UpMaxHeart()
    {
        _hearts[_hpCount].gameObject.SetActive(true);
        _curHeart = _hearts[_hpCount].transform.GetChild(0).gameObject;

        _hpCount++;

        for (int count = 0; count < _hpCount; count++)
            SetfillAmount(count, 1f);
        
    }

    void DownMaxHeart()
    {
        if (PlayerStatus.Instance._hp <= 0)
            return;

        float curAmount = _curHeart.GetComponent<Image>().fillAmount;
        _curHeart = _hearts[_hpCount - 2].transform.GetChild(0).gameObject;
        _hearts[_hpCount - 1].gameObject.SetActive(false);
        _curHeart.GetComponent<Image>().fillAmount = curAmount;

        _hpCount--; 
    }

    void KeepMaxHeart()
    {
        float lastHeartFill = ((_player._hp % _maxHeart) / _maxHeart);

        int fullHeartCount = (int)System.Math.Truncate(_player._hp / _maxHeart);

        for (int count = 0; count < fullHeartCount; count++)
            SetfillAmount(count, 1f);

        for (int count = fullHeartCount; count < _hpCount; count++)
            SetfillAmount(count, 0f);

        SetfillAmount(fullHeartCount, lastHeartFill);

    }
}
