using UnityEngine;
using System.Linq;

public class PitStopTimer : MonoBehaviour
{
    public WheelController[] wheels;
    public bool isRunning = false;
    public float elapsed = 0f;

    void Update()
    {
        if (isRunning)
        {
            elapsed += Time.deltaTime;
            if (wheels != null && wheels.Length > 0 && wheels.All(w => w.IsSecured))
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
    }
}