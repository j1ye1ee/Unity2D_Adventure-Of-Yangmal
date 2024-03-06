using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_1Effect : MonoBehaviour
{
    // boss1_1 �� ����Ʈ ���� Ŭ����


    public Boss1_1 _boss;
    // ����Ʈ ���� �ð� ����
    public float _term;
    // ����Ʈ ���� �Ÿ�
    public float _distance;

    // effectCount = effectEndCount �� �Ǹ� ������ �� �ֵ���
    public int _effectCount;
    public int _effectEndCount;

    // pooling ���� ����
    Boss1_1EffectPoolManager _poolManager;

    public GameObject _effect;

    // ����Ʈ ���� ���� ���� / �밢������ ����
    public enum eDIRECTION
    {
        VERTICAL,
        DIAGONAL
    }

    eDIRECTION _curdirection = eDIRECTION.VERTICAL;

    
    // �� ���⺰ ����Ʈ�� ���� ����Ʈ ����
    public List<GameObject> _upRight = new List<GameObject>();
    public List<GameObject> _upLeft = new List<GameObject>();
    public List<GameObject> _downRight = new List<GameObject>();
    public List<GameObject> _downLeft = new List<GameObject>();
    public List<GameObject> _up = new List<GameObject>();
    public List<GameObject> _down = new List<GameObject>();
    public List<GameObject> _right = new List<GameObject>();
    public List<GameObject> _left = new List<GameObject>();

    // ����Ʈ ���� ��ġ�� allClear�� ���� ����Ʈ
    public List<List<GameObject>> _effectList = new List<List<GameObject>>();

    // ����Ʈ ���� �ڷ�ƾ �ѹ��� ������ ����
    public bool _isDoingDestroyEffect = false;

    // ���� �����϶��� ����Ʈ ������ ����
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


    // ����Ʈ ���� ���� ���� or �밢������ �����ϰ� ����
    void RandomEffect()
    {
       int _direction = Random.Range(-1, 1);

        // ���� 4����
        if (_direction == -1)
            _curdirection = eDIRECTION.VERTICAL;

        // �밢�� 4����
        else
            _curdirection = eDIRECTION.DIAGONAL;
    }


    // ����Ʈ ����
    public void StartEffect()
    {
        // 1)���� ���� ����
        RandomEffect();
        
        // 2)������ �������� ����Ʈ ����
       if (_curdirection == eDIRECTION.VERTICAL)
          {
              StartCoroutine(CrackEffect(new Vector2(0, 1))); // ����
              StartCoroutine(CrackEffect(new Vector2(1, 0))); // ������
              StartCoroutine(CrackEffect(new Vector2(0, -1))); // �Ʒ�
              StartCoroutine(CrackEffect(new Vector2(-1, 0))); // ����
           }

        else if (_curdirection == eDIRECTION.DIAGONAL)
            {
              StartCoroutine(CrackEffect(new Vector2(1, 1))); // ������ �� �밢
              StartCoroutine(CrackEffect(new Vector2(1, -1))); // ������ �Ʒ� �밢
              StartCoroutine(CrackEffect(new Vector2(-1, -1))); // ���� �Ʒ� �밢
              StartCoroutine(CrackEffect(new Vector2(-1, 1))); // ���� �� �밢
            }
    }





    // ����Ʈ ���� �ڷ�ƾ
    IEnumerator CrackEffect(Vector2 direction)
    {
        List<GameObject> effects = _down; // �ʱ�ȭ
        bool stop = false;

        // 1) direction ���� effect ���� List �����ϱ�
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

        // 2) ���⿡ ���� ���� ���� ����
        Vector2 distance = _distance * direction.normalized;

        // 3) position = ���� �߹ٴ� ��ġ�� �ʱ�ȭ 
        Vector2 position = new Vector2(transform.position.x, transform.position.y+GetComponent<Boss1_1>()._downPoint-3.5f);

        int count = 0;

        // 4) stop = true �ɶ����� ����Ʈ ���� �ݺ�
        while (!stop )
        {

            // ���� ��ġ = ���� ��ġ + ���� ���� ��ŭ�� ���Ѵ�. 
            Vector2 InstantiatePosition = position + distance;

            // effect pool ���� �뿩 & ����Ʈ�� ���ϱ�
            GameObject effect =_poolManager.GetObject(
                    _poolManager._boss1_1EftPool,
                    _poolManager._boss1_1Eft,
                    InstantiatePosition,
                    transform.rotation);


            // �ش�Ǵ� ������ effects ����Ʈ�� �߰�
            effects.Add(effect);

            // ���� ��ġ = ������ ������ ����Ʈ ��ġ�� ����
            position = effects[count].transform.position;

            // ������ ������ ��ġ�� ����ĳ��Ʈ �߻� --> ���� �ִ��� Ȯ��
            RaycastHit2D hitdata;
            hitdata = Physics2D.Raycast(position, distance.normalized, 2f, LayerMask.GetMask("World"));

            // ���� �ִٸ� ����Ʈ ������ �����
            if (hitdata.collider != null)
                stop = true;

            // ���� ���ٸ� �ݺ��� ���
            else
            {
                yield return new WaitForSeconds(_term);
                count++;
            }
        }

    
        // 5) ������ ����Ʈ�� ����Ʈ����Ʈ�� ����
        _effectList.Add(effects);
        _effectCount++;
    }




    // STROLL ���¿����� ����Ʈ ����
    public IEnumerator DestroyEffect(List<List<GameObject>> effectList)
    {
        // 1)����Ʈ ���� ����Ʈ ã��
        for (int listCount = 0; listCount < effectList.Count; listCount++)
        {
            // 2)����Ʈ �� ���� ��� ���� �ı�
            for(int count = 0; count < effectList[listCount].Count; count++)
            {
                effectList[listCount][effectList[listCount].Count - 1 - count].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.05f);
            }
            // 3)���� �� ����Ʈ Ŭ����
            effectList[listCount].Clear();
        }

        // 3.5) ����Ʈ Ŭ���� �ɶ����� ��ٷȴٰ� ���� ����
        yield return new WaitUntil
            (() => effectList[0] == null);


        // 4) effectCount �ʱ�ȭ & effectList �ʱ�ȭ
        _effectCount = 0;
        effectList.Clear();
        _isDoingDestroyEffect = false;
        _allEffectList.Clear();

    }


    // DIE ���¿����� ����Ʈ ����
    public void DestroyEffect_DIE()
    {
        // 1) ��� �ڷ�ƾ ����
        StopAllCoroutines();

        // 2) allEffect ����Ʈ ���� ��� �ı�
        for(int count = 0; count<8; count ++)
            {
                for (int num = 0; num<_allEffectList[count].Count; num++)
                {
                    _allEffectList[count][_allEffectList[count].Count - 1 - num].gameObject.SetActive(false);
                 }
                _allEffectList[count].Clear();
             }
         // 3) allEffectList Ŭ����
         _allEffectList.Clear();
    }


}
