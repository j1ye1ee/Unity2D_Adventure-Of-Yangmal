using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : BaseButton
{
    public Button _buyBt;
    public Button _closeBt;
    ShopNPC _shopNpc;
    ShopNpcMove _npcMove;



    private void Start()
    {
        _shopNpc = GetComponent<ShopNPC>();
        _npcMove = GetComponent<ShopNpcMove>();
    }


    public override void OkButton(GameObject show)
    {
        GameObject shut;
        shut = GetComponent<ShopNPC>()._npcInfoUi;

        // 샵 리스트 활성화
        show.gameObject.SetActive(true);

        // 샵 리스트의 첫번째 아이템 선택 상태로
        GetComponent<ShopItemList>()._1stSelect.Select();

        // 현재 창 닫기
        shut.SetActive(false);

    }

    //창 닫기
    public override void CloseUI(GameObject shut)
    {
        base.CloseUI(shut);
        StartCoroutine(_shopNpc.SetCollider());

        _npcMove._curState = ShopNpcMove.eNPCMOVE.STROLL;
    }

}
