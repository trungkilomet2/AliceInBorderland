using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    WeaponUpgrade,
    ItemUpgrade,
    WeaponUnlock,
    ItemUnlock
}
public class UpdateData : MonoBehaviour
{

    public UpgradeType upgradeType;
    public string Name;
    public Sprite icon;



}

