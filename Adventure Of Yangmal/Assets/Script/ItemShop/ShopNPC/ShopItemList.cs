using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemList : MonoBehaviour
{
    public EventSystem _event;
    public Button _1stSelect;
    public Button _preSelect;

    public string _name;
    public string _info;
    public int _price;
    public Image _itemImage;
    public Sprite[] _images;

    public Text _nameText;
    public Text _infoText;
    public Text _priceText;
    public GameObject[] _items;
    public GameObject _readyItem;
    
    void Start()
    {
        _event = GameObject.FindWithTag("eventSystem").GetComponent<EventSystem>();
        _1stSelect.Select();
    }



    // 아이템 리스트 셀렉트 이벤트 트리거
    // 인포 창 내용 변경
    public void SelectItem()
    {
        
            _preSelect = _event.currentSelectedGameObject.GetComponent<Button>();

            // 아이템이 지닌 샵아이템 번호로 저장
            int num = _preSelect.gameObject.GetComponent<ShopItem>()._shopItemNum;

            // 구매할 아이템 관련 변수 세팅
            ReadyItem(num);

            // 설명창 세팅
            ChangeInfoUI(_name, _info, _price);

            // 아이템 효과 세팅
            //_item = _preSelect.gameObject.GetComponent<IPlayerItem>();
            _readyItem = _items[num];

            // 아이템 이미지 세팅
            _itemImage.sprite = _images[num];

    }


    // 구매할 아이템에 관한 변수 세팅
    void ReadyItem(int num)
    {
        ItemCSVManager item = GetComponent<ItemCSVManager>();

        _name = item._itemList[num]._name;
        _info = item._itemList[num]._info.Replace("\\n", "\n");
        _price = item._itemList[num]._price;
    }



    // 설명창 세팅
    void ChangeInfoUI(string name, string info, int price)
    {
        _nameText.text = name;
        _infoText.text = info;
        _priceText.text = price.ToString() + "G";
    }


    // 구매 버튼 셀렉트 이벤트 트리거
    // 구매 버튼에서의 왼쪽 방향키 --> 이전의 버튼으로 이동할수 있도록
    public void ReturnPreSelect()
    {
        //1) Navigation 변수의 selectOnLeft 설정
        Navigation curNavi =
            _event.currentSelectedGameObject.GetComponent<Button>().navigation;
        curNavi.selectOnLeft = _preSelect;

        //2) 현제 셀렉된 버튼에 대입
        _event.currentSelectedGameObject.GetComponent<Button>().navigation = curNavi;
    }



}
