using UnityEngine;

public class FocusManager : MonoBehaviour
{
    public static FocusManager Instance;

    public Camera mainCamera;
    public FirstPersonMovement playerMovement;

    public bool IsFocused { get; private set; }
    public Camera CurrentWheelCamera { get; private set; }
    public WheelController CurrentWheel { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EnterFocus(WheelFocusZone zone)
    {
        if (IsFocused) return;

        IsFocused = true;
        CurrentWheel = zone.wheelController;
        CurrentWheelCamera = zone.wheelCamera;

        mainCamera.gameObject.SetActive(false);
        zone.wheelCamera.gameObject.SetActive(true);

        playerMovement.inputLocked = true;
        CurrentWheel.OnFocused();
    }

    public void ExitFocus()
    {
        if (!IsFocused) return;

        CurrentWheelCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        playerMovement.inputLocked = false;
        IsFocused = false;
        CurrentWheel = null;
        CurrentWheelCamera = null;
    }
}