using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ETLSystemState { Counter, Clockwise, Random, Q };

public class TLSystemManager : MonoBehaviour
{
    public GameObject TLSouth;
    public GameObject TLEast;
    public GameObject TLNorth;
    public GameObject TLWest;

    private bool bIsStateUpdate = false;
    private bool bIsClearCenterZone = false;

    public ETLSystemState systemState;
    [HideInInspector]
    public int signalCount = 0;
    [HideInInspector]
    public float qValue = 0f;

    public float time;
    public float waitTime;
    private float fTime = 0.0f;
    private float fWaitTime = 0.0f;
    private int signalNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        signalNum = Random.Range(0, 4);
        bIsStateUpdate = false;
        bIsClearCenterZone = false;

        ChangeSignal(signalNum, "Go");
    }

    // Update is called once per frame
    void Update()
    {
        fTime += Time.deltaTime;

        switch (systemState)
        {
            case ETLSystemState.Counter:
                if (fTime > time)
                {
                    fWaitTime += Time.deltaTime;

                    if (fWaitTime < waitTime)
                    {
                        ChangeSignal(signalNum, "Wait");
                    }
                    else
                    {
                        fTime = 0.0f;
                        fWaitTime = 0.0f;
                        signalNum++;

                        if (signalNum > 3)
                            signalNum = 0;

                        ClearCenterZone();
                        ChangeSignal(signalNum, "Go");
                    }
                }
                break;
            case ETLSystemState.Clockwise:
                if (fTime > time)
                {
                    fWaitTime += Time.deltaTime;

                    if (fWaitTime < waitTime)
                    {
                        ChangeSignal(signalNum, "Wait");
                    }
                    else
                    {
                        fTime = 0.0f;
                        fWaitTime = 0.0f;
                        signalNum--;

                        if (signalNum < 0)
                            signalNum = 3;

                        ClearCenterZone();
                        ChangeSignal(signalNum, "Go");
                    }
                }
                break;

            case ETLSystemState.Random:
                if (fTime > time)
                {
                    fWaitTime += Time.deltaTime;

                    if (fWaitTime < waitTime)
                    {
                        ChangeSignal(signalNum, "Wait");
                    }
                    else
                    {
                        fTime = 0.0f;
                        fWaitTime = 0.0f;
                        signalNum = Random.Range(0, 4);

                        ClearCenterZone();
                        ChangeSignal(signalNum, "Go");
                    }
                }
                break;
            case ETLSystemState.Q:
                int nowTime = GameObject.Find("GameManager").gameObject.GetComponent<JamManager>().nowTime;

                if (fTime > time)
                {
                    fWaitTime += Time.deltaTime;

                    // 신호등이 노란불일 때
                    if (fWaitTime < waitTime)
                    {
                        stateUpdate();

                        // 1. 어디가 가장 불쾌지수가 높은지 방향 업데이트
                        GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().directionUpdate();

                        // 2. Q State Update
                        // stateUpdate();

                        ChangeSignal(signalNum, "Wait");
                    }
                    else
                    {
                        // Q State를 보고 value가 가장 작은 곳에 신호를 부여
                        fTime = 0.0f;
                        fWaitTime = 0.0f;

                        float[] temp = new float[4];

                        switch (GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().maxDirection)
                        {
                            case Direction.SOUTH:
                                for (int i = 0; i < 4; i++)
                                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, i];
                                break;
                            case Direction.EAST:
                                for (int i = 0; i < 4; i++)
                                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, i];
                                break;
                            case Direction.NORTH:
                                for (int i = 0; i < 4; i++)
                                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, i];
                                break;
                            case Direction.WEST:
                                for (int i = 0; i < 4; i++)
                                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, i];
                                break;
                        }

                        signalNum = MinState(temp);
                        // Debug.Log(signalNum);

                        ClearCenterZone();
                        ChangeSignal(signalNum, "Go");
                    }
                }
                break;
        }
    }

    private void TLInit(GameObject TL, string state)
    {
        switch (state)
        {
            case "Stop":
                TL.transform.Find("Red").gameObject.SetActive(true);
                TL.transform.Find("Yellow").gameObject.SetActive(false);
                TL.transform.Find("Green").gameObject.SetActive(false);
                TL.transform.Find("StopSensor").gameObject.SetActive(true);
                break;
            case "Wait":
                TL.transform.Find("Red").gameObject.SetActive(false);
                TL.transform.Find("Yellow").gameObject.SetActive(true);
                TL.transform.Find("Green").gameObject.SetActive(false);
                TL.transform.Find("StopSensor").gameObject.SetActive(true);
                break;
            case "Go":
                TL.transform.Find("Red").gameObject.SetActive(false);
                TL.transform.Find("Yellow").gameObject.SetActive(false);
                TL.transform.Find("Green").gameObject.SetActive(true);
                TL.transform.Find("StopSensor").gameObject.SetActive(false);
                break;
        }
    }

    private void ChangeSignal(int num, string state)
    {
        signalCount++;

        switch (num)
        {
            case 0:
                TLInit(TLSouth, state);
                TLInit(TLEast, "Stop");
                TLInit(TLNorth, "Stop");
                TLInit(TLWest, "Stop");
                break;
            case 1:
                TLInit(TLSouth, "Stop");
                TLInit(TLEast, state);
                TLInit(TLNorth, "Stop");
                TLInit(TLWest, "Stop");
                break;
            case 2:
                TLInit(TLSouth, "Stop");
                TLInit(TLEast, "Stop");
                TLInit(TLNorth, state);
                TLInit(TLWest, "Stop");
                break;
            case 3:
                TLInit(TLSouth, "Stop");
                TLInit(TLEast, "Stop");
                TLInit(TLNorth, "Stop");
                TLInit(TLWest, state);
                break;

        }
    }

    private int MinState(float[] array)
    {
        float min = array[0];
        float max = array[0];

        for (int i = 0; i < array.Length; i++)
        {
            if (min > array[i])
                min = array[i];
            else if (max < array[i])
                max = array[i];
        }

        List<int> temp = new List<int>();

        for (int i = 0; i < array.Length; i++)
        {
            if (min == array[i])
                temp.Add(i);
        }

        if (temp.Count > 1)
            return temp[Random.Range(0, temp.Count)];
        else
            return temp[0];
    }

    private float MinValue(float[] array)
    {
        float min = array[0];
        float max = array[0];

        for (int i = 0; i < array.Length; i++)
        {
            if (min > array[i])
                min = array[i];
            else if (max < array[i])
                max = array[i];
        }

        List<int> temp = new List<int>();

        for (int i = 0; i < array.Length; i++)
        {
            if (min == array[i])
                temp.Add(i);
        }

        if (temp.Count > 1)
        {
            return array[temp[Random.Range(0, temp.Count)]];
        }
        else
            return array[temp[0]];
    }

    private void stateUpdate()
    {
        if (!bIsStateUpdate)
        {
            StartCoroutine(coSateUpdate());
        }
    }

    private IEnumerator coSateUpdate()
    {
        bIsStateUpdate = true;

        int nowTime = GameObject.Find("GameManager").gameObject.GetComponent<JamManager>().nowTime;
        float discomportPoint = GameObject.Find("GameManager").gameObject.GetComponent<Discomport>().totalDiscomportPoint;
        float alpha = GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().alpha;

        float[] temp = new float[4];

        // 현재 신호를 준 방향의 Q state를 기준으로 미래(앞으로 줄 신호 방향)의 State를 고려해서 Q State 업데이트
        switch (GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().maxDirection)
        {
            case Direction.SOUTH:
                for (int i = 0; i < 4; i++)
                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, i];

                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, signalNum] +
                    alpha * (discomportPoint + GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma *
                    MinValue(temp));
                break;
            case Direction.EAST:
                for (int i = 0; i < 4; i++)
                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, i];

                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, signalNum] +
                    alpha * (discomportPoint + GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma *
                    MinValue(temp));
                break;
            case Direction.NORTH:
                for (int i = 0; i < 4; i++)
                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, i];

                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, signalNum] +
                    alpha * (discomportPoint + GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma *
                    MinValue(temp));
                break;
            case Direction.WEST:
                for (int i = 0; i < 4; i++)
                    temp[i] = GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, i];

                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, signalNum] +
                    alpha * (discomportPoint + GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma *
                    MinValue(temp));
                break;
        }

        /*
        switch (GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().maxDirection)
        {
            case Direction.SOUTH:
                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[(nowTime - 1) * 4, signalNum] +
                    alpha * GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma * discomportPoint;
                break;
            case Direction.EAST:
                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 1, signalNum] +
                    alpha * GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma * discomportPoint;
                break;
            case Direction.NORTH:
                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 2, signalNum] +
                    alpha * GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma * discomportPoint;
                break;
            case Direction.WEST:
                GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, signalNum] =
                    (1 - alpha) * GameObject.Find("GameManager").gameObject.GetComponent<State>().state[((nowTime - 1) * 4) + 3, signalNum] +
                    alpha * GameObject.Find("GameManager").gameObject.GetComponent<LearningManager>().gamma * discomportPoint;
                break;
        }
        */

        yield return new WaitForSeconds(GameObject.Find("GameManager").gameObject.GetComponent<TLSystemManager>().waitTime);
        bIsStateUpdate = false;
        StopCoroutine("coSateUpdate");
    }

    private void ClearCenterZone()
    {
        if (!bIsClearCenterZone)
        {
            StartCoroutine(coClearCenterZone());
        }
    }

    private IEnumerator coClearCenterZone()
    {
        bIsClearCenterZone = true;

        GameObject parent = GameObject.Find("PreventionZone").gameObject;
        GameObject zone = parent.transform.Find("Zone").gameObject;
        zone.SetActive(bIsClearCenterZone);

        yield return null;
        bIsClearCenterZone = false;
        zone.SetActive(bIsClearCenterZone);
        StopCoroutine("coClearCenterZone");
    }
}