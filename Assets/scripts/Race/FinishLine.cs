using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var horse = other.GetComponentInParent<HorseRaceTracker>();
        if (horse != null)
        {
            RaceManager.Instance.OnHorseFinished(horse);
        }
    }
}