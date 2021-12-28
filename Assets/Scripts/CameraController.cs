using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float rotationMultiplier = 1f;
    [SerializeField] float damping = 1f;
    Quaternion cameraMiddlePosition;
    CarController carController;
    float turnAngle;

    // Start is called before the first frame update
    void Start()
    {
        cameraMiddlePosition = transform.localRotation;
        carController = GetComponentInParent<CarController>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LateUpdate()
    {
        turnAngle = -carController.GetTurnAngle() * rotationMultiplier;
        transform.localRotation = new Quaternion(cameraMiddlePosition.x, turnAngle, cameraMiddlePosition.z, cameraMiddlePosition.w);
    }
}
