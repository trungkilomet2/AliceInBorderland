using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shared/WarriorState")]
public class SharedWarriorState : ScriptableObject
{
    public bool hasUsedSkill4 = false;
    public float skill3LastUsedTime = -Mathf.Infinity;

#if UNITY_EDITOR
    // Reset lại mỗi lần bấm Play trong Editor (tùy chọn)
    private void OnEnable()
    {
        hasUsedSkill4 = false;
        skill3LastUsedTime = -Mathf.Infinity;
    }
#endif
}