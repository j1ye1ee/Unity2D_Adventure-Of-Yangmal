using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    // 플레이어의 총알 부모클래스



    public PlayerShooter _playerShooter;
    Enemy _enemy;
    Boss _boss;
    public float _damage;
    public bool _isSItem = false;
    public bool _isMissile = false;


    // isTrigger on
    public void OnTriggerEnter2D(Collider2D other)
    {
        // 0) 미사일인 경우 월드벽 충돌시에만 충돌 처리
        if(_isMissile)
            if(other.tag == "World")
                gameObject.GetComponent<GuideMissile>()._isCollision = true;

        // 1) 월드 벽 or 장애물에 충돌시 일반 총알 삭제
        if (other.tag == "World" || other.tag == "wall")
        {
            // SItem 아닐시 setActive(fasle)
            if (!_isSItem)
                gameObject.SetActive(false);
        }

        
        // 2) 총알끼리 충돌시 적의 총알삭제 & 플레이어 총알 삭제
        else if (other.gameObject.tag == "Enemy Bullet")
        {
            other.gameObject.SetActive(false);

            // SItem 아닐시 삭제
            if (!_isSItem)
                gameObject.SetActive(false);

            // 미사일일시 미사일 클래스의 _isCollision = true;
            else if (_isMissile)
                gameObject.GetComponent<GuideMissile>()._isCollision = true;

        }


        // 3) enemy 와 충돌&& 적이 stun 상태가 아닐시
        else if (other.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>()._curState != Enemy.eENEMY_STATE.STUN)
            {
                // enemy7의 경우 --> shake 도중에는 총알 튕겨내고 공격 효과가 나지 않는 이벤트 발생
                if (other.gameObject.GetComponent<Enemy>()._isEnemy7)
                {
                    // shake 코루틴 실행 중일 경우& SItem 아닐 경우
                    if (other.gameObject.GetComponent<Enemy7>()._isDoingShake && !_isSItem)
                    {
                        //  총알 튕기기 / 공격 효과 나지 않음
                        gameObject.GetComponent<Rigidbody2D>().AddForce(_playerShooter._shootDir * -30f, ForceMode2D.Impulse);
                        StartCoroutine(WaitSetActive());
                        return;
                    }
                }

                // 그 외의 경우 --> 적 클래스 캐싱
                _enemy = other.gameObject.GetComponent<Enemy>();

                // 적 hp 차감
                _enemy.EnemyGetDamage(_damage);

                // SItem 아닐시 삭제
                if (!_isSItem)
                    gameObject.SetActive(false);

                // 미사일일시 미사일 클래스의 _isCollision = true;
                else if (_isMissile)
                    gameObject.GetComponent<GuideMissile>()._isCollision = true;

            }

        }


        // 보스 충돌시 & 보스 스턴 아닐시
        else if (other.gameObject.GetComponent<Boss>() != null)
        {
            if (other.gameObject.GetComponent<Boss>()._curState != Boss.eBOSS_STATE.STUN)
            {
                _boss = other.gameObject.GetComponent<Boss>();
                _boss.BossGetDamage(_damage);

                // SItem 아닐시 삭제
                if (!_isSItem)
                    gameObject.SetActive(false);

                // 미사일일시 미사일 클래스의 _isCollision = true;
                else if (_isMissile)
                    gameObject.GetComponent<GuideMissile>()._isCollision = true;
            }

        }

    }//    public void OnTriggerEnter2D(Collider2D other)

    IEnumerator WaitSetActive()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
