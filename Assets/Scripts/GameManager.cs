using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject car;
    public GameObject carPIT;
    public PitZone pitZone;
    public Transform afterPitPoint;

    public GameObject player;
    public Camera playerCamera;
    public Camera carCamera;
    public CarController carController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EnterPit()
    {
        car.SetActive(false);
        carPIT.SetActive(true);
    }

    public void FinishPitStop()
    {
        carPIT.SetActive(false);

        car.transform.SetPositionAndRotation(afterPitPoint.position, afterPitPoint.rotation);
        car.SetActive(true);
        carController.enabled = true;
        carCamera.gameObject.SetActive(true);

        player.SetActive(false);
        playerCamera.gameObject.SetActive(false);

        if (pitZone != null)
            pitZone.ResetCarInside();
    }
}
