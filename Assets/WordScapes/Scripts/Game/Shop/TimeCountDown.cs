using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCountDown : MonoBehaviour
{
    public TextMeshProUGUI textTime;

    private DateTime endTime;
    private TimeSpan remainingTime;


    private void Start()
    {
        UpdateEndTime();
    }

    private void Update()
    {
        remainingTime = endTime - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            textTime.text = $"{remainingTime.Hours}:{remainingTime.Minutes}";
        }
        else
        {
            UpdateEndTime();
        }
    }

    private void UpdateEndTime()
    {
        endTime = DateTime.Now.Date.AddDays(1);
    }
}
