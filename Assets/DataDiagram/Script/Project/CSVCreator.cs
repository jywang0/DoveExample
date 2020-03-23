using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class CSVCreator : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();
    private string filepath;
    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.dataPath+"/";
        DateTime NowTime = DateTime.Now.ToLocalTime();
        string text = NowTime.ToString("ddMMyyHHmmss");
        filepath += "run" + text + ".csv";
        Debug.Log(filepath);

        string[] row = new string[3];
        row[0] = "Foods";
        row[1] = "Doves";
        row[2] = "Hawks";
        rowData.Add(row);
    }

    // Update is called once per frame
    public void Save(int dove, int hawk, int food)
    {
        Debug.Log("save" + 111);
        string[] row = new string[3];
        row[0] = "" + food; // name
        row[1] = "" + dove; // ID
        row[2] = "" + hawk; // Income
        rowData.Add(row);
    }

    public void Finished()
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = filepath;

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

}
