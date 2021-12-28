using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadEndCollisionManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider carCollider)
    {
        GameObject car = carCollider.gameObject;
        car.GetComponent<CarController>().SetDrivable(false);
        GameStarter gameStarter = FindObjectOfType<GameStarter>();
        //gameStarter.SetText("Congratulations!");
    }
}
