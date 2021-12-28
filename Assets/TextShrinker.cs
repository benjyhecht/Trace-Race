using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShrinker : MonoBehaviour
{
    Text text;
    string textIn;
    float timeIn;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    public void ShrinkText(string textIn, float time)
    {
        StopAllCoroutines();
        this.textIn = textIn;
        timeIn = time;
        StartCoroutine(ShrinkText(textIn, time, 0));
    }

    private IEnumerator ShrinkText (string textIn, float time, int number)
    {
        text.color = new Color(0, 0, 1, 1);
        text.fontSize = 132;
        text.text = textIn;
        float timeTaken = 0;
        while (timeTaken < time)
        {
            text.fontSize = (int)Mathf.Lerp(132, 0, timeTaken / time);
            yield return new WaitForEndOfFrame();
            timeTaken += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        text.text = "";
        text.fontSize = 0;        
    }

    public void FadeText(string textIn, float time)
    {
        StopAllCoroutines();
        this.textIn = textIn;
        timeIn = time;
        StartCoroutine(FadeText(textIn, time, 0));
    }

    private IEnumerator FadeText(string textIn, float time, int number)
    {
        text.color = new Color(0, 0, 1, 1);
        text.fontSize = 132;
        text.text = textIn;
        float timeTaken = 0;
        while (timeTaken < time)
        {
            Color clear = new Color(0, 0, 1, 1 - timeTaken / time);
            text.color = clear;
            yield return new WaitForEndOfFrame();
            timeTaken += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        text.text = "";
    }
}
