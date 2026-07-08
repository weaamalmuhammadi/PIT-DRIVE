using UnityEngine;
using UnityEngine.InputSystem;

public class CarExitController : MonoBehaviour
{
    public PitZone pitZone;
    public Camera carCamera;
    public CarController carController;
    public GameObject player;
    public Camera playerCamera;
    public Transform exitPoint;

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame) return;
        if (pitZone == null || !pitZone.CarInside) return;

        player.transform.SetPositionAndRotation(exitPoint.position, exitPoint.rotation);
        player.SetActive(true);
        playerCamera.gameObject.SetActive(true);

        carController.enabled = false;
        carCamera.gameObject.SetActive(false);
    }
}
