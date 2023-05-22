using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discomport : MonoBehaviour
{
    [SerializeField]
    public float updateSecTotalPoint = 1.0f;
    [HideInInspector]
    public List<GameObject> carList = new List<GameObject>();
    [HideInInspector]
    public float totalDiscomportPoint = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeUpdate());
    }

    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            totalDiscomportPoint = 0.0f;

            for (int i = 0; i < carList.Count; i++)
            {
                if (carList[i] != null)
                {
                    totalDiscomportPoint += carList[i].GetComponent<Car>().discomfortPoint;
                }
                else
                {
                    carList.RemoveAt(i);
                }
            }

            // Debug.Log(totalDiscomportPoint.ToString());

            yield return new WaitForSeconds(updateSecTotalPoint);
        }
    }
}
