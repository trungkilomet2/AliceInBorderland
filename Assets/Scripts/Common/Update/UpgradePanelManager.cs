using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;
    PauseManager pauseManager;
    public List<UpgradeButton> upgradeButtons;
    private CommonUI commonUI;

    private void Start()
    {   
        commonUI  = FindAnyObjectByType<CommonUI>();
        HideButtons();
    }
    public GameObject GetChecking()
    {
        return upgradePanel;
    }
    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
    }


    public void OpenPanel(List<UpdateData> updateDatas)
    {
        Clean();
        pauseManager.PauseGame();
        upgradePanel.SetActive(true);

        for (int i = 0; i < updateDatas.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].SetUpgrade(updateDatas[i]);
        }

    }

    public void Upgrade(int pressUpgradeNumber)
    {
        commonUI.UpgradeAfterUpLevel(pressUpgradeNumber);
        ClosePanel();
    }

    public void ClosePanel()
    {
        HideButtons();
        pauseManager.UnPauseGame();
        upgradePanel.SetActive(false);

    }

    private void HideButtons()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        for(int i = 0; i< upgradeButtons.Count;i++)
        {
            upgradeButtons[i].Clean(); 
        }

    }

}
