using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemCSVManager : CSVReader
{
    // 아이템 구조체 선언
    public struct ItemStruct
    {
        public int _num; // 번호
        public string _name; // 이름
        public string _info; // 설명
        public int _price; // 가격

        public ItemStruct(int num, string name, string info, int price)
        {
            this._num = num;
            this._name = name;
            this._info = info;
            this._price = price;
        }

    }

    public List<ItemStruct> _itemList = new List<ItemStruct>();


    private void Awake()
    {
        SetItemStruct();
    }


    // 아이템 구조체 생성
    void SetItemStruct()
    {
        ReadCSV("ItemList");

        for(int i =0; i<_dataList.Count; i++)
        {
            string[] data = _dataList[i].Split(',');

            int num = int.Parse(data[0]);
            string name = data[1];
            string info = data[2];
            int price = int.Parse(data[3]);

            ItemStruct item = new ItemStruct(num, name, info, price);
            _itemList.Add(item);
        }

    }//void SetItemStruct

}
