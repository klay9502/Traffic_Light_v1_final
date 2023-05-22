using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePattern : MonoBehaviour
{
    // 시간 단위를 몇 초로 할 지 결정
    public float timePerSec = 60.0f;

    /*
    [HideInInspector]
    public float[] SouthPattern = {
        1, 1, 1, 2, 3, 5, 10, 10, 8, 5,
        6, 7, 4, 5, 4, 4, 5, 10, 10, 8,
        5, 3, 2, 1
    };

    [HideInInspector]
    public float[] EastPattern = {
        1, 1, 1, 1, 2, 3, 3, 3, 2, 1,
        2, 2, 2, 3, 2, 2, 3, 3, 3, 3,
        2, 2, 1, 1
    };

    [HideInInspector]
    public float[] NorthPattern = {
        8, 5, 3, 3, 2, 1, 1, 1, 1, 2,
        2, 2, 3, 3, 2, 2, 1, 1, 1, 1,
        2, 4, 5, 7
    };

    [HideInInspector]
    public float[] WestPattern = {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 2,
        3, 4, 7, 8, 7, 5, 3, 2, 1, 1,
        1, 1, 1, 1
    };
    */

    [HideInInspector]
    public float[] SouthPattern;
    [HideInInspector]
    public float[] EastPattern;
    [HideInInspector]
    public float[] NorthPattern;
    [HideInInspector]
    public float[] WestPattern;

    public void Start()
    {
        SouthPattern = new float[] {
           1, 2, 2, 2, 3,
           3, 3, 2, 1, 1
        };

        EastPattern = new float[] {
            5, 5, 4, 3, 4,
            5, 5, 5, 4, 3
        };

        NorthPattern = new float[] {
            4, 3, 2, 1, 2,
            2, 3, 4, 5, 5
        };

        WestPattern = new float[] {
            1, 1, 1, 1, 1,
            1, 1, 2, 2, 1
        };
    }
}
