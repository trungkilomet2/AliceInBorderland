using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Skill4 : BossSkillBase
{
    public GameObject J_Boss;
    public float timeRelease = 5f;
    protected override void Activate()
    {
        float range = 20f;
        Vector3 randomPosition = new Vector3(
            Random.Range(-range, range),
            Random.Range(-range, range),
            0f
        );

        J_Boss bossPrefab = J_Boss.GetComponent<J_Boss>();
        J_Boss bossSpawned = Instantiate(bossPrefab, randomPosition, Quaternion.identity);
        bossSpawned.SetTarget(target);
        bossSpawned.isClone = true;
    }
}
