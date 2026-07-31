using UnityEngine;
using System.Linq;

public class PitStopTimer : MonoBehaviour
{
    public WheelFocusZone[] zones;
    public bool isRunning = false;
    public float elapsed = 0f;

    void Update()
    {
        if (isRunning)
        {
            elapsed += Time.deltaTime;
            if (zones != null && zones.Length > 0 && zones.All(z => z.ServiceDone))
                StopTimer();
        }
    }

    public void StartTimer()
    {
        elapsed = 0f;
        isRunning = true;
    }

    void StopTimer()
    {
        isRunning = false;
        Debug.Log($"Pit stop complete: {elapsed:F2}s");

        if (GameManager.Instance != null)
            GameManager.Instance.FinishPitStop();
    }

    void OnGUI()
    {
        if (!isRunning && elapsed <= 0f) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 28,
            alignment = TextAnchor.UpperCenter
        };
        style.normal.textColor = Color.white;

        Rect rect = new Rect(Screen.width / 2f - 100f, 20f, 200f, 40f);
        GUI.Label(rect, $"{elapsed:F2}s", style);
    }
}