using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpdater : MonoBehaviour
{
    CarController carController;
    Text text;

    void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = (int) (carController.GetSpeed() * 21) + " UPH";
    }
}
