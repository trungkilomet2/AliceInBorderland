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

    [SerializeField] float progressTimeRate = 10f;
    [SerializeField] float progressPerSplit = 0.5f;

    public float Progress
    {
        get
        {
            return 1f + commonUI.currentTime / progressTimeRate * progressPerSplit;
        }
    }
}
