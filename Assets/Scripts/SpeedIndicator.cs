using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour
{
    CarController carController;
    Image image;

    void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        float scale = carController.GetSpeed() / carController.GetMaxSpeed();
        if (scale < 0)
        {
            scale = 0;
        }
        transform.localScale = new Vector2(1, scale);
        image.color = new Color(scale * scale, 1 - scale * scale, 0);
    }
}
