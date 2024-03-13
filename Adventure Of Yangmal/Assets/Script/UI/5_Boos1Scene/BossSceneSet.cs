using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneSet : SceneSetting
{
    private void Awake()
    {
        //플레이어 setOn
        SetOn(PlayerStatus.Instance.gameObject);
        PlayerTrsfReset();

        // main Ui SetOn
        SetOn(UiManager.Instance.transform.parent.gameObject);
        
        // stageName set
        UiManager.Instance.SetStageText(_stageName);

        // 플레이어 콜라이더 true
        PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;

        // 플레이어 위치 재설정
        PlayerTrsfReset();

        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
    }
}
