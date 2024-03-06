using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2WallGroup : MonoBehaviour
{
    Boss2_StageManager _stageManager;
    int _childNum;

    void Start()
    {
        _stageManager = GameObject.FindWithTag("Stage Manager").GetComponent<Boss2_StageManager>();
        _childNum = gameObject.transform.childCount;
        GetComponent<ProtectBoss2>().enabled = false;


        for (int index = 0; index < _childNum; index++)
        {
            GameObject childWall = gameObject.transform.GetChild(index).gameObject;
            childWall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }


    public void RigidBodyOn()
    {
        for (int index = 0; index < _childNum; index++)
        {
            GameObject childWall = gameObject.transform.GetChild(index).gameObject;
            childWall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<ProtectBoss2>().enabled = true;
            childWall.AddComponent<ProtectBoss2>();
        }
    }
}



