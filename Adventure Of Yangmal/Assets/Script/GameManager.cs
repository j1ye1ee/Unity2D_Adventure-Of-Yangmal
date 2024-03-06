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

    // 게임 재시작 여부 세팅
    public void SetIsReplay(bool isTrue)
    {
        Debug.Log(isTrue + "리플레이");
        _isReplay = isTrue;
    }

    // 게임오버 세팅
    public void SetIsGameOver(bool isTrue)
    {
        Debug.Log(isTrue+"게임오버세팅");
        _isGameOver = isTrue;
    }
}
