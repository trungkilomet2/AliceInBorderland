using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterName
{
    [EnumMember(Value = "Archer")]
    Archer,
    [EnumMember(Value = "Mage")]
    Mage,
    [EnumMember(Value = "Warrior")]
    Warrior,
    [EnumMember(Value = "Summoner")]
    Summoner
}

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
    public List<UpdateData> acquireItemUpdate;



    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Start()
    {
        upgradePanelManager = FindAnyObjectByType<UpgradePanelManager>();
    }

    void UpdateWeaponCharacterByCharacterSelecting()
    {
        string characterName = PlayerPrefs.GetString("SelectedCharacter");

        if (characterName == CharacterName.Archer.ToString())
        {

        }
        else if (characterName == CharacterName.Mage.ToString())
        {

        }
        else if (characterName == CharacterName.Warrior.ToString())
        {

        }
        else if (characterName == CharacterName.Summoner.ToString())
        {

        }

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


    public void RemoveLowTierWeapon(WeaponData wpData)
    {
        foreach (UpdateData data in acquireUpdate)
        {
            if (data.weaponData.WeaponName.Trim().Equals(wpData.WeaponName))
            {
                acquireUpdate.Remove(data);
                RemoveWeaponPrefabFromScene(data.weaponData);
                break;
            }
        }
    }

    public void RemoveLowTierItem(WeaponData itemData)
    {
        foreach (UpdateData data in acquireItemUpdate)
        {
            if (data.weaponData.WeaponName.Trim().Equals(itemData.WeaponName))
            {
                acquireItemUpdate.Remove(data);
                RemoveWeaponPrefabFromScene(data.weaponData);
                break;
            }
        }
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
                weaponManager.UpdateWeapon(upgradeChoice.weaponData);
                RemoveLowTierWeapon(upgradeChoice.weaponData);
                acquireUpdate.Add(upgradeChoice);
                break;
            case UpgradeType.ItemUpgrade:
                weaponManager.UpdateWeapon(upgradeChoice.weaponData);
                RemoveLowTierItem(upgradeChoice.weaponData);
                acquireItemUpdate.Add(upgradeChoice);
                break;
            case UpgradeType.WeaponUnlock:
                weaponManager.AddWeapon(upgradeChoice.weaponData);
                acquireUpdate.Add(upgradeChoice);
                break;
            case UpgradeType.ItemUnlock:
                weaponManager.AddWeapon(upgradeChoice.weaponData);
                acquireItemUpdate.Add(upgradeChoice);
                break;
        }

        upgradeData.Remove(upgradeChoice);
        LoadUpdateUI();
    }

    bool CheckDupliCateUpdateData(UpdateData data, List<UpdateData> listUpdate)
    {
        foreach (UpdateData updateData in listUpdate)
        {
            if (data == updateData) return true;
        }

        return false;
    }

    public void RemoveWeaponPrefabFromScene(WeaponData wpData)
    {
        if (wpData == null || wpData.weaponPrefabs == null) return;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == (wpData.weaponPrefabs.gameObject.name + "(Clone)"))
            {
                Destroy(obj);
            }
        }
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
            UpdateData updateData = upgradeData[Random.Range(0, upgradeData.Count)];
            if (CheckDupliCateUpdateData(updateData, listUpgrade))
            {
                i--;
                continue;
            }
            listUpgrade.Add(updateData);
        }
        return listUpgrade;
    }

    public void AddUpgradesIntoTheListOfAvailableUpgrades(UpdateData weaponStages)
    {
        this.upgradeData.Add(weaponStages);
    }

    private void LoadUpdateUI()
    {
        int i = 1;
        foreach (UpdateData updateWP in acquireUpdate)
        {
            string WPBD = "WPBD" + i;
            string WPUI = "WPUI/" + WPBD + "/Image";
            Image image = GameObject.Find(WPUI).GetComponent<Image>();
            image.gameObject.SetActive(true);
            image.enabled = true;
            image.sprite = updateWP.icon;
            i++;
        }
        i = 1;
        foreach (UpdateData updateItem in acquireItemUpdate)
        {
            string WPBD = "ItemBD" + i;
            string WPUI = "ItemUI/" + WPBD + "/Image";
            Image image = GameObject.Find(WPUI).GetComponent<Image>();
            image.gameObject.SetActive(true);
            image.enabled = true;
            image.sprite = updateItem.icon;
            i++;
        }

    }

}
