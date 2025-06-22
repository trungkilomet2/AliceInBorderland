using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Skill3 : BossSkillBase
{
    public GameObject thunderBolt;
    private int numberOfBolts = 10; // Number of bolts to spawn
    private float skillZone = 10f; // Range within which bolts can be spawned

    protected override void Activate()
    {
        for (int i = 0; i < numberOfBolts; i++)
        {
            // Generate a random point within a circle around the target
            Vector2 randomCircle = Random.insideUnitCircle * skillZone;
            Vector3 spawnPosition = target.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

            GameObject bolt = Instantiate(thunderBolt, spawnPosition, Quaternion.identity);
            Destroy(bolt, skillDuration);
        }
    }

}
