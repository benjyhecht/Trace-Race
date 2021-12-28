using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheelTurner : MonoBehaviour
{
    float currentRotation = 0;
    float maxAngle = 30;
    float minAngle = -30;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (currentRotation > minAngle)
            {
                currentRotation += -Time.deltaTime * 100;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (currentRotation < maxAngle)
            {
                currentRotation += Time.deltaTime * 100;
            }
        }
        else
        {
            if (currentRotation > 1)
            {
                currentRotation += -Time.deltaTime * 100;
            }
            else if (currentRotation < 0)
            {
                currentRotation += Time.deltaTime * 100;
            }
            else
            {
                currentRotation = 0;
            }
        }
        currentRotation = Mathf.Clamp(currentRotation, minAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
    }

    private IEnumerator RotateWheel(float angle)
    {
        float startAngle = transform.localRotation.y;
        float totalTime = Mathf.Abs(startAngle - angle) / 100;
        float timeTaken = 0;
        while (timeTaken < totalTime)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(startAngle, angle, timeTaken / totalTime), 0));
            timeTaken += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localRotation = Quaternion.Euler(new Vector3(0, angle, 0)); ;
        yield return new WaitForEndOfFrame();
    }
}
