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



    // �ڽ� �±� --> ���� ui ����
    private void OnTriggerEnter2D(Collider2D other)
    {

        // ���� ��ư ���� ���·�
        _itemBt._firstSelect.Select();
        // �÷��̾� ������ ����
        _info.SetPlayerInfoText();

        if (!_isUiOpen && other.tag == "Player")
        {
            _Ui.SetActive(true);
            _isUiOpen = true;

            //�÷��̾� ������ ����
            UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

            // �ݶ��̴� ����
            GetComponent<Collider2D>().enabled = false;

            // ����Ʈ �ʱ�ȭ
            _vectorList.Clear();
        }

        else
            return;
    }



    // ������ ���� ��ġ ����
    public Vector2 RandomInstantiatePosition()
    {
        bool dup;
        Vector2Int newPosition;
        Vector2Int center = new Vector2Int((int)_center.transform.position.x, (int)_center.transform.position.y);
        Debug.Log(center);

        // �ߺ� ��ġ�� ������ ������ġ �ٽ� ����
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
                        Debug.Log("�ߺ��߻�");
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



    // ���� ��ġ�� ���� ������ ����
    public virtual void InstantiateItem()
    {
        int itemNum = Random.Range(0, 9);
        GameObject item = Instantiate(_item[itemNum], RandomInstantiatePosition(), transform.rotation);
    }

   



    // 3�� �� �ݶ��̴� �ѱ�
    public IEnumerator SetCollider()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Collider2D>().enabled = true;
        _isUiOpen = false;
    }
  


}
