using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVManager : MonoBehaviour
{
    [SerializeField] TextAsset playerSizeSetting;
    void Awake()
    {
        Variables.playerSizes = new List<PlayerSize>();
        ParseDatas(playerSizeSetting, (rowStrs, iy) =>
        {
            PlayerSize playerSize = new PlayerSize();
            if (int.TryParse(rowStrs[0], out int size))
            {
                playerSize.size = size;
            }
            if (int.TryParse(rowStrs[1], out int eatenCountToNextSize))
            {
                playerSize.eatenCountToNextSize = eatenCountToNextSize;
            }
            if (float.TryParse(rowStrs[2], out float cameraAperture))
            {
                playerSize.cameraAperture = cameraAperture;
            }
            Variables.playerSizes.Add(playerSize);
        });
    }

    void ParseDatas(TextAsset csv, Action<string[], int> Action)
    {
        List<string[]> strList = CsvToStrList(csv);
        for (int iy = 1; iy < strList.Count; iy++)
        {
            Action(strList[iy], iy);
        }
    }

    List<string[]> CsvToStrList(TextAsset csvFile)
    {
        var strList = new List<string[]>();
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            strList.Add(line.Split(',')); // , 区切りでリストに追加
        }
        return strList;
    }
}
