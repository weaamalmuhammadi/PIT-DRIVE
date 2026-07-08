using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum WheelState { Attached, Loosening, Detached, Tightening, Secured }

public class WheelController : MonoBehaviour
{
    public WheelState state = WheelState.Attached;
    public Transform focusPoint;
    public List<LugNut> lugNuts;
    public GameObject oldWheelMesh; // can stay None
    public GameObject newWheelMesh; // can stay None

    public bool IsSecured => state == WheelState.Secured;

    Vector3 installLocalPos;
    Vector3 newSpawnLocalPos;

    void Awake()
    {
        if (oldWheelMesh != null) installLocalPos = oldWheelMesh.transform.localPosition;
        if (newWheelMesh != null) newSpawnLocalPos = newWheelMesh.transform.localPosition;
    }

    public void OnFocused()
    {
        if (state == WheelState.Attached)
            state = WheelState.Loosening;
    }

    void Update()
    {
        if (lugNuts != null && lugNuts.Count > 0)
        {
            if (state == WheelState.Loosening && lugNuts.All(n => n.isLoose))
                DetachAndSwap();
            else if (state == WheelState.Tightening && lugNuts.All(n => n.isTight))
                state = WheelState.Secured;
        }
    }

    void DetachAndSwap()
    {
        state = WheelState.Detached;

        foreach (var nut in lugNuts)
        {
            nut.ResetPlacement();
            nut.tightness = 0f;
        }

        StartCoroutine(SwapAnimation());
    }

    public void ResetForNewService()
    {
        StopAllCoroutines();
        state = WheelState.Attached;

        if (oldWheelMesh != null)
        {
            oldWheelMesh.transform.localPosition = installLocalPos;
            oldWheelMesh.SetActive(true);
        }
        if (newWheelMesh != null)
        {
            newWheelMesh.transform.localPosition = newSpawnLocalPos;
            newWheelMesh.SetActive(false);
        }

        foreach (var nut in lugNuts)
        {
            nut.ResetPlacement();
            nut.tightness = 100f;
        }
    }

    IEnumerator SwapAnimation()
    {
        const float duration = 0.3f;

        if (oldWheelMesh != null)
        {
            Vector3 startPos = oldWheelMesh.transform.localPosition;
            Vector3 fallPos = startPos + Vector3.down * 0.5f;

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                oldWheelMesh.transform.localPosition = Vector3.Lerp(startPos, fallPos, t / duration);
                yield return null;
            }

            oldWheelMesh.SetActive(false);
            oldWheelMesh.transform.localPosition = startPos;
        }

        if (newWheelMesh != null)
        {
            newWheelMesh.transform.localPosition = newSpawnLocalPos;
            newWheelMesh.SetActive(true);

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                newWheelMesh.transform.localPosition = Vector3.Lerp(newSpawnLocalPos, installLocalPos, t / duration);
                yield return null;
            }

            newWheelMesh.transform.localPosition = installLocalPos;
        }

        state = WheelState.Tightening;
    }

    void OnGUI()
    {
        if (lugNuts == null || lugNuts.Count == 0) return;
        if (state == WheelState.Attached || state == WheelState.Secured) return;

        Camera cam = FocusManager.Instance != null && FocusManager.Instance.IsFocused
            ? FocusManager.Instance.CurrentWheelCamera
            : FocusManager.Instance != null ? FocusManager.Instance.mainCamera : Camera.main;
        if (cam == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(transform.position + Vector3.up * 0.3f);
        if (screenPos.z < 0f) return;

        float avgTightness = lugNuts.Average(n => n.tightness) / 100f;
        float barWidth = 70f, barHeight = 10f;
        float x = screenPos.x - barWidth / 2f;
        float y = Screen.height - screenPos.y - barHeight / 2f;

        GUI.color = new Color(0f, 0f, 0f, 0.6f);
        GUI.DrawTexture(new Rect(x, y, barWidth, barHeight), Texture2D.whiteTexture);

        GUI.color = Color.Lerp(Color.red, Color.green, avgTightness);
        GUI.DrawTexture(new Rect(x, y, barWidth * avgTightness, barHeight), Texture2D.whiteTexture);

        GUI.color = Color.white;
    }
}