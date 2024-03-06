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

    // 구경하기 창 버튼
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
        // 구경하기 창 버튼 캐싱
        _shopBt = GameObject.FindWithTag("Shop").GetComponent<ShopButton>();
        _itemList = GetComponent<ShopItemList>();
        _playerCoin
            = GameObject.FindWithTag("Player").GetComponent<PlayerCoinManager>();
        _playerInfo = GetComponent<PlayerInfo>();
        _npcMove = GetComponent<ShopNpcMove>();
    }

    // 구매하기
    public override void OkButton()
    {
        // 구매버튼 비활성화
        _buyBt.interactable = false;

        // 아이템 준비
        int price = _itemList._price;
        _itemList._readyItem.GetComponent<ShopItem>().SetIsMaxValue();

        // 상태 지정
        // 1) 돈없으면 nomoney
        if (_playerCoin._playerCoin - price < 0)
            _eState = eBUYSTATE.NOMONEY;

        // 2) 최대치 도달&이미 있는 아이템 = maxValue
        else if (_itemList._readyItem.GetComponent<ShopItem>()._isMaxValue)
            _eState = eBUYSTATE.MAXVALUE;

        // 3) 둘다 아니면 buy
        else if(!_itemList._readyItem.GetComponent<ShopItem>()._isMaxValue)
            _eState = eBUYSTATE.BUY;

        // 4) 지정한 상태에 따라 버튼 동작
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


    // 메세지창 1초 후 제거 & 구매버튼 활성화 코루틴 & 아이템버튼으로 선택
    IEnumerator WaitAndDestroy(GameObject message)
    {
        yield return new WaitForSeconds(1f);
        message.SetActive(false);
        _buyBt.interactable = true;
        _buyBt.Select();

    }

    // 구매
    void BuyItem()
    {
        int price;
        price = _itemList._price;

        // 돈 소비
        _playerCoin.ConsumeCoin(price);
        _successMessage.SetActive(true);

        // 프리팹 아이템 사용 , 스피드 업 아이템 예외
        if (_itemList._readyItem.GetComponent<ItemSpeedUp>() != null)
            BuySpeedUpItem();

        else
            _itemList._readyItem.GetComponent<IPlayerItem>().UseItem();

        StartCoroutine(WaitAndDestroy(_successMessage));
        // 소지금 창 설정
        _playerInfo.SetPlayerInfoText();
    }
    

    // 스피드 아이템의 예외
    void BuySpeedUpItem()
    {
        _itemList._readyItem.GetComponent<ItemSpeedUp>().BuySpeedItem();
    }



    // shop닫기 후 
    public override void CloseUI(GameObject shut)
    {
        StartCoroutine(_shopBt.gameObject.GetComponent<ShopNPC>().SetCollider(shut));
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);


        _npcMove._curState = ShopNpcMove.eNPCMOVE.STROLL;

    }


}
