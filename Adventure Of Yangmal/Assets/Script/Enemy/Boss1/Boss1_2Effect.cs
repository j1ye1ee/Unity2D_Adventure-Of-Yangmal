using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_2Effect : MonoBehaviour
{
    //  Boss1_2의 EFFECT 관리 클래스


    public Boss1_2 _boss;

    // effect 생성 관련 변수
    public float _term;
    public float _distance;
    public float _downPoint;

    // 풀링 관련 변수
    Boss1_2EffectPoolManager _poolManager;

    public bool _isDoingEffect = false;

    // DIE 시 이펙트 삭제를 위한 리스트
    public List<GameObject> _allEffect = new List<GameObject>();

    // FOLLOW 상태 진입시 이펙트 삭제를 위한 리스트
    public List<List<GameObject>> _effectList = new List<List<GameObject>>();
    public bool _isDoingDestroyEffect = false;



    void Awake()
    {
        _boss = GetComponent<Boss1_2>();
    }

    private void Start()
    {
        _poolManager = GameObject.FindWithTag("boss1_2EffectPool").GetComponent<Boss1_2EffectPoolManager>();
    }

    // iSTATE == DIE ,  이펙트 전부 삭제
    public void AllDestroy()
    {
        for(int count = 0; count < _allEffect.Count; count++)
        {
            _allEffect[_allEffect.Count - 1 - count].gameObject.SetActive(false);
        }
        _allEffect.Clear();

        // 작업 종료 후 isAllDestroy = true  변경
        _boss. _isAllDestroy = true;
    }


    // 이펙트 생성 코루틴
    public IEnumerator CrackEffect()
    {

        // 1) 이펙트 방향은 아래 수직으로만 생성
        List<GameObject> down = new List<GameObject>();
        int count = 0;
        bool stop = false;

        Vector2 distance = _distance * Vector2.down.normalized;
        Vector2 position = new Vector2(transform.position.x, transform.position.y + _downPoint);

        // 2) stop = true 될때까지 반복하여 이펙트 생성
        while(!stop)
        {
            // 생성 위치 = 현재 위치 + 생성 간격 만큼을 더한다.
            Vector2 InstancePosition = position + distance;

            // effect pool 에서 대여 & 리스트에 더하기
            GameObject effect = _poolManager.GetObject(
                _poolManager._boss1_2EftPool,
                _poolManager._boss1_2Eft,
                InstancePosition,
                transform.rotation);

            down.Add(effect);
            _allEffect.Add(effect);

            // 현재 위치 = 이전에 생성된 이펙트 위치로 갱신
            position = down[count].transform.position;

            // 앞으로 생성할 위치에 레이캐스트 발사 -- > 벽이 있는지 확인
            RaycastHit2D hitData;
            hitData = Physics2D.Raycast(position, distance.normalized, 2f, LayerMask.GetMask("World"));

            // 벽이 있을시 = 이펙트 생성 중지
            if (hitData.collider != null)
                stop = true;

            // 벽이 없을시 = 이펙트 계속 생성
            else
            {
                yield return new WaitForSeconds(_term);
                count++;

            }
        }

        // 3) 생성한 이펙트리스트 DOWN 을 effectList에 저장
        _effectList.Add(down);
    }


    // FOLLOW 상태에서의 이펙트 삭제
    public IEnumerator DestroyEffect(List<List<GameObject>> effectList)
    {
        // 코루틴 한 번만 실행을 위함
        _isDoingDestroyEffect = true;

        // 1) 리스트 내부 리스트 찾기
        for (int listCount = 0; listCount < effectList.Count; listCount++)
        {
            //2) 리스트 별 내부 요소 전부 삭제
            for (int count = 0; count < effectList[listCount].Count; count++)
            {
                effectList[listCount][effectList[listCount].Count - 1 - count].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.05f);
            }
            effectList[listCount].Clear();
        }
        _isDoingDestroyEffect = false;
    }
}
