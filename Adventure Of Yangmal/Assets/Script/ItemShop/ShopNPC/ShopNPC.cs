using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    // shop NPC 태깅 --> 구경하기 창 열기

    public GameObject _fullUi;
    public GameObject _npcInfoUi;
    bool _isUiOpen = false;
    public ShopButton _shopBt;
    PlayerInfo _playerInfo;



    void Start()
    {
        _shopBt = GetComponent<ShopButton>();
        _playerInfo = GetComponent<PlayerInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isUiOpen && other.tag == "Player")
        {
            // 플레이어와 태깅한 순간 아이템 리스트의 PLAYER INFO 코인설정하기
            _playerInfo.SetPlayerInfoText();

            
            //구매 버튼 선택 상태로
            _shopBt._firstSelect.Select();

            _fullUi.SetActive(true);
            _npcInfoUi.SetActive(true);
            _isUiOpen = true;

            //플레이어 움직임 정지
            UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

            // 콜라이더 끄기
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // 3초 뒤 콜라이더 켜기
    public IEnumerator SetCollider()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;
    }

    // 매개변수가 있는 경우
    public IEnumerator SetCollider(GameObject shut)
    {
        shut.SetActive(false);
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;

    }
}
