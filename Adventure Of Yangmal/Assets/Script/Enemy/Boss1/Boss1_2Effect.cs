using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_2Effect : MonoBehaviour
{
    //  Boss1_2�� EFFECT ���� Ŭ����


    public Boss1_2 _boss;

    // effect ���� ���� ����
    public float _term;
    public float _distance;
    public float _downPoint;

    // Ǯ�� ���� ����
    Boss1_2EffectPoolManager _poolManager;

    public bool _isDoingEffect = false;

    // DIE �� ����Ʈ ������ ���� ����Ʈ
    public List<GameObject> _allEffect = new List<GameObject>();

    // FOLLOW ���� ���Խ� ����Ʈ ������ ���� ����Ʈ
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

    // iSTATE == DIE ,  ����Ʈ ���� ����
    public void AllDestroy()
    {
        for(int count = 0; count < _allEffect.Count; count++)
        {
            _allEffect[_allEffect.Count - 1 - count].gameObject.SetActive(false);
        }
        _allEffect.Clear();

        // �۾� ���� �� isAllDestroy = true  ����
        _boss. _isAllDestroy = true;
    }


    // ����Ʈ ���� �ڷ�ƾ
    public IEnumerator CrackEffect()
    {

        // 1) ����Ʈ ������ �Ʒ� �������θ� ����
        List<GameObject> down = new List<GameObject>();
        int count = 0;
        bool stop = false;

        Vector2 distance = _distance * Vector2.down.normalized;
        Vector2 position = new Vector2(transform.position.x, transform.position.y + _downPoint);

        // 2) stop = true �ɶ����� �ݺ��Ͽ� ����Ʈ ����
        while(!stop)
        {
            // ���� ��ġ = ���� ��ġ + ���� ���� ��ŭ�� ���Ѵ�.
            Vector2 InstancePosition = position + distance;

            // effect pool ���� �뿩 & ����Ʈ�� ���ϱ�
            GameObject effect = _poolManager.GetObject(
                _poolManager._boss1_2EftPool,
                _poolManager._boss1_2Eft,
                InstancePosition,
                transform.rotation);

            down.Add(effect);
            _allEffect.Add(effect);

            // ���� ��ġ = ������ ������ ����Ʈ ��ġ�� ����
            position = down[count].transform.position;

            // ������ ������ ��ġ�� ����ĳ��Ʈ �߻� -- > ���� �ִ��� Ȯ��
            RaycastHit2D hitData;
            hitData = Physics2D.Raycast(position, distance.normalized, 2f, LayerMask.GetMask("World"));

            // ���� ������ = ����Ʈ ���� ����
            if (hitData.collider != null)
                stop = true;

            // ���� ������ = ����Ʈ ��� ����
            else
            {
                yield return new WaitForSeconds(_term);
                count++;

            }
        }

        // 3) ������ ����Ʈ����Ʈ DOWN �� effectList�� ����
        _effectList.Add(down);
    }


    // FOLLOW ���¿����� ����Ʈ ����
    public IEnumerator DestroyEffect(List<List<GameObject>> effectList)
    {
        // �ڷ�ƾ �� ���� ������ ����
        _isDoingDestroyEffect = true;

        // 1) ����Ʈ ���� ����Ʈ ã��
        for (int listCount = 0; listCount < effectList.Count; listCount++)
        {
            //2) ����Ʈ �� ���� ��� ���� ����
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
