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

    //Upgrade

    [SerializeField] List<UpdateData> upgradeData; // Luu cac upgrade assest
    private UpgradePanelManager upgradePanelManager;
    private List<UpdateData> selectUpdate;
    public List<UpdateData> acquireUpdate;

    private WeaponManager weaponManager;

    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Start()
    {
        upgradePanelManager = FindAnyObjectByType<UpgradePanelManager>();
    }

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
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
            Debug.Log("Da up level");
        }
        UpdateExpBar();
    }
    public void LevelUp()
    {
        if (selectUpdate == null) { selectUpdate = new List<UpdateData>(); }
        selectUpdate.Clear();
        selectUpdate.AddRange(GetRandomUpdatesInUpgradeData(4));
        currentLevel++;
        maxExp *= 1.1f;
        upgradePanelManager.OpenPanel(selectUpdate);
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

    public void UpgradeAfterUpLevel(int numberOfChoice)
    {
        UpdateData upgradeChoice = selectUpdate[numberOfChoice];
        if (acquireUpdate == null)
        {
            acquireUpdate = new List<UpdateData>();
        }

        switch (upgradeChoice.upgradeType)
        {
            case UpgradeType.WeaponUpgrade:
                break;
            case UpgradeType.ItemUpgrade:
                break;
            case UpgradeType.WeaponUnlock:
                weaponManager.AddWeapon(upgradeChoice.weaponData);
                break;
            case UpgradeType.ItemUnlock:
                break;
        }

        acquireUpdate.Add(upgradeChoice);
        upgradeData.Remove(upgradeChoice);
    }

    public List<UpdateData> GetRandomUpdatesInUpgradeData(int count)
    {
        List<UpdateData> listUpgrade = new List<UpdateData>();

        if (count > upgradeData.Count)
        {
            count = upgradeData.Count;
        }

        for (int i = 0; i < count; i++)
        {
            listUpgrade.Add(upgradeData[Random.Range(0, upgradeData.Count)]);
        }

        return listUpgrade;
    }

}
