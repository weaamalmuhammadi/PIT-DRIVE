
using UnityEngine;
public class FinishLineTrigger : MonoBehaviour
{
    public RaceManager raceManager; // اسحب أوبجكت RaceManager هنا من الـ Inspector
 
    void OnTriggerEnter(Collider other)
    {
        // السيارة لازم يكون عليها Tag = "Player"
        if (other.CompareTag("Player"))
        {
            raceManager.OnCarCrossedFinish();
        }
    }
}
