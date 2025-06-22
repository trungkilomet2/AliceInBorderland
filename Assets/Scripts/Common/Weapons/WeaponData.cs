using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    [SerializeField] public int damage;
    [SerializeField] public float timeToAttack;
    public GameObject weaponPrefabs;
    public List<UpdateData> weaponStages;



}
