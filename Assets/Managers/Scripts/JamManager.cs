using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamManager : MonoBehaviour
{
    public GameObject carPrefab;

    public float learningSpeed = 1.0f;
    public bool discomfortVisualization = false;

    /*
    // 10 / n 초 마다 생성
    // 랜덤치는 가우시안 분포를 따름.
    public int southJam = 10;
    public int eastJam = 2;
    public int northJam = 5;
    public int westJam = 1;
    */

    // 패턴마다 생성
    private TimePattern pattern;
    public int nowTime = 1;

    [HideInInspector]
    public int chartDate = 0;

    [HideInInspector]
    public int[] directionJamCount = new int[4];

    [HideInInspector]
    public Vector3[] startPosition = new Vector3[16];
    [HideInInspector]
    public Vector3[] endPosition = new Vector3[16];
    [HideInInspector]
    public Quaternion[] carRotation = new Quaternion[4];

    public void Start()
    {
        Time.timeScale = learningSpeed;

        carPrefab.GetComponent<Car>().visualization = discomfortVisualization;
        pattern = GetComponent<TimePattern>();

        // Right South
        startPosition[0] = new Vector3(1.25f, -40f, 0f);
        startPosition[1] = new Vector3(1.75f, -40f, 0f);
        startPosition[2] = new Vector3(2.25f, -40f, 0f);
        startPosition[3] = new Vector3(2.75f, -40f, 0f);
        // Right East
        startPosition[4] = new Vector3(40f, 2.25f, 0f);
        startPosition[5] = new Vector3(40f, 2.75f, 0f);
        startPosition[6] = new Vector3(40f, 3.25f, 0f);
        startPosition[7] = new Vector3(40f, 3.75f, 0f);
        // Right North
        startPosition[8] = new Vector3(0.75f, 40f, 0f);
        startPosition[9] = new Vector3(0.25f, 40f, 0f);
        startPosition[10] = new Vector3(-0.25f, 40f, 0f);
        startPosition[11] = new Vector3(-0.75f, 40f, 0f);
        // Right West
        startPosition[12] = new Vector3(-40f, 1.75f, 0f);
        startPosition[13] = new Vector3(-40f, 1.25f, 0f);
        startPosition[14] = new Vector3(-40f, 0.75f, 0f);
        startPosition[15] = new Vector3(-40f, 0.25f, 0f);

        // Right South
        endPosition[0] = new Vector3(-8f, 2.25f, 0f);
        endPosition[1] = new Vector3(1.75f, 11f, 0f);
        endPosition[2] = new Vector3(2.25f, 11f, 0f);
        endPosition[3] = new Vector3(2.75f, 11f, 0f);
        // Right East
        endPosition[4] = new Vector3(0.75f, -7f, 0f);
        endPosition[5] = new Vector3(-8f, 2.75f, 0f);
        endPosition[6] = new Vector3(-8f, 3.25f, 0f);
        endPosition[7] = new Vector3(-8f, 3.75f, 0f);
        // Right North
        endPosition[8] = new Vector3(10f, 1.75f, 0f);
        endPosition[9] = new Vector3(0.25f, -7f, 0f);
        endPosition[10] =new Vector3(-0.25f, -7f, 0f);
        endPosition[11] =new Vector3(-0.75f, -7f, 0f);
        // Right West
        endPosition[12] =new Vector3(1.25f, 11f, 0f);
        endPosition[13] =new Vector3(10f, 1.25f, 0f);
        endPosition[14] =new Vector3(10f, 0.75f, 0f);
        endPosition[15] =new Vector3(10f, 0.25f, 0f);

        carRotation[0] = Quaternion.Euler(0, 0, 0);
        carRotation[1] = Quaternion.Euler(0, 0, 90);
        carRotation[2] = Quaternion.Euler(0, 0, 180);
        carRotation[3] = Quaternion.Euler(0, 0, 270);

        StartCoroutine(TimeUpdate());
        StartCoroutine(CreateCar("South"));
        StartCoroutine(CreateCar("North"));
        StartCoroutine(CreateCar("East"));
        StartCoroutine(CreateCar("West"));

        for (int i = 0; i < 4; i++)
        {
            directionJamCount[i] = 0;
        }
    }

    private IEnumerator TimeUpdate()
    {
        Text uiTime = GameObject.Find("Canvas").transform.Find("TextTime").GetComponent<Text>();

        while (true)
        {
            uiTime.text = "Time: " + nowTime.ToString();

            yield return new WaitForSeconds(pattern.timePerSec);

            if (nowTime >= 10)
            {
                nowTime = 1;
                chartDate = -4000;
            }
            else
            {
                nowTime++;
                chartDate = -2000;
            }
        }
    }

    private IEnumerator CreateCar(string direction)
    {
        while(true)
        {
            int idx;
            GameObject obj;

            switch (direction)
            {
                case "South":
                    yield return new WaitForSeconds(RandomGaussian(0.0f, 10 / pattern.SouthPattern[nowTime - 1]));
                    idx = Random.Range(0, 4);

                    obj = Instantiate(carPrefab, startPosition[idx], carRotation[0]);
                    GetComponent<Discomport>().carList.Add(obj);
                    directionJamCount[0]++;
                    break;
                case "East":
                    yield return new WaitForSeconds(RandomGaussian(0.0f, 10 / pattern.EastPattern[nowTime - 1]));
                    idx = Random.Range(4, 8);

                    obj = Instantiate(carPrefab, startPosition[idx], carRotation[1]);
                    GetComponent<Discomport>().carList.Add(obj);
                    directionJamCount[1]++;
                    break;
                case "North":
                    yield return new WaitForSeconds(RandomGaussian(0.0f, 10 / pattern.NorthPattern[nowTime - 1]));
                    idx = Random.Range(8, 12);

                    obj = Instantiate(carPrefab, startPosition[idx], carRotation[2]);
                    GetComponent<Discomport>().carList.Add(obj);
                    directionJamCount[2]++;
                    break;
                case "West":
                    yield return new WaitForSeconds(RandomGaussian(0.0f, 10 / pattern.WestPattern[nowTime - 1]));
                    idx = Random.Range(12, 16);

                    obj = Instantiate(carPrefab, startPosition[idx], carRotation[3]);
                    GetComponent<Discomport>().carList.Add(obj);
                    directionJamCount[3]++;
                    break;

            }
        }
    }

    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while(S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }

    // 보험
    public static float GetRandomNormalDistribution(float mean, float standard)  // 정규 분포로 부터 랜덤값을 가져오는 함수 
    {
        var x1 = Random.Range(0f, 1f);
        var x2 = Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
