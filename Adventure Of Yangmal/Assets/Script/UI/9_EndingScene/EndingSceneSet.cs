using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneSet : SceneSetting
{
    private void Awake()
    {
        SetOff(PlayerStatus.Instance.gameObject);
        SetOff(UiManager.Instance.transform.parent.gameObject);
    }
}
