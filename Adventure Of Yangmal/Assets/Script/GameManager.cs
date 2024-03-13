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

    // 씬 넘어갈때 폭탄 & 미사일 & 총알 setActiveFalse
    public void SceneChangeBulletDestroy()
    {
        GameObject pool = PlayerBulletPoolManager.Instance.gameObject;

        for (int i = 0; i < pool.transform.childCount; i++)
        {
            GameObject bullet = pool.transform.GetChild(i).gameObject;

            if (bullet.activeSelf)
            {
                if (bullet.GetComponent<Bomb>() != null)
                {
                    bullet.GetComponent<Bomb>().StopAllCoroutines();
                    bullet.GetComponent<Bomb>().BombSetActiveFasle();
                }

                else if (bullet.GetComponent<GuideMissile>() != null)
                {
                    bullet.GetComponent<GuideMissile>().StopAllCoroutines();
                    bullet.GetComponent<GuideMissile>().MissileSetActiveFasle();
                }

                else
                    bullet.SetActive(false);
            }
        }
    }
}
