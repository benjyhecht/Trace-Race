using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Control:")]
    [SerializeField] float maxSpeed = 1;
    [Range(0, 1)][SerializeField] float grassFactor = .5f;
    [Range(0,1)][SerializeField] float minAcceleration = 1;
    [SerializeField] float accelFactor = 60;
    [SerializeField] float rotationSpeed = 50;
    [SerializeField] float rotationRadius = 2.5f;

    [Header("Handbrake Control:")]
    [Range(0, 2)] [SerializeField] float kickOut = .625f;
    [Range(0, 1)] [SerializeField] float turningAmplifier = 0;
    [SerializeField] float recoverySpeed = 8;
    [SerializeField] float braking = 6;

    float speed = 0;
    float topSpeed;
    float minSpeed;
    float turnAngle = 0;
    float acceleration;
    float grassTime = 0;
    bool handBrake = false;
    bool drivable = false;

    void Start()
    {
        topSpeed = maxSpeed;
        minSpeed = topSpeed * -.2f;
    }

    void Update()
    {
        acceleration = minAcceleration + (1 - Mathf.Abs(speed) / maxSpeed) / 8;
        CheckGround();
        if (speed > topSpeed)
        {
            Accelerate(-2 - Mathf.Min(grassTime, 6));
        }
        else if (speed < minSpeed)
        {
            Accelerate(2);
        }
        if (Input.GetKey(KeyCode.W) && speed < topSpeed && !handBrake && drivable)
        {
            Accelerate(1);
        }
        else if (Input.GetKey(KeyCode.S) && speed > minSpeed && !handBrake && drivable)
        {
            Accelerate(-2);
        }
        else
        {
            if (!handBrake)
            {
                speed *= 100 / (100 + acceleration);
            }
            else
            {
                speed *= 100 / (100 + braking * acceleration);
            }
        }

        if (Input.GetKey(KeyCode.A) && drivable)
        {
            if (!handBrake)
            {
                transform.RotateAround(transform.position, Vector3.down, rotationSpeed * Time.deltaTime / Mathf.Max(1, Mathf.Abs(speed / rotationRadius)));
            }
            else
            {
                transform.RotateAround(transform.position, Vector3.down, rotationSpeed * Time.deltaTime / Mathf.Max(1, Mathf.Abs(speed / (rotationRadius + turningAmplifier))));
            }
            turnAngle -= Time.deltaTime * kickOut;
            if (turnAngle < -90 * Mathf.Deg2Rad)
            {
                turnAngle = -90 * Mathf.Deg2Rad;
            }
        }
        else if (Input.GetKey(KeyCode.D) && drivable)
        {
            if (!handBrake)
            {
                transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime / Mathf.Max(1, Mathf.Abs(speed / rotationRadius)));
            }
            else
            {
                transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime / Mathf.Max(1, Mathf.Abs(speed / (rotationRadius + turningAmplifier))));
            }
            turnAngle += Time.deltaTime * kickOut;
            if (turnAngle > 90 * Mathf.Deg2Rad)
            {
                turnAngle = 90 * Mathf.Deg2Rad;
            }
        }

        if (Input.GetKey(KeyCode.Space) && drivable)
        {
            handBrake = true;
        }
        else
        {
            handBrake = false;
        }
        transform.Translate(new Vector3(speed * Time.deltaTime * Mathf.Sin(turnAngle), 0, -speed * Time.deltaTime * Mathf.Cos(turnAngle)));

        if (!drivable)
        {
            if (speed > .1f)
            {
                Accelerate(-7);
            }
            else if (speed < -.1f)
            {
                Accelerate(7);
            }
            else
            {
                speed = 0;
            }
        }
    }

    public void LateUpdate()
    {
        if (!handBrake)
        {
            if (turnAngle != 0)
            {
                turnAngle = Mathf.Lerp(turnAngle, 0, Time.deltaTime * recoverySpeed / Mathf.Abs(turnAngle));  
            }
        }
    }

    private void Accelerate(float direction)
    {
        speed += acceleration * accelFactor * direction;
    }

    private void CheckGround()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position + new Vector3(0, .1f, 0), Vector3.down, out hitInfo, 10))
        {
            if (hitInfo.collider.gameObject.tag == "Asphalt")
            {
                topSpeed = maxSpeed;
                grassTime = 0;
            }
            else
            {
                topSpeed = maxSpeed * grassFactor;
                grassTime += Time.deltaTime * 30;
            }
            minSpeed = topSpeed * -.2f;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetDrivable(bool drivableIn)
    {
        drivable = drivableIn;
    }

    public float GetTurnAngle()
    {
        return turnAngle;
    }

    public bool GetHandBraking()
    {
        return handBrake;
    }
}
