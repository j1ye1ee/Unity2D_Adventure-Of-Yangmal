using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneSet : SceneSetting
{
    [SerializeField]
    GameObject _mainCanvas;
    [SerializeField]
    GameObject _playerBulletPool;



    private void Awake()
    {
        Scene _curScene = SceneManager.GetActiveScene();

        if (GameManager.Instance._isReplay || GameManager.Instance._isGameOver)
        {
            // 1. �÷��̾� , ����ui setOn
            SetOn(PlayerStatus.Instance.gameObject);
            SetOn(UiManager.Instance.transform.parent.gameObject);

            //2.���÷��� & ù ��° ���� ������ �� �ߺ� ������Ʈ ���� & �÷��̾� �ʱ�ȭ
            if (_curScene.name == "Stage1StartRoom")
            {
                if (_mainCanvas != null)
                    Destroy(_mainCanvas.gameObject);
            }

            ResetAll();

            PlayerTrsfReset();
        }

    }


    private void Start()
    {
        PlayerTrsfReset();

        _playerBulletPool = PlayerBulletPoolManager.Instance.gameObject;
        SetStartUi();
        // ���ӿ��� ����
        GameManager.Instance.SetIsGameOver(false);

        // instance�� �����ִ� ������ bulletpool ����
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().ResetAllBullet();
        // Ǯ �����, �Ѿ� �ʱ�ȭ
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().InstaniatePlayerBulletPool();
    }


    void ResetAll()
    {
        // playerStatus ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.PlayerStatusAllReset();

        // playerMove ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerMove>().ResetMoveSpeed();

        // playerCoin ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerCoinManager>().ResetCoin();

        // ���񰡵� ������ �������̶�� ������ ����
        if (PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>()._isButterfly)
        {
            GameObject butterfly = GameManager.Instance.gameObject.transform.GetChild(0).gameObject;
            Destroy(butterfly);
        }
        // playerItem ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>().ResetItem();

        // playerShooter ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerShooter>().ResetPlayerShoot();


        // player ���� & �ݶ��̴� ���󺹱�
        SpriteRenderer playerSprite =
        PlayerStatus.Instance.gameObject.GetComponent<SpriteRenderer>();

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);

       PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
