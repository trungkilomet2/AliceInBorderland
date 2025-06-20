using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Image icon;

    public void SetUpgrade(UpdateData updateData)
    {
        icon.sprite = updateData.icon;
    }

}
