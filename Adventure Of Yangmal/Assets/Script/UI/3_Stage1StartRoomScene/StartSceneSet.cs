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

        if (GameManager.Instance._isReplay ||GameManager.Instance._isGameOver)
        {
            // 1. �÷��̾� , ����ui setOn
            SetOn(PlayerStatus.Instance.gameObject);
            SetOn(UiManager.Instance.transform.parent.gameObject);

            //2.���÷��� & ù ��° ���� ������ �� �ߺ� ������Ʈ ���� & �÷��̾� �ʱ�ȭ
            if (_curScene.name == "Stage1StartRoom")
            {
                if(_mainCanvas != null)
                    Destroy(_mainCanvas.gameObject);
            }

            ResetAll();

            // 3. �÷��̾� ��ġ ����
            PlayerTrsfReset();

            // 4. ���ӿ��� ����
            GameManager.Instance.SetIsGameOver(false);

        }
    }


    private void Start()
    {
        _playerBulletPool = PlayerBulletPoolManager.Instance.gameObject;
        SetStartUi();
    }


    void ResetAll()
    {
        // playerStatus ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.PlayerStatusAllReset();

        // playerMove ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerMove>().ResetMoveSpeed();

        // playerCoin ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerCoinManager>().ResetCoin();

        // playerItem ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>().ResetItem();

        // playerShooter ���� ���� �ʱ�ȭ
        PlayerStatus.Instance.gameObject.GetComponent<PlayerShooter>().ResetPlayerShoot();

        // playerBullet ���� ���� �ʱ�ȭ
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().ResetAllBullet();

        // player ������ ���󺹱�
        SpriteRenderer playerSprite =
        PlayerStatus.Instance.gameObject.GetComponent<SpriteRenderer>();

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);

    }
}