using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;

    int lapNumber = 0;
    int currentCheckpoint = 0;
    CarController carController;
    LapTextUpdater lapTextUpdater;
    TextShrinker textShrinker;
    CheckpointColliderManager[] checkpoints = new CheckpointColliderManager[4];

    void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        lapTextUpdater = FindObjectOfType<LapTextUpdater>();
        textShrinker = FindObjectOfType<TextShrinker>();
    }

    public void AddCollider(CheckpointColliderManager collider)
    {
        collider.SetMaterial(redMat);
        Vector3 pos = collider.transform.position;
        if (pos.z >= 3 && pos.z < 5)
        {
            checkpoints[0] = collider;
            collider.SetMaterial(greenMat);
        }
        if (pos.x >= 3 && pos.x < 5)
        {
            checkpoints[1] = collider;
        }
        if (pos.z >= -5 && pos.z < -3)
        {
            checkpoints[2] = collider;
        }
        if (pos.x >= -5 && pos.x < -3)
        {
            checkpoints[3] = collider;
        }
    }

    public void CheckpointReached(CheckpointColliderManager collider)
    {
        if (System.Array.IndexOf(checkpoints, collider) == currentCheckpoint)
        {
            if (currentCheckpoint == 0)
            {
                lapNumber++;
                if (lapNumber == 1)
                {
                    lapTextUpdater.StartTimer();
                }
                if (lapNumber > 3)
                {
                    carController.SetDrivable(false);
                    lapTextUpdater.StopTimer();
                }
            }
            currentCheckpoint++;            
            if (currentCheckpoint >= 4)
            {
                currentCheckpoint = 0;
                
            }

            checkpoints[currentCheckpoint].SetMaterial(greenMat);
            if (currentCheckpoint == 0)
            {
                lapTextUpdater.updateText(lapNumber, checkpoints.Length);
            }
            else
            {
                lapTextUpdater.updateText(lapNumber, currentCheckpoint);
            }
            textShrinker.FadeText("Checkpoint reached!", 3);
        }
        else
        {
            textShrinker.FadeText("Wrong Checkpoint!", 3);
        }
    }

    public void CheckpointLeft(CheckpointColliderManager collider)
    {
        collider.SetMaterial(redMat);
    }
}
