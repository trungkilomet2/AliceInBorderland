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
    const string MAXLEVEL = "Maxlevel";


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

        if (weaponData.nextLevel.Name.Equals(MAXLEVEL)) return;

        CommonUI commonUI = GetComponent<CommonUI>();

        if (commonUI != null)
        {
            commonUI.AddUpgradesIntoTheListOfAvailableUpgrades(weaponData.nextLevel);
        }
    }

    public void UnlockNextLevelSKill(WeaponData weaponData)
    {
        CommonUI commonUI = GetComponent<CommonUI>();

        if (commonUI != null)
        {
            commonUI.AddUpgradesIntoTheListOfAvailableUpgrades(weaponData.nextLevel);
        }

    }




}
