using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    GameObject parent;

    void Start()
    {
        parent = transform.parent.gameObject;
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        transform.SetParent(parent.transform);
    }
}
