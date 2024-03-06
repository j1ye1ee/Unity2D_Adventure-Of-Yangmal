using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    Boss2_StageManager _stageManager;
    bool _isCheck = false;

    private void Start()
    {
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();
        GetComponent<Collider2D>().enabled = false;
    }
    private void Update()
    {
        if (_stageManager._curState == Boss2_StageManager.eSTAGE_STATE.PAGE1CLEAR
            && !_isCheck)
            GetComponent<Collider2D>().enabled = true;

        else
            return;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "OriginPositionCheck")
        {
            other.gameObject.transform.parent.GetComponent<Boss2Move>()._isGoal = true;
            _isCheck = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
