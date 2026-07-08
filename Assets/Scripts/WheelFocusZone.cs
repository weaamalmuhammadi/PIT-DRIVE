using UnityEngine;
using UnityEngine.InputSystem;

public class WheelFocusZone : MonoBehaviour
{
    public Camera wheelCamera;
    public WheelController wheelController;

    public bool ServiceDone { get; private set; }

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
        if (!ServiceDone && FocusManager.Instance != null && FocusManager.Instance.CurrentZone == this && wheelController.IsSecured)
            ServiceDone = true;

        if (Keyboard.current == null || !Keyboard.current.fKey.wasPressedThisFrame) return;
        if (FocusManager.Instance == null) return;

        bool thisWheelIsFocused = FocusManager.Instance.IsFocused &&
                                   FocusManager.Instance.CurrentZone == this;

        if (playerInside && !FocusManager.Instance.IsFocused)
            FocusManager.Instance.EnterFocus(this);
        else if (thisWheelIsFocused)
            FocusManager.Instance.ExitFocus();
    }
}
