using UnityEngine;

public class PitZone : MonoBehaviour
{
    public bool CarInside { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
            CarInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
            CarInside = false;
    }
}
