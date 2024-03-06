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



    // ������ ����Ʈ ����Ʈ �̺�Ʈ Ʈ����
    // ���� â ���� ����
    public void SelectItem()
    {
        
            _preSelect = _event.currentSelectedGameObject.GetComponent<Button>();

            // �������� ���� �������� ��ȣ�� ����
            int num = _preSelect.gameObject.GetComponent<ShopItem>()._shopItemNum;

            // ������ ������ ���� ���� ����
            ReadyItem(num);

            // ����â ����
            ChangeInfoUI(_name, _info, _price);

            // ������ ȿ�� ����
            //_item = _preSelect.gameObject.GetComponent<IPlayerItem>();
            _readyItem = _items[num];

            // ������ �̹��� ����
            _itemImage.sprite = _images[num];

    }


    // ������ �����ۿ� ���� ���� ����
    void ReadyItem(int num)
    {
        ItemCSVManager item = GetComponent<ItemCSVManager>();

        _name = item._itemList[num]._name;
        _info = item._itemList[num]._info.Replace("\\n", "\n");
        _price = item._itemList[num]._price;
    }



    // ����â ����
    void ChangeInfoUI(string name, string info, int price)
    {
        _nameText.text = name;
        _infoText.text = info;
        _priceText.text = price.ToString() + "G";
    }


    // ���� ��ư ����Ʈ �̺�Ʈ Ʈ����
    // ���� ��ư������ ���� ����Ű --> ������ ��ư���� �̵��Ҽ� �ֵ���
    public void ReturnPreSelect()
    {
        //1) Navigation ������ selectOnLeft ����
        Navigation curNavi =
            _event.currentSelectedGameObject.GetComponent<Button>().navigation;
        curNavi.selectOnLeft = _preSelect;

        //2) ���� ������ ��ư�� ����
        _event.currentSelectedGameObject.GetComponent<Button>().navigation = curNavi;
    }



}
