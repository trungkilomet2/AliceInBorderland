using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponInventory : MonoBehaviour
{
    public WeaponData weaponData;

    private WeaponStats weaponStats;

    public float timeToAttack = 1f;
    float timer;

    private void Update()
    {
        
        timer -= Time.deltaTime;
        
        if(timer <= 0)
        {
            timer = timeToAttack;
            Attack();
        }
    }

    public virtual void setData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        timeToAttack = weaponData.timeToAttack;
    }

    public void Attack()
    {

    }


}
