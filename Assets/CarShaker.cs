using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShaker : MonoBehaviour
{
    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != originalPosition)
        {
            transform.localPosition = originalPosition;
        }
        else
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere / 200;
        }
    }
}
