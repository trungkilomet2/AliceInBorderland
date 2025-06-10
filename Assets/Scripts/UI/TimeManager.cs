using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI time_counter;
    [SerializeField] int maxTimeInSeconds = 1800; // 30 minitues
    private float currentTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;
        currentTime += Time.deltaTime;

        if (currentTime >= maxTimeInSeconds)
        {
            currentTime = maxTimeInSeconds;
            isRunning = false;
            // Spawn Last Boss -- Joker
        }
        CountTimer();
    }


    void CountTimer()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        time_counter.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}



