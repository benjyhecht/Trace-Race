using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearWheelSpinner : MonoBehaviour
{
    CarController carController = null;
    float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = transform.parent.gameObject;
        carController = parent.GetComponent<CarController>();
        int loopCount = 0;
        while (carController == null && loopCount < 15)
        {
            parent = parent.transform.parent.gameObject;
            carController = parent.GetComponent<CarController>();
            loopCount++;
        }
        if (loopCount == 15)
        {
            print("Loop count reached");
        }
    }

    void Update()
    {
        if (carController != null && !carController.GetHandBraking())
        {
            speed = carController.GetSpeed();
            transform.RotateAroundLocal(Vector3.right, speed * Time.deltaTime * 3);
        }
    }
}
