using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCarController : MonoBehaviour
{
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddRelativeForce(Vector3.back * 50 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddRelativeForce(Vector3.forward * 50 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddRelativeTorque(Vector3.down * 15 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddRelativeTorque(Vector3.up * 15 * Time.deltaTime);
        }
    }
}
