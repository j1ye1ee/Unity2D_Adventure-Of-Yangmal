using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemBoxButton : BaseButton
{
    public Button _buyBt;
    ItemShopBox _itemBox;
    public GameObject _errorMessage;
    public GameObject _errorMessage2;
    public GameObject _successMessage;
    PlayerInfo _playerInfo;
    PlayerCoinManager _playerCoin;




    void Start()
    {
        _playerCoin
           = GameObject.FindWithTag("Player").GetComponent<PlayerCoinManager>();
        _playerInfo = GetComponent<PlayerInfo>();
        _itemBox = GetComponent<ItemShopBox>();
    }

    public override void OkButton()
    {
        // ���Ź�ư ��Ȱ��ȭ
        _buyBt.interactable = false;
        int price = _itemBox._price;

        // ������ �ִ� ������ŭ �����Ǿ��ٸ�
        if (_itemBox._vectorList.Count == 44)
        {
            _errorMessage2.SetActive(true);
            StartCoroutine(WaitAndDestroy(_errorMessage2));
        }


        // �ƴϸ� ����
        else
        {
            // �÷��̾��� ���� �����ϴٸ� �����޼��� ���

            if (_playerCoin._playerCoin - price < 0)
            {
                _errorMessage.SetActive(true);
                StartCoroutine(WaitAndDestroy(_errorMessage));
                return;
            }

            // �ƴ϶�� ���� ����
            else
            {
                _playerCoin.ConsumeCoin(price);
                _successMessage.SetActive(true);
                StartCoroutine(WaitAndDestroy(_successMessage));
            }

            // ������ �ڽ����� ������ġ�� ������ ����
            _itemBox.InstantiateItem();

            // info ����
            GetComponent<PlayerInfo>().SetPlayerInfoText();
        }

    }

    // �޼���â 1�� �� ���� & ���Ź�ư Ȱ��ȭ �ڷ�ƾ & ���Ź�ư���� ����
    IEnumerator WaitAndDestroy(GameObject message)
    {
        yield return new WaitForSeconds(1f);
        message.SetActive(false);
        _buyBt.interactable = true;
        _buyBt.Select();
    }


    // BOX �ݶ��̴� �ѱ� & _isUIOpen = false �� ����
    public override void CloseUI(GameObject shut)
    {
        // �÷��̾� ������ ����
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        // �ݶ��̴� 3�� �ڿ� �ѱ� & _uiOpen false�� ����
        StartCoroutine(_itemBox.GetComponent<ItemShopBox>().SetCollider());
        shut.gameObject.SetActive(false);

    }

}
