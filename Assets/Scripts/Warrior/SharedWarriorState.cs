using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shared/WarriorState")]
public class SharedWarriorState : ScriptableObject
{
    public bool hasUsedSkill4 = false;

#if UNITY_EDITOR
    // Reset lại mỗi lần bấm Play trong Editor (tùy chọn)
    private void OnEnable()
    {
        hasUsedSkill4 = false;
    }
#endif
}