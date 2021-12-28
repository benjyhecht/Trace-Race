using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundManager : MonoBehaviour
{
    [SerializeField] float minVol;
    [SerializeField] float minPitch;
    [SerializeField] float maxVol;
    [SerializeField] float maxPitch;

    AudioSource audioSource;
    CarController carController;
    float speed;
    float maxSpeed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        maxSpeed = carController.GetMaxSpeed();
    }

    void Update()
    {
        speed = Mathf.Abs(carController.GetSpeed());
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
        audioSource.volume = Mathf.Lerp(minVol, maxVol, speed / maxSpeed);
    }
}
