using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2StartSceneSet : SceneSetting
{
    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� ���󺹱�
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        PlayerTrsfReset();

        // ui ����
        SetStartUi();
    }


}
