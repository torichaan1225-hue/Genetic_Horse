using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RaceState
{
    Waiting,
    Countdown,
    Racing,
    Finished
}

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;
    public RaceState currentState;
    public List<HorseRaceTracker> horses = new List<HorseRaceTracker>();

    [Header("Countdown Settings")]
    public float countdownDuration = 3f;

    [Header("Finish Settings")]
    public List<HorseRaceTracker> finishOrder = new List<HorseRaceTracker>();

    public event System.Action<int> OnCountdownTick;
    public event System.Action OnRaceStart;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void BeginRace()
    {
        currentState = RaceState.Waiting;
        finishOrder.Clear();
        foreach (var horse in horses)
        {
            horse.canMove = false;
            horse.hasFinished = false;
        }
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        currentState = RaceState.Countdown;

        int count = Mathf.CeilToInt(countdownDuration);
        for (int i = count; i > 0; i--)
        {
            OnCountdownTick?.Invoke(i);
            yield return new WaitForSeconds(1f);
        }

        currentState = RaceState.Racing;
        OnRaceStart?.Invoke();

        foreach (var horse in horses)
        {
            horse.canMove = true;
        }
    }

    public void OnHorseFinished(HorseRaceTracker horse)
    {
        if (horse.hasFinished) return;

        horse.hasFinished = true;
        horse.finishTime = Time.time;
        finishOrder.Add(horse);

        if (finishOrder.Count >= horses.Count)
        {
            EndRace();
        }
    }

    void EndRace()
    {
        currentState = RaceState.Finished;
        RaceResultData.Instance.SetResult(finishOrder);
        SceneLoader.Instance.LoadResultScene();
    }
}