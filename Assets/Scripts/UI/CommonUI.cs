using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CommonUI : MonoBehaviour
{
    //EXP Bar
    public Image imageEXP;
    private float currentExp;
    private float maxExp;
    public int currentLevel = 1;
    public TextMeshProUGUI levelText;

    // Image Bar
    public Image imageHP;
    private float currentHP;
    private float maxHP;

    // TimeCounter
    public TextMeshProUGUI timerCounter;
    private int maxTimeInSeconds = 1800;
    public float currentTime = 0f;
    private bool isRunning = true;

    private void Update()
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

    public void CountTimer()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetCurrentHp(float hp)
    {
        this.currentHP = hp;
    }
    public void SetMaxHp(float hp)
    {
        this.maxHP = hp;
    }
    public void UpdateHealthBar()
    {
        if (imageHP != null)
            imageHP.fillAmount = currentHP / maxHP;
    }

    public void AddExp(float amount)
    {
        currentExp += amount;
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }
        UpdateExpBar();
    } 
    public void LevelUp()
    {
        currentLevel++;
        maxExp *= 1.1f;
    }
    private void UpdateExpBar()
    {
        if (imageEXP != null)
            imageEXP.fillAmount = currentExp / maxExp;
    }
    public void SetExp(float current, float max)
    {
        currentExp = current;
        maxExp = max;
        UpdateExpBar();
    }

}
