using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shop ������ �θ�Ŭ����

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

    // maxValue ���� �˻� 
    public virtual void SetIsMaxValue() { }
}
