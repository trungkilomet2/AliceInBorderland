using System.Collections;
using System.Collections.Generic;
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
        pauseManager.PauseGame();
        upgradePanel.SetActive(true);
        for (int i = 0; i < updateDatas.Count; i++)
        {
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
        pauseManager.UnPauseGame();
        upgradePanel.SetActive(false);

    }

}
