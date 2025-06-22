using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //// public Transform weaponObjectsContainer;

    // public WeaponData startingWeapon;

    // private void Start()
    // {
    //     AddWeapon(startingWeapon);
    // }

    public void AddWeapon(WeaponData weaponData)
    {
        GameObject weaponObject = Instantiate(weaponData.weaponPrefabs);

        CommonUI commonUI = GetComponent<CommonUI>();

        if (commonUI != null)
        {
            commonUI.AddUpgradesIntoTheListOfAvailableUpgrades(weaponData.nextLevel);
        }
    }

    public void UpdateWeapon(WeaponData weaponData)
    {
        GameObject weaponObject = Instantiate(weaponData.weaponPrefabs);

        CommonUI commonUI = GetComponent<CommonUI>();

        if (commonUI != null)
        {
            commonUI.AddUpgradesIntoTheListOfAvailableUpgrades(weaponData.nextLevel);
        }


    }

    public void RemoveLowTierWeapon(WeaponData wpData,ref List<UpdateData> aquireList)
    {
        foreach (UpdateData data in aquireList)
        {
            if (data.weaponData.WeaponName.Equals(wpData.WeaponName))
            {
                aquireList.Remove(data);
                break;
            }
        }

        

    }

}
