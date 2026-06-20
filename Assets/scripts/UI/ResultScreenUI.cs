using UnityEngine;

public class ResultScreenUI : MonoBehaviour
{
    public Transform resultListParent;
    public GameObject resultRowPrefab;

    void Start()
    {
        foreach (var entry in RaceResultData.Instance.results)
        {
            var row = Instantiate(resultRowPrefab, resultListParent);
            row.GetComponent<ResultRowUI>().Setup(entry.rank, entry.horseName, entry.finishTime);
        }
    }
}