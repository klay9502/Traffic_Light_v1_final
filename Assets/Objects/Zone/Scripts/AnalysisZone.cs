using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalysisZone : MonoBehaviour
{
    [HideInInspector]
    public int count = 0;
    public GameObject obj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "carSensor(Clone)")
        {
            count++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "carSensor(Clone)")
        {
            count--;
        }
    }
}
