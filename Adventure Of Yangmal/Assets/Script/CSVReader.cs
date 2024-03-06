using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public List<string> _dataList = new List<string>();

    // CSV파일 이름으로 CSV 파일 읽어오기
    public void ReadCSV(string fileName)
    {
        // 리스트 초기화
        _dataList.Clear();

        //StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + fileName + ".csv");
        TextAsset source = Resources.Load<TextAsset>("itemList");
        StringReader sr = new StringReader(source.text);

        bool _isEnd = false;

        // CSV 한 줄 = data
        while(!_isEnd)
        {
            string data = sr.ReadLine();

            if(data == null)
            {
                _isEnd = true;
                break;
            }

            // 리스트에 한 줄씩 저장
            _dataList.Add(data);
        }
    }
}
