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
            // 1. 플레이어 , 메인ui setOn
            SetOn(PlayerStatus.Instance.gameObject);
            SetOn(UiManager.Instance.transform.parent.gameObject);

            //2.리플레이 & 첫 번째 던전 시작할 때 중복 오브젝트 제거 & 플레이어 초기화
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
        // 게임오버 해제
        GameManager.Instance.SetIsGameOver(false);

        // instance에 남아있는 이전의 bulletpool 삭제
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().ResetAllBullet();
        // 풀 재생산, 총알 초기화
        _playerBulletPool.GetComponent<PlayerBulletPoolManager>().InstaniatePlayerBulletPool();
    }


    void ResetAll()
    {
        // playerStatus 관련 변수 초기화
        PlayerStatus.Instance.PlayerStatusAllReset();

        // playerMove 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerMove>().ResetMoveSpeed();

        // playerCoin 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerCoinManager>().ResetCoin();

        // 나비가드 아이템 착용중이라면 아이템 삭제
        if (PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>()._isButterfly)
        {
            GameObject butterfly = GameManager.Instance.gameObject.transform.GetChild(0).gameObject;
            Destroy(butterfly);
        }
        // playerItem 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerItemManager>().ResetItem();

        // playerShooter 관련 변수 초기화
        PlayerStatus.Instance.gameObject.GetComponent<PlayerShooter>().ResetPlayerShoot();


        // player 투명도 & 콜라이더 원상복구
        SpriteRenderer playerSprite =
        PlayerStatus.Instance.gameObject.GetComponent<SpriteRenderer>();

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);

       PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
