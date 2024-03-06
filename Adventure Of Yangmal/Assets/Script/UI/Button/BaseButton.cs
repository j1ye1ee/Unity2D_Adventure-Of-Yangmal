using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    // ��ư �θ� Ŭ����

    // ù��° ���õǾ����� ��ư ����
    public Button _firstSelect;



    // ����
    public virtual void OkButton() { }

    // ���� �� show ����
    public virtual void OkButton(GameObject show)
    {
        //�÷��̾� ������ ����
        UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);
        show.SetActive(true);
    }


    // â �ݱ�
    // ���� gameObject ����
    public virtual void CloseUI(GameObject shut)
    {
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        shut.SetActive(false);
    }

    public virtual void CloseUI() { }






}
