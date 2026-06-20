using UnityEngine;
using TMPro;

public class ResultRowUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text horseNameText;
    public TMP_Text finishTimeText;

    public void Setup(int rank, string horseName, float finishTime)
    {
        rankText.text = rank.ToString();
        horseNameText.text = horseName;
        finishTimeText.text = FormatTime(finishTime);
    }

    string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60f);
        float remainingSeconds = seconds % 60f;
        return $"{minutes}:{remainingSeconds:00.00}";
    }
}