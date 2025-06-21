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

    }

}
