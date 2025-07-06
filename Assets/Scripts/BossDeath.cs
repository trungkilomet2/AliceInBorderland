using UnityEngine;
using System.Collections;

public class BossDeath : MonoBehaviour
{
    public GameObject beamPrefab;
    public Transform[] beamSources; // 4 vị trí cột
    public float beamDelay = 0.2f;
    public float destroyDelay = 2f;

    public void TriggerDeath()
    {
        StartCoroutine(BeamAttack());
    }

    IEnumerator BeamAttack()
    {
        foreach (Transform source in beamSources)
        {
            GameObject beam = Instantiate(beamPrefab, source.position, Quaternion.identity);
            beam.GetComponent<BeamEffect>().target = this.transform;
            FindObjectOfType<CutSceneManager>().RegisterBeam(beam);
            yield return new WaitForSeconds(beamDelay);
        }

        yield return new WaitForSeconds(destroyDelay);

        // Hiệu ứng rung lắc hoặc animation
        // transform.DOShakePosition(0.5f, 0.2f); // nếu dùng DOTween

        // Biến mất
        Destroy(gameObject);
    }
}
