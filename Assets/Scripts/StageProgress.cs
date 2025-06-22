using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageProgress : MonoBehaviour
{
    CommonUI commonUI;

    void Awake()
    {
        commonUI = FindObjectOfType<CommonUI>();
    }

    [SerializeField] float progressTimeRate = 30f;
    [SerializeField] float progressPerSplit = 0.2f;

    public float Progress
    {
        get
        {
            return 1f + commonUI.currentTime / progressTimeRate * progressPerSplit;
        }
    }
}
