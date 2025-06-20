using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;
    PauseManager pauseManager;
    public List<UpgradeButton> upgradeButtons;
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
        Debug.Log("Upgrade + " + pressUpgradeNumber);
    }

    public void ClosePanel()
    {
        pauseManager.UnPauseGame();
        upgradePanel.SetActive(false);

    }

}
