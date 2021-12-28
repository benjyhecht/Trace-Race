using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointColliderManager : MonoBehaviour
{
    LapManager lapManager;

    void Start()
    {
        lapManager = FindObjectOfType<LapManager>();
        lapManager.AddCollider(this);
    }

    public void SetMaterial(Material mat)
    {
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    public void OnTriggerEnter(Collider collider)
    {
        lapManager.CheckpointReached(this);
    }

    public void OnTriggerExit(Collider collider)
    {
        lapManager.CheckpointLeft(this);
    }
}
