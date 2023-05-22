using PathCreation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum CarState { STOP, SPEEDUP, SPEEDDOWN };
    public CarState carState;
    private bool isSpeedDown = false;
    private bool isSpeedUP = false;
    public float SpeedUpTime = 5f;
    public float SpeedDownTime = 5f;
    private float SpeedTime = 0;

    public bool isDuplication = false;

    // Discomfort Point
    public float discomfortPoint;
    public float discomfortLevel = 2.0f;
    public bool visualization = false;
    private float elapsedTime = 0;

    private GameObject path;
    public float speed = 0f;
    public float maxSpeed = 1f;

    private float fTime = 0;
    private JamManager jamMgr;

    private Vector3 nowPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    private int posNumber = -1;

    [HideInInspector]
    public bool bIsStayZone = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0f;
        maxSpeed = 1f;

        carState = CarState.SPEEDUP;
        isSpeedDown = false;
        isSpeedUP = false;

        path = GameObject.Find("Path");
        jamMgr = GameObject.Find("GameManager").GetComponent<JamManager>();

        for (int i = 0; i < 16; i++)
        {
            if (transform.position == jamMgr.startPosition[i])
            {
                nowPos = jamMgr.startPosition[i];
                targetPos = jamMgr.endPosition[i];
                posNumber = i;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        fTime += Time.deltaTime * speed;
        elapsedTime += Time.deltaTime;

        // 생성된 직후 부터 불쾌지수 상승
        // StayZone을 벗어나면 불쾌지수 0으로 초기화.
        if (bIsStayZone)
            discomfortPoint = Mathf.Pow(elapsedTime, discomfortLevel);
        else
            discomfortPoint = 0.0f;

        OnDiscomfortColor(visualization);
        PathCreator line;
        Quaternion targetRotation;

        switch (carState)
        {
            case CarState.STOP:
                speed = 0f;
                break;
            case CarState.SPEEDDOWN:
                InStartSpeedDown();
                break;
            case CarState.SPEEDUP:
                InStartSpeedUP();
                break;
        }

        switch (posNumber)
        {
            case 0:
                line = path.transform.Find("S2WPath").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 1:
                line = path.transform.Find("S2NPath_01").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 2:
                line = path.transform.Find("S2NPath_02").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 3:
                line = path.transform.Find("S2NPath_03").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 4:
                line = path.transform.Find("E2SPath").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 5:
                line = path.transform.Find("E2WPath_01").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 6:
                line = path.transform.Find("E2WPath_02").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 7:
                line = path.transform.Find("E2WPath_03").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 8:
                line = path.transform.Find("N2EPath").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 9:
                line = path.transform.Find("N2SPath_01").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 10:
                line = path.transform.Find("N2SPath_02").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 11:
                line = path.transform.Find("N2SPath_03").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 12:
                line = path.transform.Find("W2NPath").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 13:
                line = path.transform.Find("W2EPath_01").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 14:
                line = path.transform.Find("W2EPath_02").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;
            case 15:
                line = path.transform.Find("W2EPath_03").gameObject.GetComponent<PathCreator>();
                transform.position = line.path.GetPointAtDistance(fTime);
                targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: line.path.GetDirectionAtDistance(fTime));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fTime);
                break;

        }
    }

    private void InStartSpeedDown()
    {
        speed = 0f;
        /*
        if (isSpeedDown)
        { return; }

        StartCoroutine("SpeedDown");
        */
    }

    private void InStartSpeedUP()
    {
        speed = maxSpeed;

        /*
        if (isSpeedUP)
        { return; }

        StartCoroutine("SpeedUp");
        */
    }
    /*
    IEnumerator SpeedUp()
    {
        isSpeedUP = true;
        SpeedTime = 0;

        while (speed < maxSpeed && carState == CarState.SPEEDUP)
        {
            SpeedTime += Time.deltaTime / SpeedUpTime;

            speed = Mathf.Lerp(speed, maxSpeed, SpeedTime);
            yield return null;
        }

        isSpeedUP = false;
    }

    IEnumerator SpeedDown()
    {
        isSpeedDown = true;
        SpeedTime = 0;

        while (0f < speed && carState == CarState.SPEEDDOWN)
        {
            SpeedTime += Time.deltaTime / SpeedDownTime;

            speed = Mathf.Lerp(speed, 0f, SpeedTime);
            yield return null;
        }

        isSpeedDown = false;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestoryZone")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "StayZone")
        {
            bIsStayZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StayZone")
        {
            bIsStayZone = false;
        }
    }

    private void OnDiscomfortColor(bool visualization)
    {
        if (visualization)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, (255.0f - discomfortPoint) / 255f, (255.0f - discomfortPoint) / 255f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
