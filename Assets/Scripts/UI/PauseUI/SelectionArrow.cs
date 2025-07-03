using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip moveArrowSound;
    [SerializeField] private AudioClip clickSound;
    private RectTransform rect;
    private int currentIndex = 0;

    private AudioManager audioManager;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void ChangePosition(int change)
    {
        currentIndex += change;

        if (currentIndex < 0)
        {
            currentIndex = options.Length - 1;
        }
        else if (currentIndex > options.Length - 1)
        {
            currentIndex = 0;
        }
        rect.position = new Vector3(rect.position.x, options[currentIndex].position.y, 0);
        audioManager.PlaySoundClip(moveArrowSound);
    }

    private void Interact()
    {
        options[currentIndex].GetComponent<Button>().onClick.Invoke();
        audioManager.PlaySoundClip(clickSound);
    }

}
