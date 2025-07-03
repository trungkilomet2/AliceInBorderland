using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
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

    private SkillBase[] skillBase;
    private GameObject player;
    bool isUpdateSkill = false;
    private EquipmentTooltip[] equipmentTooltips;


    private AudioManager audioManager;

    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        upgradePanelManager = FindAnyObjectByType<UpgradePanelManager>();
        UpdateWeaponCharacterByCharacterSelecting();
        FindEquipmentToolTip();
    }

    void UpdateWeaponCharacterByCharacterSelecting()
    {
        string characterName = PlayerPrefs.GetString("SelectedCharacter");
        StringBuilder sourcesSkillUpdate = new StringBuilder();
        sourcesSkillUpdate.Append("Data/Characters/");

        StringBuilder sourceSkillUpdateImage = new StringBuilder();
        sourceSkillUpdateImage.Append("SkillUI/");

        if (characterName == CharacterName.Archer.ToString())
        {
            sourcesSkillUpdate.Append("BowData");
            sourceSkillUpdateImage.Append("Archer/");
        }
        else if (characterName == CharacterName.Mage.ToString())
        {
            sourcesSkillUpdate.Append("BookData");
            sourceSkillUpdateImage.Append("Mage/");
        }
        else if (characterName == CharacterName.Warrior.ToString())
        {
            sourcesSkillUpdate.Append("HarmerData");
            sourceSkillUpdateImage.Append("Warrior/");
        }
        else if (characterName == CharacterName.Summoner.ToString())
        {
            sourcesSkillUpdate.Append("StaffData");
            sourceSkillUpdateImage.Append("Summoner/");
        }
        UpdateData data = Resources.Load<UpdateData>(sourcesSkillUpdate.ToString());
        upgradeData.Add(data);

        LoadImageSkillByCharacterSelect(sourceSkillUpdateImage);

    }

    void FindEquipmentToolTip()
    {
        List<EquipmentTooltip> result = new List<EquipmentTooltip>();
        equipmentTooltips = Resources.FindObjectsOfTypeAll<EquipmentTooltip>();

        foreach (EquipmentTooltip e in equipmentTooltips)
        {
            Debug.Log("1");
        }

    }

    void LoadImageSkillByCharacterSelect(StringBuilder sourceSkillUI)
    {
        for (int i = 1; i <= 4; i++)
        {
            StringBuilder imageSource = new StringBuilder();
            imageSource.Append(sourceSkillUI);
            string SkillNaming = "Skill" + i;
            Sprite skillNum = Resources.Load<Sprite>(imageSource.Append(SkillNaming).ToString());
            string skillBorderObjectNumber = "SkillBorder" + i;
            string skillImageNumber = "Skill" + i;
            string skillImageSource = skillBorderObjectNumber + "/" + skillImageNumber;
            Image imageSkill = GameObject.Find(skillImageSource).GetComponent<Image>();
            imageSkill.gameObject.SetActive(true);
            imageSkill.enabled = true;
            imageSkill.sprite = skillNum;
            GameObject borderObject;
            DeactiveSkillBorderObject(out borderObject, skillBorderObjectNumber);
        }
    }

    void DeactiveSkillBorderObject(out GameObject borderObject, string nameBorderObjectNumber)
    {
        borderObject = GameObject.Find(nameBorderObjectNumber);
        borderObject.SetActive(false);
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
        if (upgradeData.Count > 0)
        {
            upgradePanelManager.OpenPanel(selectUpdate);
        }
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

    void RemoveSkillUpdate()
    {
        for (int i = 0; i < upgradeData.Count; i++)
        {
            if (upgradeData[i].upgradeType == UpgradeType.SkillUpgrade)
            {
                upgradeData.Remove(upgradeData[i]);
            }
        }

    }

    void RemoveItemUnused()
    {
        for (int i = 0; i < upgradeData.Count; i++)
        {
            if (upgradeData[i].upgradeType == UpgradeType.WeaponUnlock)
            {
                upgradeData.Remove(upgradeData[i]);
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
            case UpgradeType.SkillUpgrade:
                weaponManager.UnlockNextLevelSKill(upgradeChoice.weaponData);
                SkillUpdateByLevel(upgradeChoice);
                RemoveLowTierWeapon(upgradeChoice.weaponData);
                acquireUpdate.Add(upgradeChoice);
                isUpdateSkill = true;
                break;
        }

        if (acquireUpdate.Count == 4)
        {
            if (isUpdateSkill)
            {
                RemoveItemUnused();
            }
            else
            {
                RemoveSkillUpdate();
            }
        }

        audioManager?.PlayChooseItemSound();
        upgradeData.Remove(upgradeChoice);
        LoadUpdateUI();
    }

    void SkillUpdateByLevel(UpdateData updateData)
    {
        player = GameObject.FindWithTag("Player");
        skillBase = player.GetComponents<SkillBase>();
        SkillNum numberOfSkill = SkillNum.Passive;

        if (updateData.Name.Split(" ")[1].Equals("I"))
        {
            numberOfSkill = SkillNum.Skill1;
            DisplaySkillUI(1);
        }
        else if (updateData.Name.Split(" ")[1].Equals("II"))
        {
            numberOfSkill = SkillNum.Skill2;
            DisplaySkillUI(2);
        }
        else if (updateData.Name.Split(" ")[1].Equals("III"))
        {
            numberOfSkill = SkillNum.Skill3;
            DisplaySkillUI(3);

        }
        else if (updateData.Name.Split(" ")[1].Equals("IV"))
        {
            numberOfSkill = SkillNum.Skill4;
            DisplaySkillUI(4);
        }

        foreach (SkillBase skill in skillBase)
        {

            if (numberOfSkill == skill.skillNum)
            {
                float cooldownTime;
                skill.UnlockSkillBySkillNum(numberOfSkill);
                cooldownTime = skill.GetCurrentCooldown();
                if (skill.GetIsCoolingDown())
                {
                    FindSkillCoolDown(numberOfSkill);
                }
            }
        }
    }
    void FindSkillCoolDown(SkillNum numberOfSkill)
    {
        if (numberOfSkill == SkillNum.Skill1)
        {
            CoolDownSkill1 coolDownSkill1 = FindAnyObjectByType<CoolDownSkill1>();
            coolDownSkill1.StartCooldown();
        }

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
            // equipmentTooltips[i].SetEquipmentInfo(updateData.weaponData.name.ToString());
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

    void DisplaySkillUI(int numberBorderSkill)
    {
        string numberSkillBorder = "SkillBorder" + numberBorderSkill;
        Transform skillUI = GameObject.Find("SkillUI").transform;
        Transform skillBorder = FindInactiveChild(skillUI, numberSkillBorder);
        skillBorder.gameObject.SetActive(true);
    }
    Transform FindInactiveChild(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child;
        }
        return null;
    }




}
