using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string Name;
    public GameObject animatedPrefab;
    public EnemyStats stats;

    public bool isRanged;
    public GameObject bulletPrefab;
    public float fireRate = 2f;
}
