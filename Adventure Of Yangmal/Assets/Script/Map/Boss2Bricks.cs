using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bricks : MonoBehaviour
{
    public enum eBRICKSTATE
    {
        BEFORE_PAGE2,
        AFTER_PAGE2
    }

    public eBRICKSTATE _eState = eBRICKSTATE.BEFORE_PAGE2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(_eState)
        {
            case eBRICKSTATE.BEFORE_PAGE2:
                if(other.gameObject.tag == "Player Bullet" )
                {
                    if (other.gameObject.GetComponent<GuideMissile>() != null)
                        other.gameObject.GetComponent<GuideMissile>().MissileSetActiveFasle();
                    else
                        other.gameObject.SetActive(false);
                }
                break;
            case eBRICKSTATE.AFTER_PAGE2:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bomb")
        {
            collision.gameObject.GetComponent<Bomb>().BombSetActiveFasle();
        }
        
    }

}
