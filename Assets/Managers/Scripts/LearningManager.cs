using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { SOUTH, EAST, NORTH, WEST, NONE};
public class LearningManager : MonoBehaviour
{
    private bool bIsValueUpdate = false;

    public float gamma = 0.9f;
    public float alpha = 0.1f;
    [HideInInspector]
    public Direction maxDirection = Direction.NONE;

    public void directionUpdate()
    {
        if (!bIsValueUpdate)
        {
            StartCoroutine(coDirectionUpdate());
        }
    }

    private IEnumerator coDirectionUpdate()
    {
        bIsValueUpdate = true;

        if (MaxDiscomportZone() != Direction.NONE)
        {
            maxDirection = MaxDiscomportZone();

            
            switch(MaxDiscomportZone())
            {
                case Direction.SOUTH:
                    Debug.Log("South");
                    break;
                case Direction.EAST:
                    Debug.Log("East");
                    break;
                case Direction.NORTH:
                    Debug.Log("North");
                    break;
                case Direction.WEST:
                    Debug.Log("West");
                    break;
            }
            
        }
        else
        {
            Debug.LogError("Direction value is NONE!!");
        }

        yield return new WaitForSeconds(GameObject.Find("GameManager").gameObject.GetComponent<TLSystemManager>().waitTime);
        bIsValueUpdate = false;
        StopCoroutine("coValueUpdate");
    }

    private Direction MaxDiscomportZone()
    {
        float south = GameObject.Find("CountZone").transform.Find("South").GetComponent<TrafficCount>().totalZoneDiscomport();
        float east = GameObject.Find("CountZone").transform.Find("East").GetComponent<TrafficCount>().totalZoneDiscomport();
        float north = GameObject.Find("CountZone").transform.Find("North").GetComponent<TrafficCount>().totalZoneDiscomport();
        float west = GameObject.Find("CountZone").transform.Find("West").GetComponent<TrafficCount>().totalZoneDiscomport();

        List<float> temp = new List<float>();

        temp.Add(south);
        temp.Add(east);
        temp.Add(north);
        temp.Add(west);

        switch (temp.IndexOf(temp.Max()))
        {
            case 0:
                return Direction.SOUTH;
            case 1:
                return Direction.EAST;
            case 2:
                return Direction.NORTH;
            case 3:
                return Direction.WEST;
        }

        return Direction.NONE;
    }
}
