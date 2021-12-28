using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTextUpdater : MonoBehaviour
{
    Text text;
    bool timed = false;
    float seconds = 0;
    string secondString;
    float minutes = 0;
    string minuteString;
    string lapText;

    void Start()
    {
        text = GetComponent<Text>();
        text.fontSize = 32 * Screen.height / 1024;
        updateText(0, 0);
        print(Screen.height);
    }

    private void Update()
    {
        if (timed)
        {
            seconds += Time.deltaTime;
            minutes = seconds / 60;
            if (seconds % 60 < 10)
            {
                secondString = "0" + (seconds % 60).ToString("F2");
            }
            else
            {
                secondString = (seconds % 60).ToString("F2");
            }
            minuteString = ((int)minutes % 60).ToString();
            if (minutes >= 1)
            {
                text.text = (lapText + "\n" + minuteString + ":" + secondString).ToString();
            }
            else
            {
                text.text = (lapText + "\n" + secondString).ToString();
            }

        }
    }

    public void updateText(int lap, int cp)
    {
        lapText = "Lap: " + lap + "\n CP: " + cp;
        text.text = lapText.ToString();
    }

    public void StartTimer()
    {
        timed = true;
    }

    public void StopTimer()
    {
        timed = false;
    }
}
