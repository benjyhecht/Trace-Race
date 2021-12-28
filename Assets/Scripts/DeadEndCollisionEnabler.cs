using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEndCollisionEnabler : MonoBehaviour
{
    [SerializeField] GameObject endingCollider;

    public void SetColliderOn()
    {
        endingCollider.SetActive(true);
    }
}
