using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    
    public bool _isReplay
    { get; private set; }

    public bool _isGameOver
    { get; private set; }


    private void Start()
    {
        _isGameOver = false;
        _isReplay = false;
    }

    // ���� ����� ���� ����
    public void SetIsReplay(bool isTrue)
    {
        Debug.Log(isTrue + "���÷���");
        _isReplay = isTrue;
    }

    // ���ӿ��� ����
    public void SetIsGameOver(bool isTrue)
    {
        Debug.Log(isTrue+"���ӿ�������");
        _isGameOver = isTrue;
    }
}
