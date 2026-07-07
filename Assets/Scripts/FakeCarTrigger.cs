using UnityEngine;
using UnityEngine.InputSystem;

public class FakeCarTrigger : MonoBehaviour
{
    public PitStopTimer timer;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            timer.StartTimer();
    }
}