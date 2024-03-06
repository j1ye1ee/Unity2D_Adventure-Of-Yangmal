using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopBox : MonoBehaviour
{
    public GameObject []_item;
    public GameObject _Ui;
    bool _isUiOpen = false;

    public int _price;

    public GameObject _center;

    public List<Vector2> _vectorList = new List<Vector2>();

    itemBoxButton _itemBt;
    PlayerInfo _info;

    private void Start()
    {
        _itemBt = GetComponent<itemBoxButton>();
        _info = GetComponent<PlayerInfo>();
    }



    // 박스 태깅 --> 구매 ui 오픈
    private void OnTriggerEnter2D(Collider2D other)
    {

        // 구매 버튼 선택 상태로
        _itemBt._firstSelect.Select();
        // 플레이어 소지금 설정
        _info.SetPlayerInfoText();

        if (!_isUiOpen && other.tag == "Player")
        {
            _Ui.SetActive(true);
            _isUiOpen = true;

            //플레이어 움직임 정지
            UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

            // 콜라이더 끄기
            GetComponent<Collider2D>().enabled = false;

            // 리스트 초기화
            _vectorList.Clear();
        }

        else
            return;
    }



    // 아이템 생성 위치 지정
    public Vector2 RandomInstantiatePosition()
    {
        bool dup;
        Vector2Int newPosition;
        Vector2Int center = new Vector2Int((int)_center.transform.position.x, (int)_center.transform.position.y);
        Debug.Log(center);

        // 중복 위치가 나오면 랜덤위치 다시 생성
        do
        {
            dup = false;

            int x = Random.Range(center.x - 8, center.x +8);
            int y = Random.Range(center.y - 5, center.y + 5);
            newPosition = new Vector2Int(x, y);

            if (_vectorList.Count != 0)
            {
                foreach (Vector2 prePosition in _vectorList)
                {
                    if (prePosition == newPosition)
                    {
                        dup = true;
                        Debug.Log("중복발생");
                    }
                }// foreach (Vector2 prePosition in _vectorList)
            }//if (_vectorList.Count != 0)
            
            if (dup == false)
                break;

        } while (dup == true);

        _vectorList.Add(newPosition);

        Debug.Log(newPosition.x+","+ newPosition.y);
        return newPosition;
    }



    // 랜덤 위치에 랜덤 아이템 생성
    public virtual void InstantiateItem()
    {
        int itemNum = Random.Range(0, 9);
        GameObject item = Instantiate(_item[itemNum], RandomInstantiatePosition(), transform.rotation);
    }

   



    // 3초 뒤 콜라이더 켜기
    public IEnumerator SetCollider()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;
    }
  


}
