using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shop 아이템 부모클래스

public class ShopItem : Item
{

    public int _shopItemNum
    {
        get; protected set;
    }

    public bool _isMaxValue
    {
        get; protected set;
    }

    public float _maxValue
    {
        get; protected set;
    }

    // maxValue 도달 검사 
    public virtual void SetIsMaxValue() { }
}
