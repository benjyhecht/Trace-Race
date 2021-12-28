using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLightManager : MonoBehaviour
{
    [SerializeField] Material glowMat;
    [SerializeField] Material normalMat;
    Color color;
    bool braking = false;
    Material[] glowingMats = new Material[2];
    Material[] normalMats = new Material[2];

    void Start()
    {
        glowingMats[0] = glowMat;
        glowingMats[1] = glowMat;
        normalMats[0] = normalMat;
        normalMats[1] = normalMat;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S))
        {
            if (!braking)
            {
                braking = true;
                BrakeLight(true);
            }
        }
        else
        {
            if (braking)
            {
                braking = false;
                BrakeLight(false);
            }
        }
    }

    public void BrakeLight(bool braking)
    {        
        if (braking)
        {
            GetComponent<MeshRenderer>().materials = glowingMats;
        }
        else
        {
            GetComponent<MeshRenderer>().materials = normalMats;
        }
    }
}
