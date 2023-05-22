using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCount : MonoBehaviour
{
    private List<GameObject> cars = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "carSensor(Clone)")
        {
            cars.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "carSensor(Clone)")
        {
            cars.Remove(collision.gameObject);
        }
    }

    public float totalZoneDiscomport()
    {
        float total = 0f;

        for (int i = 0; i < cars.Count; i++)
        {
            total += cars[i].GetComponent<Car>().discomfortPoint;
        }

        return total;
    }
}
