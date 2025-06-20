using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInspector : MonoBehaviour
{
    public GameObject skillEffect;

    private float timeToActivate;
    private float skillDuration = 0.5f;

    private void Start()
    {
        // Randomize timeToActivate between 1 and 2 seconds
        timeToActivate = Random.Range(1f, 2f);
        StartCoroutine(ActivateSkillEffect());
    }

    private IEnumerator ActivateSkillEffect()
    {
        yield return new WaitForSeconds(timeToActivate);

        if (skillEffect != null)
        {
            GameObject bolt = Instantiate(skillEffect, transform.position, transform.rotation);
            Destroy(bolt, skillDuration);
        }

        Destroy(gameObject);
    }
}
