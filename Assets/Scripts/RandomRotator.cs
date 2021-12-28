using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(Vector3.down * .5f);
        transform.rotation = Random.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
