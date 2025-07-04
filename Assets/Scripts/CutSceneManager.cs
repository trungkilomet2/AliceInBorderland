using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutSceneManager : MonoBehaviour
{
    public GameObject creditCanvas;    // Gán Canvas Credit
    private List<GameObject> activeBeams = new List<GameObject>();

    public void OnBossDefeated()
    {
        // Tắt beam sáng
        foreach (GameObject beam in activeBeams)
        {
            if (beam != null)
                Destroy(beam);  // Xóa beam khỏi scene
        }
        SceneManager.LoadScene("Credit");
    }
    public void RegisterBeam(GameObject beam)
    {
        activeBeams.Add(beam);
    }
}

