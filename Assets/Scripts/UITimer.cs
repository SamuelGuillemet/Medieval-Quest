using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public TMPro.TMP_Text TimerText;
    public bool playing { get; set; }
    private float timer;

    private void Awake()
    {
        playing = true;
        timer = 0;
    }

    void Update()
    {
        if (playing == true)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);
            int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
            TimerText.SetText(
                minutes.ToString("00")
                    + ":"
                    + seconds.ToString("00")
                    + ":"
                    + milliseconds.ToString("00")
            );
        }
    }
}
