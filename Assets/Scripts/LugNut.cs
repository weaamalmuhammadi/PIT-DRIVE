using UnityEngine;
using System.Collections;

public class LugNut : MonoBehaviour
{
    public float tightness = 100f; // 100 = fully tight, 0 = loose
    public float changeSpeed = 40f;
    public float unscrewDistance = 0.05f; // how far the nut backs out along its bolt axis as it loosens
    public float fallDistance = 0.5f;
    public float fallDuration = 0.6f;

    public bool isLoose => tightness <= 0f;
    public bool isTight => tightness >= 100f;

    Quaternion baseRotation;
    Vector3 baseLocalPosition;
    bool hasFallen;
    bool wasLoose;

    void Start()
    {
        baseRotation = transform.localRotation;
        baseLocalPosition = transform.localPosition;
        Debug.Log($"[LugNut:{name}] Start. baseLocalPosition={baseLocalPosition} baseRotation euler={baseRotation.eulerAngles} tightness={tightness}");
    }

    public void Loosen(float dt)
    {
        tightness = Mathf.Max(0, tightness - changeSpeed * dt);
        Debug.Log($"[LugNut:{name}] rotating (loosening), tightness={tightness:F1}");
    }

    public void Tighten(float dt)
    {
        tightness = Mathf.Min(100, tightness + changeSpeed * dt);
        Debug.Log($"[LugNut:{name}] rotating (tightening), tightness={tightness:F1}");
    }

    public void ResetPlacement()
    {
        StopAllCoroutines();
        hasFallen = false;
        wasLoose = true; // tightness is about to be set to 0 by the caller; don't treat that as a fresh "just went loose" edge
        gameObject.SetActive(true);
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

        Debug.Log($"[LugNut:{name}] Update: localPos={transform.localPosition} localEuler={transform.localRotation.eulerAngles} tightness={tightness:F1}");

        // only fall on the transition into looseness (actively unscrewed), not every frame it happens to already be at 0
        // (e.g. right after ResetPlacement sets tightness=0 for the new tire's not-yet-tightened nuts)
        if (isLoose && !wasLoose)
        {
            hasFallen = true;
            StartCoroutine(FallAnimation());
        }
        wasLoose = isLoose;
    }

    IEnumerator FallAnimation()
    {
        Debug.Log($"[LugNut:{name}] Falling off, like the tire.");
        Vector3 start = transform.localPosition;
        Vector3 localDown = transform.parent != null ? transform.parent.InverseTransformDirection(Vector3.down) : Vector3.down;
        Vector3 end = start + localDown * fallDistance;
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

        // disappear once fallen, same as the tire does after its own fall
        gameObject.SetActive(false);
    }
}
