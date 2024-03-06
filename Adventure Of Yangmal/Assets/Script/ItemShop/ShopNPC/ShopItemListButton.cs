using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemListButton : BaseButton
{
    enum eBUYSTATE
    {
        BUY,
        NOMONEY,
        MAXVALUE
    }


    eBUYSTATE _eState = eBUYSTATE.BUY;

    // �����ϱ� â ��ư
    ShopButton _shopBt;

    ShopItemList _itemList;
    PlayerInfo _playerInfo;
    PlayerCoinManager _playerCoin;

    public GameObject _errorMessage_NoMoney;
    public GameObject _errorMessage_MaxValue;
    public GameObject _successMessage;

    public Button _buyBt;

    ShopNpcMove _npcMove;


    private void Start()
    {
        // �����ϱ� â ��ư ĳ��
        _shopBt = GameObject.FindWithTag("Shop").GetComponent<ShopButton>();
        _itemList = GetComponent<ShopItemList>();
        _playerCoin
            = GameObject.FindWithTag("Player").GetComponent<PlayerCoinManager>();
        _playerInfo = GetComponent<PlayerInfo>();
        _npcMove = GetComponent<ShopNpcMove>();
    }

    // �����ϱ�
    public override void OkButton()
    {
        // ���Ź�ư ��Ȱ��ȭ
        _buyBt.interactable = false;

        // ������ �غ�
        int price = _itemList._price;
        _itemList._readyItem.GetComponent<ShopItem>().SetIsMaxValue();

        // ���� ����
        // 1) �������� nomoney
        if (_playerCoin._playerCoin - price < 0)
            _eState = eBUYSTATE.NOMONEY;

        // 2) �ִ�ġ ����&�̹� �ִ� ������ = maxValue
        else if (_itemList._readyItem.GetComponent<ShopItem>()._isMaxValue)
            _eState = eBUYSTATE.MAXVALUE;

        // 3) �Ѵ� �ƴϸ� buy
        else if(!_itemList._readyItem.GetComponent<ShopItem>()._isMaxValue)
            _eState = eBUYSTATE.BUY;

        // 4) ������ ���¿� ���� ��ư ����
        switch (_eState)
        {
            case eBUYSTATE.NOMONEY:
                _errorMessage_NoMoney.SetActive(true);
                StartCoroutine(WaitAndDestroy(_errorMessage_NoMoney));
                break;

            case eBUYSTATE.MAXVALUE:
                _errorMessage_MaxValue.SetActive(true);
                StartCoroutine(WaitAndDestroy(_errorMessage_MaxValue));
                break;

            case eBUYSTATE.BUY:
                BuyItem();
                break;
        }
    }


    // �޼���â 1�� �� ���� & ���Ź�ư Ȱ��ȭ �ڷ�ƾ & �����۹�ư���� ����
    IEnumerator WaitAndDestroy(GameObject message)
    {
        yield return new WaitForSeconds(1f);
        message.SetActive(false);
        _buyBt.interactable = true;
        _buyBt.Select();

    }

    // ����
    void BuyItem()
    {
        int price;
        price = _itemList._price;

        // �� �Һ�
        _playerCoin.ConsumeCoin(price);
        _successMessage.SetActive(true);

        // ������ ������ ��� , ���ǵ� �� ������ ����
        if (_itemList._readyItem.GetComponent<ItemSpeedUp>() != null)
            BuySpeedUpItem();

        else
            _itemList._readyItem.GetComponent<IPlayerItem>().UseItem();

        StartCoroutine(WaitAndDestroy(_successMessage));
        // ������ â ����
        _playerInfo.SetPlayerInfoText();
    }
    

    // ���ǵ� �������� ����
    void BuySpeedUpItem()
    {
        _itemList._readyItem.GetComponent<ItemSpeedUp>().BuySpeedItem();
    }



    // shop�ݱ� �� 
    public override void CloseUI(GameObject shut)
    {
        StartCoroutine(_shopBt.gameObject.GetComponent<ShopNPC>().SetCollider(shut));
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);


        _npcMove._curState = ShopNpcMove.eNPCMOVE.STROLL;

    }


}
