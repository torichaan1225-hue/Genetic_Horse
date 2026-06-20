using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HorseResultEntry
{
    public int rank;
    public string horseName;
    public float finishTime;
}

public class RaceResultData : MonoBehaviour
{
    public static RaceResultData Instance;
    public List<HorseResultEntry> results = new List<HorseResultEntry>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetResult(List<HorseRaceTracker> finishOrder)
    {
        results.Clear();
        for (int i = 0; i < finishOrder.Count; i++)
        {
            results.Add(new HorseResultEntry
            {
                rank = i + 1,
                horseName = finishOrder[i].horseName,
                finishTime = finishOrder[i].finishTime
            });
        }
    }
}