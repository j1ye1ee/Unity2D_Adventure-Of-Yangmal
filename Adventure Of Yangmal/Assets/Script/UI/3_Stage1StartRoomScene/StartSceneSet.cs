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
            // 1. 플레이어 , 메인ui setOn
            SetOn(PlayerStatus.Instance.gameObject);
            SetOn(UiManager.Instance.transform.parent.gameObject);

            //2.리플레이 & 첫 번째 던전 시작할 때 중복 오브젝트 제거 & 플레이어 초기화
            if (_curScene.name == "Stage1StartRoom")
            {
                if(_mainCanvas != null)
                    Destroy(_mainCanvas.gameObject);
            }

            ResetAll();

            // 3. 플레이어 위치 리셋
            PlayerTrsfReset();

            // 4. 게임오버 해제
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
        // playerStatus 관련 변수 초기화
        PlayerStatus.Instance.PlayerStatusAllReset();

        // playerMove 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerMove>().ResetMoveSpeed();

        // playerCoin 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerCoinManager>().ResetCoin();

        // playerItem 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>().ResetItem();

        // playerShooter 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerShooter>().ResetPlayerShoot();

        // playerBullet 관련 변수 초기화
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().ResetAllBullet();

        // player 투명도 원상복구
        SpriteRenderer playerSprite =
        PlayerStatus.Instance.gameObject.GetComponent<SpriteRenderer>();

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);

    }
}
