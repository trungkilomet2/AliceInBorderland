using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3_Warrior : SkillBase
{
    public GameObject baseWarrior;         // nhân vật hiện tại
    public GameObject transformedWarrior;  // nhân vật mới
    public float transformDuration = 10f;  // thời gian giữ dạng mới

    private float transformEndTime = 0f;
    private bool isTransformed = false;

    protected override void Activate()
    {
        if (isTransformed || transformedWarrior == null || baseWarrior == null) return;

        // Đổi form
        baseWarrior.SetActive(false);
        transformedWarrior.SetActive(true);
        transformedWarrior.transform.position = baseWarrior.transform.position;

        transformEndTime = Time.time + transformDuration;
        isTransformed = true;
    }

    void Update()
    {
        if (isTransformed && Time.time >= transformEndTime)
        {
            // Quay về dạng gốc
            transformedWarrior.SetActive(false);
            baseWarrior.SetActive(true);
            baseWarrior.transform.position = transformedWarrior.transform.position;

            isTransformed = false;
        }
    }
}
