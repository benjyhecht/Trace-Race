using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    GameObject car;
    AudioSource audioSource;
    TextShrinker textShrinker;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        car = GameObject.FindGameObjectWithTag("Player");
        car.GetComponent<CarController>().SetDrivable(false);
        textShrinker = GetComponent<TextShrinker>();
        string[] textArray = new string[] { "3", "2", "1", "Go!" };
        audioSource.pitch = 3.5f;
        audioSource.Play();
        StartCoroutine(ShrinkNumber(textArray, 0));
    }

    IEnumerator ShrinkNumber(string[] array, int index)
    {
        float timeTaken = 0;
        textShrinker.ShrinkText(array[index], 1);
        while (timeTaken < 1)
        {
            yield return new WaitForEndOfFrame();
            timeTaken += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        if (index == 2)
        {
            car.GetComponent<CarController>().SetDrivable(true);
            audioSource.pitch = 1f;
            audioSource.Play();
        }
        else
        {
            audioSource.Play();
            StartCoroutine(ShrinkNumber(array, index + 1));
        }
    }
}
