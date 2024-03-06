using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1EnterDoor : MonoBehaviour
{
    public GameObject _Boss1EnterUi;
    GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _Boss1EnterUi.gameObject.SetActive(true);

            // �ڽ� ������Ʈ ����
            for(int i = 0; i<_Boss1EnterUi.transform.childCount; i++)
            {
                _Boss1EnterUi.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            StartCoroutine(_Boss1EnterUi.GetComponent<EndStageUi>().Flow());

            // ������Ʈ �ݶ��̴� ����
            gameObject.GetComponent<Collider2D>().enabled = false;

            // �÷��̾� ��ž
            UiManager.Instance.StopPlayer(_player);
        }

    }

    public void StartColliderCoroutine()
    {
        StartCoroutine(SetCollider());
    }
    public IEnumerator SetCollider()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
