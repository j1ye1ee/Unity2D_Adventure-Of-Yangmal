using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SItemShopBox : ItemShopBox
{
    public enum EITEM
    {
        eSITEM,
        eNORMALITEM
    }

    public EITEM _eItem;

    //랜덤 위치에 랜덤 Sitem 생성,
    // 30프로의 확률 : normal아이템, 나머지 : SITEM
    public override void InstantiateItem()
    {
        // 최대 생성갯수 44 넘지 않았다면 랜덤위치에 랜덤아이템 생성
        if (_vectorList.Count != 44)
        {
            int itemNum=0;
            int rate = Random.Range(1, 101);

            if (rate <= 30)
                _eItem = EITEM.eNORMALITEM;
            else
                _eItem = EITEM.eSITEM;


            switch (_eItem)
            {
                case EITEM.eNORMALITEM:
                    itemNum = Random.Range(3, 10);
                    break;
                case EITEM.eSITEM:
                    itemNum = Random.Range(0, 3);
                    break;
            }

         GameObject item = Instantiate(_item[itemNum], RandomInstantiatePosition(), transform.rotation);
        }// if (_vectorList.Count != 44)
    }//public override void InstantiateItem()
}
