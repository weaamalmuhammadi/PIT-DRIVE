using UnityEngine;
using System.Collections;

public class LugNut : MonoBehaviour
{
    public float tightness = 100f; // 100 = fully tight, 0 = loose
    public float changeSpeed = 40f;
    public float unscrewDistance = 0.02f; // how far the nut backs out along its bolt axis as it loosens
    public float fallDistance = 0.15f;
    public float fallDuration = 0.6f;

    public bool isLoose => tightness <= 0f;
    public bool isTight => tightness >= 100f;

    Quaternion baseRotation;
    Vector3 baseLocalPosition;
    bool hasFallen;

    void Start()
    {
        baseRotation = transform.localRotation;
        baseLocalPosition = transform.localPosition;
    }

    public void Loosen(float dt) => tightness = Mathf.Max(0, tightness - changeSpeed * dt);
    public void Tighten(float dt) => tightness = Mathf.Min(100, tightness + changeSpeed * dt);

    public void ResetPlacement()
    {
        StopAllCoroutines();
        hasFallen = false;
        transform.localPosition = baseLocalPosition;
        transform.localRotation = baseRotation;
    }

    void Update()
    {
        if (hasFallen) return;

        // spin on its own bolt axis, plus back outward along that same axis so it reads visually
        // even when the camera is looking straight down the axis (a hex nut spinning in place alone is imperceptible)
        transform.localRotation = baseRotation * Quaternion.Euler(0, 0, tightness * 3.6f);
        Vector3 boltAxis = baseRotation * Vector3.forward;
        transform.localPosition = baseLocalPosition + boltAxis * ((100f - tightness) / 100f * unscrewDistance);

        if (isLoose)
        {
            hasFallen = true;
            StartCoroutine(FallAnimation());
        }
    }

    IEnumerator FallAnimation()
    {
        Vector3 start = transform.localPosition;
        Vector3 end = start + Vector3.down * fallDistance;
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(Random.Range(60f, 180f), Random.Range(60f, 180f), Random.Range(60f, 180f));

        float t = 0f;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float p = t / fallDuration;
            transform.localPosition = Vector3.Lerp(start, end, p);
            transform.localRotation = Quaternion.Slerp(startRot, endRot, p);
            yield return null;
        }
        transform.localPosition = end;
        transform.localRotation = endRot;
    }
}
