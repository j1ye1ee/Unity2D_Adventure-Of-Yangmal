using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartStoryUi : MonoBehaviour
{
    public List<GameObject> _storyImages = new List<GameObject>();
    public GameObject _fadeOut;
    public Button _skipBt;
    public GameObject _bts;
    public string _nextStage;

    protected int _num = 0;

    private void Start()
    {
        StartCoroutine(OpenSkipBt());
    }


    protected void AllSkip()
    {

        StartCoroutine(EnterToStartScene());
    }

    public virtual void Skip()
    {

        // 버튼 3초간 비활성&재활성
        StartCoroutine(ButtonSet());

        _storyImages[_num].gameObject.SetActive(false);
        _num++;

        if (_num == _storyImages.Count)
        {
                StartCoroutine(OpenNextStage());
        }
        else
            StartCoroutine(OpenNextPage(_storyImages[_num]));

    }


    // 검정 화면 생성~ 다음 씬으로
    IEnumerator EnterToStartScene()
    {
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // 페이드아웃 변수 초기화
        _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd = false;
        SceneManager.LoadScene(_nextStage);
    }


    // 검정화면 생성 --> 다음 이미지 띄우기
    protected IEnumerator OpenNextPage(GameObject next)
    {
        //검은화면 생성~
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());

        yield return new WaitUntil(
    () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // 페이드아웃 변수 초기화
        _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd = false;
        _fadeOut.SetActive(false);

        next.SetActive(true);
    }

    // 3초 뒤 버튼 오픈
    protected IEnumerator OpenSkipBt()
    {
        yield return new WaitForSeconds(3f);

        _bts.gameObject.SetActive(true);
        _skipBt.Select();

    }

    // 검정화면 생성 --> 다음 스테이지로
    IEnumerator OpenNextStage()
    {
        //검은화면 생성~
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());

        yield return new WaitUntil(
    () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        SceneManager.LoadScene(_nextStage);

    }

    // 버튼 3초간 비활성화 후 활성
    protected IEnumerator ButtonSet()
    {
        _skipBt.enabled = false;
        yield return new WaitForSeconds(2f);
        _skipBt.enabled = true;

    }

}
