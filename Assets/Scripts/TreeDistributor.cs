using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDistributor : MonoBehaviour
{
    [SerializeField] GameObject[] trees;

    void Start()
    {
        transform.rotation = Quaternion.identity;
        int index = Random.Range(0, trees.Length);
        GameObject tree = Instantiate(trees[index], Vector3.zero, Quaternion.identity, gameObject.transform);
        tree.transform.localPosition = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        tree.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        float scale = Random.Range(.3f, .6f);
        tree.transform.localScale = new Vector3(scale, scale, scale);
    }
}
