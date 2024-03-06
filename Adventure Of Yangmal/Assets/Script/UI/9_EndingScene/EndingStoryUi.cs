using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingStoryUi : StartStoryUi
{
    [SerializeField]
    GameObject _theEndUi;


    void Start()
    {
        StartCoroutine(OpenSkipBt());
    }

    // endingStoru skip
    public override void Skip()
    {
        // 버튼 3초간 비활성&활성
        StartCoroutine(ButtonSet());

        _storyImages[_num].gameObject.SetActive(false);
        _num++;

        if (_num == _storyImages.Count)
        {
            _theEndUi.SetActive(true);
            _skipBt.gameObject.SetActive(false);
        }
        else
            StartCoroutine(OpenNextPage(_storyImages[_num]));


    }


}
