using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    public bool _isGuideMissile;
    public bool _isButterfly;
    public bool _isBomb;
    public GameObject _butterfly;
    itemUseEffect _itemEffect;
    public AudioSource _audio;



    private void Start()
    {
       _itemEffect = GameObject.FindWithTag("ItemUseEffect").GetComponent<itemUseEffect>();
    }



    // �����۰� �浹�� ������ ���

    private void OnTriggerEnter2D(Collider2D other)
    {
        IPlayerItem item = other.GetComponent<IPlayerItem>();
        if (item != null)
        {
            item.UseItem();

           // ������ ����Ʈ �ؽ�Ʈ ��������
           string effectText = other.GetComponent<Item>()._effectText;
            // ItemUseEffect�� ����
            _itemEffect._effectText = effectText;
            _itemEffect._isDoingTimeCheck = true;
            // EffectStart ȣ��
            _itemEffect.StartEffect();
            // ȿ���� ���
            _audio.Play();

            Destroy(other.gameObject);
        }
            
    }

    public void ResetItem()
    {
        _isGuideMissile = false;
        _isButterfly = false;
        _isBomb = false;
    }

}
