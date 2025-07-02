using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Skill4 : BossSkillBase
{
    public GameObject J_Boss;
    public float timeRelease = 5f;
    protected override void Activate()
    {
        float offsetRange = 3f;
        Vector3 randomOffset = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            Random.Range(-offsetRange, offsetRange),
            0f
        );

        Vector3 spawnPosition = transform.position + randomOffset;

        J_Boss bossPrefab = J_Boss.GetComponent<J_Boss>();
        J_Boss bossSpawned = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        bossSpawned.SetTarget(target);
        bossSpawned.isClone = true;
    }

}
