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
        // 구매버튼 비활성화
        _buyBt.interactable = false;
        int price = _itemBox._price;

        // 아이템 최대 개수만큼 생성되었다면
        if (_itemBox._vectorList.Count == 44)
        {
            _errorMessage2.SetActive(true);
            StartCoroutine(WaitAndDestroy(_errorMessage2));
        }


        // 아니면 진행
        else
        {
            // 플레이어의 돈이 부족하다면 에러메세지 띄움

            if (_playerCoin._playerCoin - price < 0)
            {
                _errorMessage.SetActive(true);
                StartCoroutine(WaitAndDestroy(_errorMessage));
                return;
            }

            // 아니라면 구매 진행
            else
            {
                _playerCoin.ConsumeCoin(price);
                _successMessage.SetActive(true);
                StartCoroutine(WaitAndDestroy(_successMessage));
            }

            // 아이템 박스에서 랜덤위치에 아이템 생성
            _itemBox.InstantiateItem();

            // info 세팅
            GetComponent<PlayerInfo>().SetPlayerInfoText();
        }

    }

    // 메세지창 1초 후 제거 & 구매버튼 활성화 코루틴 & 구매버튼으로 선택
    IEnumerator WaitAndDestroy(GameObject message)
    {
        yield return new WaitForSeconds(1f);
        message.SetActive(false);
        _buyBt.interactable = true;
        _buyBt.Select();
    }


    // BOX 콜라이더 켜기 & _isUIOpen = false 로 변경
    public override void CloseUI(GameObject shut)
    {
        // 플레이어 움직임 복구
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        // 콜라이더 3초 뒤에 켜기 & _uiOpen false로 변경
        StartCoroutine(_itemBox.GetComponent<ItemShopBox>().SetCollider());
        shut.gameObject.SetActive(false);

    }

}
