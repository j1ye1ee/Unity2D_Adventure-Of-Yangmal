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



    // 아이템과 충돌시 아이템 사용

    private void OnTriggerEnter2D(Collider2D other)
    {
        IPlayerItem item = other.GetComponent<IPlayerItem>();
        if (item != null)
        {
            item.UseItem();

           // 아이템 이펙트 텍스트 가져오기
           string effectText = other.GetComponent<Item>()._effectText;
            // ItemUseEffect에 전달
            _itemEffect._effectText = effectText;
            _itemEffect._isDoingTimeCheck = true;
            // EffectStart 호출
            _itemEffect.StartEffect();
            // 효과음 재생
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
