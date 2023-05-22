using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JamChart : MonoBehaviour
{
    public GraphChart chart;
    public GraphChart directionChart;
    public GraphChart discomfortChart;

    private AnalysisZone countZone;
    private float timer = 0.2f;
    private float X = 4f;

    private JamManager jamMgr;
    private Discomport discomport;

    private float sumDiscomportPoint = 0.0f;
    private int count = 0;
    private int nowTime;

    // CSV Part
    private List<string[]> WriteRowData = new List<string[]>();

    // Start is called before the first frame update
    void Start()
    {
        countZone = GameObject.Find("AnalysisZone").GetComponent<AnalysisZone>();
        jamMgr = GameObject.Find("GameManager").GetComponent<JamManager>();
        discomport = GameObject.Find("GameManager").GetComponent<Discomport>();

        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("Cars");
        chart.DataSource.EndBatch();

        directionChart.DataSource.StartBatch();
        directionChart.DataSource.ClearCategory("South");
        directionChart.DataSource.ClearCategory("East");
        directionChart.DataSource.ClearCategory("North");
        directionChart.DataSource.ClearCategory("West");
        directionChart.DataSource.EndBatch();

        discomfortChart.DataSource.StartBatch();
        discomfortChart.DataSource.ClearCategory("Total");
        discomfortChart.DataSource.EndBatch();

        sumDiscomportPoint = 0f;
        nowTime = GameObject.Find("GameManager").gameObject.GetComponent<JamManager>().nowTime;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            /*
            timer = 0.2f;
            chart.DataSource.AddPointToCategoryRealtime("Cars", X, countZone.count);

            directionChart.DataSource.AddPointToCategoryRealtime("South", X, jamMgr.directionJamCount[0]);
            directionChart.DataSource.AddPointToCategoryRealtime("East", X, jamMgr.directionJamCount[1]);
            directionChart.DataSource.AddPointToCategoryRealtime("North", X, jamMgr.directionJamCount[2]);
            directionChart.DataSource.AddPointToCategoryRealtime("West", X, jamMgr.directionJamCount[3]);

            discomfortChart.DataSource.AddPointToCategoryRealtime("Total", X, discomport.totalDiscomportPoint);
            string[] rowDataTemp = new string[2];
            rowDataTemp[0] = countZone.count.ToString();
            // rowDataTemp[0] = discomport.totalDiscomportPoint.ToString();
            rowDataTemp[1] = GameObject.Find("GameManager").GetComponent<JamManager>().chartDate.ToString();
            GameObject.Find("GameManager").GetComponent<JamManager>().chartDate = 0;
            WriteRowData.Add(rowDataTemp);
            WriteCsv(WriteRowData, Application.streamingAssetsPath + @"\" + "trafficFlow.csv");
            X++;
            */
            sumDiscomportPoint += discomport.totalDiscomportPoint;
            count++;
        }

        if (nowTime != GameObject.Find("GameManager").gameObject.GetComponent<JamManager>().nowTime)
        {
            // writeTimer ´ç Æò±Õ°ª ±¸ÇÔ.
            nowTime = GameObject.Find("GameManager").gameObject.GetComponent<JamManager>().nowTime;

            string[] rowDataTemp = new string[1];

            float tempTotal = sumDiscomportPoint / count;

            rowDataTemp[0] = tempTotal.ToString();  // ºÒÄè Áö¼ö ÃÑÇÕ
            WriteRowData.Add(rowDataTemp);
            WriteCsv(WriteRowData, Application.streamingAssetsPath + @"\" + "DiscomportChart.csv");

            sumDiscomportPoint = 0f;
            count = 0;

            Debug.Log("### WriteData ###");
        }
    }

    private void WriteCsv(List<string[]> rowData, string filePath)
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder stringBuilder = new StringBuilder();

        for (int index = 0; index < length; index++)
            stringBuilder.AppendLine(string.Join(delimiter, output[index]));

        Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.WriteLine(stringBuilder);
        outStream.Close();
    }
}
