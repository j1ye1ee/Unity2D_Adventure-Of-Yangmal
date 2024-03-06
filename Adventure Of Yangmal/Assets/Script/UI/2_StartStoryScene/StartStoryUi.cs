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

        // ��ư 3�ʰ� ��Ȱ��&��Ȱ��
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


    // ���� ȭ�� ����~ ���� ������
    IEnumerator EnterToStartScene()
    {
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // ���̵�ƿ� ���� �ʱ�ȭ
        _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd = false;
        SceneManager.LoadScene(_nextStage);
    }


    // ����ȭ�� ���� --> ���� �̹��� ����
    protected IEnumerator OpenNextPage(GameObject next)
    {
        //����ȭ�� ����~
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());

        yield return new WaitUntil(
    () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // ���̵�ƿ� ���� �ʱ�ȭ
        _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd = false;
        _fadeOut.SetActive(false);

        next.SetActive(true);
    }

    // 3�� �� ��ư ����
    protected IEnumerator OpenSkipBt()
    {
        yield return new WaitForSeconds(3f);

        _bts.gameObject.SetActive(true);
        _skipBt.Select();

    }

    // ����ȭ�� ���� --> ���� ����������
    IEnumerator OpenNextStage()
    {
        //����ȭ�� ����~
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());

        yield return new WaitUntil(
    () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        SceneManager.LoadScene(_nextStage);

    }

    // ��ư 3�ʰ� ��Ȱ��ȭ �� Ȱ��
    protected IEnumerator ButtonSet()
    {
        _skipBt.enabled = false;
        yield return new WaitForSeconds(2f);
        _skipBt.enabled = true;

    }

}
