using UnityEngine;

public class Sensor : MonoBehaviour
{
    public enum SensorType { StartSensor, DuplicationSensor };
    public SensorType sensorType;
    private GameObject parentCar;
    // Start is called before the first frame update
    void Awake()
    {
        parentCar = transform.parent.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (sensorType == SensorType.StartSensor)
        {
            if (collision.gameObject.name == "carSensor(Clone)" || collision.gameObject.tag == "Sensor")
            {
                parentCar.GetComponent<Car>().carState = Car.CarState.STOP;
            }
        }

        /*
        if (sensorType == SensorType.DuplicationSensor)
        {
            if (collision.gameObject.tag == "DuplicationSensor")
            {
                if (parentCar.GetComponent<Car>().isDuplication)
                {
                    Destroy(parentCar);
                }
            }
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sensorType == SensorType.StartSensor)
        {
            parentCar.GetComponent<Car>().carState = Car.CarState.SPEEDUP;
        }
    }
}