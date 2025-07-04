using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumnName;
    [SerializeField] private string textIntro;
    private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumnName) * 100;
      //  Debug.Log(volumnName + ":" + volumeValue);
        txt.text = textIntro + volumeValue.ToString();
    }
}
