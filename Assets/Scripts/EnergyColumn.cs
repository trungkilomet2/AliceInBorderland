using UnityEngine;
using System.Collections;

public class EnergyColumn : MonoBehaviour
{
    [Header("Beam")]
    public GameObject beamPrefab;            // drag BeamSprite prefab
    public Color beamColor = Color.white;    // riêng từng cột
    public float riseTime = 0.4f;            // thời gian beam scale lên
    public float beamDuration = 1.6f;        // tồn tại

    GameObject beamGO;
    SpriteRenderer beamSR;

    public void FireBeam(Transform target)
    {
        StartCoroutine(BeamRoutine(target));
    }

    IEnumerator BeamRoutine(Transform target)
    {
        // Tạo beam nếu chưa có
        if (beamGO == null)
        {
            beamGO = Instantiate(beamPrefab, transform);
            beamSR = beamGO.GetComponent<SpriteRenderer>();
            beamSR.color = beamColor;
        }
        beamGO.SetActive(true);

        // Scale từ 0 → 1 theo Y (phụt)
        Vector3 p0 = transform.position;
        Vector3 p1 = target.position;
        Vector3 dir = p1 - p0;
        float distance = dir.magnitude;

        // Giữa đoạn beam
        Vector3 mid = (p0 + p1) / 2f;
        beamGO.transform.position = mid;
        beamGO.transform.rotation =
            Quaternion.LookRotation(Vector3.forward, dir);    // xoay sprite theo tia
        beamGO.transform.localScale = new Vector3(1, 0, 1);   // cao = 0 ban đầu

        float t = 0;
        while (t < riseTime)
        {
            float k = t / riseTime;
            beamGO.transform.localScale = new Vector3(1, k * distance, 1);
            t += Time.deltaTime;
            yield return null;
        }
        beamGO.transform.localScale = new Vector3(1, distance, 1);

        // Giữ tia sáng beamDuration
        yield return new WaitForSeconds(beamDuration);

        beamGO.SetActive(false);
    }
}
