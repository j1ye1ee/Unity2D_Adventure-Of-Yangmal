using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public List<string> _dataList = new List<string>();

    // CSV���� �̸����� CSV ���� �о����
    public void ReadCSV(string fileName)
    {
        // ����Ʈ �ʱ�ȭ
        _dataList.Clear();

        //StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + fileName + ".csv");
        TextAsset source = Resources.Load<TextAsset>("itemList");
        StringReader sr = new StringReader(source.text);

        bool _isEnd = false;

        // CSV �� �� = data
        while(!_isEnd)
        {
            string data = sr.ReadLine();

            if(data == null)
            {
                _isEnd = true;
                break;
            }

            // ����Ʈ�� �� �پ� ����
            _dataList.Add(data);
        }
    }
}
