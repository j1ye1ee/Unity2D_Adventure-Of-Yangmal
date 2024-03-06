using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    // shop NPC �±� --> �����ϱ� â ����

    public GameObject _fullUi;
    public GameObject _npcInfoUi;
    bool _isUiOpen = false;
    public ShopButton _shopBt;
    PlayerInfo _playerInfo;



    void Start()
    {
        _shopBt = GetComponent<ShopButton>();
        _playerInfo = GetComponent<PlayerInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isUiOpen && other.tag == "Player")
        {
            // �÷��̾�� �±��� ���� ������ ����Ʈ�� PLAYER INFO ���μ����ϱ�
            _playerInfo.SetPlayerInfoText();

            
            //���� ��ư ���� ���·�
            _shopBt._firstSelect.Select();

            _fullUi.SetActive(true);
            _npcInfoUi.SetActive(true);
            _isUiOpen = true;

            //�÷��̾� ������ ����
            UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

            // �ݶ��̴� ����
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // 3�� �� �ݶ��̴� �ѱ�
    public IEnumerator SetCollider()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;
    }

    // �Ű������� �ִ� ���
    public IEnumerator SetCollider(GameObject shut)
    {
        shut.SetActive(false);
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;

    }
}
