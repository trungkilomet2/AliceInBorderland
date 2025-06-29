using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Skill3_Warrior : SkillBase
{
    public GameObject baseWarrior;            // nhân vật hiện tại
    public GameObject transformedWarriorPrefab; // Prefab nhân vật mới
    public float transformDuration = 10f;

    public float cooldownTime = 5f;
    private float lastUsedTime = -Mathf.Infinity;

    private float transformEndTime = 0f;
    private bool isTransformed = false;
    private GameObject spawnedTransformedWarrior; // nhân vật mới đã được tạo
    private EnemiesManager enemiesManager;
    private CharacterCommonBehavior baseBehavior;
    private CharacterCommonBehavior form2Behavior;
    public SharedWarriorState sharedWarriorState;

    protected override void Activate()
    {
        if (sharedWarriorState != null && sharedWarriorState.hasUsedSkill4)
        {
            return;
        }
        else
        {
            // nếu đang cooldown → không cho dùng
        if (Time.time < lastUsedTime + cooldownTime) return;

        // bắt đầu cooldown
        lastUsedTime = Time.time;
        if (isTransformed || transformedWarriorPrefab == null || baseWarrior == null) return;

        // Spawn nhân vật mới
        spawnedTransformedWarrior = Instantiate(
            transformedWarriorPrefab,
            baseWarrior.transform.position,
            Quaternion.identity
        );

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(spawnedTransformedWarrior);
        }

        enemiesManager = FindObjectOfType<EnemiesManager>();
        if (enemiesManager != null)
        {
            enemiesManager.SetPlayer(spawnedTransformedWarrior);
        }

        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();

        if (vcam != null)
        {
            vcam.Follow = spawnedTransformedWarrior.transform;
            vcam.LookAt = spawnedTransformedWarrior.transform;
        }

        // Lấy hp hiện tại của form 1
        baseBehavior = baseWarrior.GetComponent<CharacterCommonBehavior>();
        form2Behavior = spawnedTransformedWarrior.GetComponent<CharacterCommonBehavior>();

        if (baseBehavior != null && form2Behavior != null)
        {
            form2Behavior.hp = baseBehavior.hp; // Gán lại máu
        }

        // Tắt nhân vật gốc

        Destroy(baseWarrior);
        transformEndTime = Time.time + transformDuration;
        isTransformed = true;
        }
        


    }

    void Update()
    {
        base.Update();
        if (isTransformed && Time.time >= transformEndTime)
        {
            // Quay về nhân vật gốc
            baseWarrior.transform.position = spawnedTransformedWarrior.transform.position;
            baseWarrior.SetActive(true);

            // Xoá nhân vật mới
            Destroy(spawnedTransformedWarrior);

            isTransformed = false;
        }
    }
}
