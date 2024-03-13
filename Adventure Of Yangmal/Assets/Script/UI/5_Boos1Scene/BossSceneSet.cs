using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneSet : SceneSetting
{
    private void Awake()
    {
        //�÷��̾� setOn
        SetOn(PlayerStatus.Instance.gameObject);
        PlayerTrsfReset();

        // main Ui SetOn
        SetOn(UiManager.Instance.transform.parent.gameObject);
        
        // stageName set
        UiManager.Instance.SetStageText(_stageName);

        // �÷��̾� �ݶ��̴� true
        PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;

        // �÷��̾� ��ġ �缳��
        PlayerTrsfReset();

        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
    }
}
