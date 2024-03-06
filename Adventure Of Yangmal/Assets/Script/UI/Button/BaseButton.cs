using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    // 버튼 부모 클래스

    // 첫번째 선택되어있을 버튼 지정
    public Button _firstSelect;



    // 선택
    public virtual void OkButton() { }

    // 선택 후 show 띄우기
    public virtual void OkButton(GameObject show)
    {
        //플레이어 움직임 정지
        UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);
        show.SetActive(true);
    }


    // 창 닫기
    // 닫을 gameObject 지정
    public virtual void CloseUI(GameObject shut)
    {
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        shut.SetActive(false);
    }

    public virtual void CloseUI() { }






}
