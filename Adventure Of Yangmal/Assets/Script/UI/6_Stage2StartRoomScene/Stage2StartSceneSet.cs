using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2StartSceneSet : SceneSetting
{
    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 원상복구
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        PlayerTrsfReset();

        // ui 설정
        SetStartUi();
    }


}
