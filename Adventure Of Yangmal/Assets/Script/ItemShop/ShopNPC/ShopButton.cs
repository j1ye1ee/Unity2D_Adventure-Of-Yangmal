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

        // �� ����Ʈ Ȱ��ȭ
        show.gameObject.SetActive(true);

        // �� ����Ʈ�� ù��° ������ ���� ���·�
        GetComponent<ShopItemList>()._1stSelect.Select();

        // ���� â �ݱ�
        shut.SetActive(false);

    }

    //â �ݱ�
    public override void CloseUI(GameObject shut)
    {
        base.CloseUI(shut);
        StartCoroutine(_shopNpc.SetCollider());

        _npcMove._curState = ShopNpcMove.eNPCMOVE.STROLL;
    }

}
