using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1Effect : MonoBehaviour
{
    // boss1_1 의 이펙트 관리 클래스


    public Boss1_1 _boss;
    // 이펙트 생성 시간 간격
    public float _term;
    // 이펙트 생성 거리
    public float _distance;

    // effectCount = effectEndCount 가 되면 종료할 수 있도록
    public int _effectCount;
    public int _effectEndCount;

    // pooling 관련 변수
    Boss1_1EffectPoolManager _poolManager;

    public GameObject _effect;

    // 이펙트 생성 방향 수직 / 대각선으로 결정
    public enum eDIRECTION
    {
        VERTICAL,
        DIAGONAL
    }

    eDIRECTION _curdirection = eDIRECTION.VERTICAL;

    
    // 각 방향별 이펙트를 담을 리스트 선언
    public List<GameObject> _upRight = new List<GameObject>();
    public List<GameObject> _upLeft = new List<GameObject>();
    public List<GameObject> _downRight = new List<GameObject>();
    public List<GameObject> _downLeft = new List<GameObject>();
    public List<GameObject> _up = new List<GameObject>();
    public List<GameObject> _down = new List<GameObject>();
    public List<GameObject> _right = new List<GameObject>();
    public List<GameObject> _left = new List<GameObject>();

    // 이펙트 생성 마치고 allClear를 위한 리스트
    public List<List<GameObject>> _effectList = new List<List<GameObject>>();

    // 이펙트 삭제 코루틴 한번만 실행을 위함
    public bool _isDoingDestroyEffect = false;

    // 죽음 상태일때의 이펙트 삭제를 위함
    public bool _isAllDestroy = false;
    public List<List<GameObject>> _allEffectList = new List<List<GameObject>>();




    void Start()
    {
        _effectEndCount = 4;
        _boss = GetComponent<Boss1_1>();
        _allEffectList.Add(_upRight);
        _allEffectList.Add(_upLeft);
        _allEffectList.Add(_downRight);
        _allEffectList.Add(_downLeft);
        _allEffectList.Add(_down);
        _allEffectList.Add(_up);
        _allEffectList.Add(_right);
        _allEffectList.Add(_left);

        _poolManager = GameObject.FindWithTag("boss1_1EffectPool").GetComponent<Boss1_1EffectPoolManager>();
    }


    // 이펙트 생성 방향 수직 or 대각선으로 랜덤하게 설정
    void RandomEffect()
    {
       int _direction = Random.Range(-1, 1);

        // 수직 4방향
        if (_direction == -1)
            _curdirection = eDIRECTION.VERTICAL;

        // 대각선 4방향
        else
            _curdirection = eDIRECTION.DIAGONAL;
    }


    // 이펙트 시작
    public void StartEffect()
    {
        // 1)생성 방향 설정
        RandomEffect();
        
        // 2)설정된 방향으로 이펙트 생성
       if (_curdirection == eDIRECTION.VERTICAL)
          {
              StartCoroutine(CrackEffect(new Vector2(0, 1))); // 위쪽
              StartCoroutine(CrackEffect(new Vector2(1, 0))); // 오른쪽
              StartCoroutine(CrackEffect(new Vector2(0, -1))); // 아래
              StartCoroutine(CrackEffect(new Vector2(-1, 0))); // 왼쪽
           }

        else if (_curdirection == eDIRECTION.DIAGONAL)
            {
              StartCoroutine(CrackEffect(new Vector2(1, 1))); // 오른쪽 위 대각
              StartCoroutine(CrackEffect(new Vector2(1, -1))); // 오른쪽 아래 대각
              StartCoroutine(CrackEffect(new Vector2(-1, -1))); // 왼쪽 아래 대각
              StartCoroutine(CrackEffect(new Vector2(-1, 1))); // 왼쪽 위 대각
            }
    }





    // 이펙트 생성 코루틴
    IEnumerator CrackEffect(Vector2 direction)
    {
        List<GameObject> effects = _down; // 초기화
        bool stop = false;

        // 1) direction 별로 effect 넣을 List 지정하기
        if(direction.x == 0)
        {
            if (direction.y == -1)
                effects = _down;
            else if (direction.y == 1)
                effects = _up;
        }

        else if(direction.x == 1)
        {
            if (direction.y == 0)
                effects = _right;

            else if (direction.y == 1)
                effects = _upRight;

            else if (direction.y == -1)
                effects = _downRight;
        }

        else if(direction.x == -1)
        {
            if (direction.y == 0)
                effects = _left;

            else if (direction.y == 1)
                effects = _upLeft;

            else if (direction.y == -1)
                effects = _downLeft;
        }

        // 2) 방향에 따라 생성 간격 설정
        Vector2 distance = _distance * direction.normalized;

        // 3) position = 현재 발바닥 위치로 초기화 
        Vector2 position = new Vector2(transform.position.x, transform.position.y+GetComponent<Boss1_1>()._downPoint-3.5f);

        int count = 0;

        // 4) stop = true 될때까지 이펙트 생성 반복
        while (!stop )
        {

            // 생성 위치 = 현재 위치 + 생성 간격 만큼을 더한다. 
            Vector2 InstantiatePosition = position + distance;

            // effect pool 에서 대여 & 리스트에 더하기
            GameObject effect =_poolManager.GetObject(
                    _poolManager._boss1_1EftPool,
                    _poolManager._boss1_1Eft,
                    InstantiatePosition,
                    transform.rotation);


            // 해당되는 방향의 effects 리스트에 추가
            effects.Add(effect);

            // 현재 위치 = 직전에 생성된 이펙트 위치로 갱신
            position = effects[count].transform.position;

            // 앞으로 생성할 위치에 레이캐스트 발사 --> 벽이 있는지 확인
            RaycastHit2D hitdata;
            hitdata = Physics2D.Raycast(position, distance.normalized, 2f, LayerMask.GetMask("World"));

            // 벽이 있다면 이펙트 생성을 멈춘다
            if (hitdata.collider != null)
                stop = true;

            // 벽이 없다면 반복문 계속
            else
            {
                yield return new WaitForSeconds(_term);
                count++;
            }
        }

    
        // 5) 생성된 리스트를 이펙트리스트에 저장
        _effectList.Add(effects);
        _effectCount++;
    }




    // STROLL 상태에서의 이펙트 삭제
    public IEnumerator DestroyEffect(List<List<GameObject>> effectList)
    {
        // 1)리스트 내부 리스트 찾기
        for (int listCount = 0; listCount < effectList.Count; listCount++)
        {
            // 2)리스트 별 내부 요소 전부 파괴
            for(int count = 0; count < effectList[listCount].Count; count++)
            {
                effectList[listCount][effectList[listCount].Count - 1 - count].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.05f);
            }
            // 3)삭제 후 리스트 클리어
            effectList[listCount].Clear();
        }

        // 3.5) 리스트 클리어 될때까지 기다렸다가 다음 진행
        yield return new WaitUntil
            (() => effectList[0] == null);


        // 4) effectCount 초기화 & effectList 초기화
        _effectCount = 0;
        effectList.Clear();
        _isDoingDestroyEffect = false;
        _allEffectList.Clear();

    }


    // DIE 상태에서의 이펙트 삭제
    public void DestroyEffect_DIE()
    {
        // 1) 모든 코루틴 중지
        StopAllCoroutines();

        // 2) allEffect 리스트 내부 요소 파괴
        for(int count = 0; count<8; count ++)
            {
                for (int num = 0; num<_allEffectList[count].Count; num++)
                {
                    _allEffectList[count][_allEffectList[count].Count - 1 - num].gameObject.SetActive(false);
                 }
                _allEffectList[count].Clear();
             }
         // 3) allEffectList 클리어
         _allEffectList.Clear();
    }


}
