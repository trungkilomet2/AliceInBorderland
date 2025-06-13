using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CommonUI : MonoBehaviour
{
    public Image imageEXP;
    private float currentExp;
    private float maxExp;
    public int currentLevel = 1;
    public TextMeshProUGUI levelText;
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
