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

    // �� �Ѿ�� ��ź & �̻��� & �Ѿ� setActiveFalse
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
