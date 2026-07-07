using UnityEngine;
using UnityEngine.InputSystem;

public class WheelFocusZone : MonoBehaviour
{
    public Camera wheelCamera;
    public WheelController wheelController;

    private bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.fKey.wasPressedThisFrame) return;
        if (FocusManager.Instance == null) return;

        bool thisWheelIsFocused = FocusManager.Instance.IsFocused &&
                                   FocusManager.Instance.CurrentWheel == wheelController;

        if (playerInside && !FocusManager.Instance.IsFocused)
            FocusManager.Instance.EnterFocus(this);
        else if (thisWheelIsFocused)
            FocusManager.Instance.ExitFocus();
    }
}
