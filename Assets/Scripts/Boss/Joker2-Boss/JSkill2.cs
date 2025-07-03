using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSkill2 : BossSkillBase
{
    public GameObject balloonPrefab;
    public float balloonRadius = 3f;
    public float balloonDuration = 2f;

    private void Awake()
    {
        cooldown = 20f; 
    }

    protected override void Activate()
    {
        StartCoroutine(SummonBalloons());
    }

    IEnumerator SummonBalloons()
    {
        Vector3 bossPos = transform.position;
        int count = 5;
        List<GameObject> balloons = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * balloonRadius;
            GameObject balloon = Instantiate(balloonPrefab, bossPos, Quaternion.identity);
            balloon.GetComponent<BalloonFollow>().Init(target.transform, offset, angle);
            balloons.Add(balloon);
        }

        yield return new WaitForSeconds(balloonDuration);

        foreach (var balloon in balloons)
        {
            if (balloon != null)
                balloon.GetComponent<BalloonFollow>().Explode();
        }
    }
}