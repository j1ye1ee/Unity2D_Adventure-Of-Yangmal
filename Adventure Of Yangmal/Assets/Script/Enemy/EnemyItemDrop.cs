using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{

    enum eITEMDROP_STATE
    {
        GOODITEM,
        MAXHPUP,
        SITEM,
        COIN,
    }

    eITEMDROP_STATE _eStatae;

    public GameObject[] _goodItem;
    public GameObject _maxHpUp;
    public GameObject[] _sItem;
    public GameObject _dropItem;
    public GameObject _coin;

    void Start()
    {
        SetDropItem();
    }

    void SetDropItem()
    {
        // ������ ��� ���� ����
        SetDropState();

        switch(_eStatae)
        {
            case eITEMDROP_STATE.GOODITEM:
                DropGoodItem();
                break;
            case eITEMDROP_STATE.MAXHPUP:
                DropMaxHpUp();
                break;
            case eITEMDROP_STATE.SITEM:
                DropSItem();
                break;
            case eITEMDROP_STATE.COIN:
                DropCoin();
                break;
        }
    }//void SetDropItem()




    // ������ ��� ���� ����
    void SetDropState()
    {
        // 30������ Ȯ���� ������ ��� / else = coin ���
        float dropRate = Random.Range(1f, 100f);
        if (dropRate >= 71f)
        {
            float rate = Random.Range(1f, 100f);

            //3% Ȯ���� Sitem ���
            if (rate <= 3)
                _eStatae = eITEMDROP_STATE.SITEM;

            //10% Ȯ���� MaxHpUp ���
            else if (rate > 3 && rate <= 13)
                _eStatae = eITEMDROP_STATE.MAXHPUP;

            // 87% Ȯ���� goodItem ���
            else if (rate > 13 && rate <= 100)
                _eStatae = eITEMDROP_STATE.GOODITEM;
        }// if (dropRate >= 51f)

        else
            _eStatae = eITEMDROP_STATE.COIN;
    }

    // goodItem ���
    void DropGoodItem()
    {
        int itemNum;
        itemNum = Random.Range(0, 6);
        _dropItem = _goodItem[itemNum];
    }
    // sItem ���
    void DropSItem()
    {
        int itemNum;
        itemNum = Random.Range(0, 3);
        _dropItem = _sItem[itemNum];
    }
    // MaxHpUp ���
    void DropMaxHpUp()
    {
        _dropItem = _maxHpUp;
    }

    void DropCoin()
    {
        _dropItem = _coin;
    }

}
