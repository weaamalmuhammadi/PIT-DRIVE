using UnityEngine;

public class PitZone : MonoBehaviour
{
    public bool CarInside { get; private set; }

    public void ResetCarInside() => CarInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarInside = true;
            if (GameManager.Instance != null)
                GameManager.Instance.EnterPit();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
            CarInside = false;
    }
}
