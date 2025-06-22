using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    [SerializeField] StageData stageData;
    [SerializeField] EnemiesManager enemiesManager;
    CommonUI commonUI;
    int eventIndex;

    private void Awake()
    {
        commonUI = FindObjectOfType<CommonUI>();
    }

    private void Update()
    {
        if (stageData?.stageEvents == null || eventIndex >= stageData.stageEvents.Count)
        {
            return;
        }

        StageEvent currentEvent = stageData.stageEvents[eventIndex];

        if (commonUI.currentTime >= currentEvent.time)
        {
            Debug.Log($"Event triggered: {currentEvent.message} at time {commonUI.currentTime:F1}");

            if (currentEvent.enemyToSpawn != null)
            {
                for (int i = 0; i < currentEvent.count; i++)
                {
                    enemiesManager.SpawnEnemy(currentEvent.enemyToSpawn);
                }
            }
            else
            {
                Debug.LogWarning($"No enemy prefab assigned for event: {currentEvent.message}");
            }

            eventIndex++;
        }
    }
}
