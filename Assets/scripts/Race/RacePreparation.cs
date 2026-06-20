using System.Collections.Generic;
using UnityEngine;

public class RacePreparation : MonoBehaviour
{

    [System.Serializable]
    public class HorseModelEntry
    {
        public string modelName;
        public GameObject prefab;
    }

    [SerializeField] private SupabaseHorseLoader supabaseLoader;
    [SerializeField] private HorseModelEntry[] horseModels;
    [SerializeField] private GameObject fallbackPrefab;
    [SerializeField] private Transform[] startPositions;

    private Dictionary<string, GameObject> modelLookup;

    void Awake()
    {
        modelLookup = new Dictionary<string, GameObject>();
        foreach (var entry in horseModels)
        {
            modelLookup[entry.modelName] = entry.prefab;
        }
    }

    void Start()
    {
        supabaseLoader.LoadAllHorses(horseList => {
            if (horseList.Count > startPositions.Length)
            {
                Debug.LogWarning($"発走位置の数({startPositions.Length})より馬の数({horseList.Count})が多いため、超過した馬は生成されません。");
            }

            int spawnCount = Mathf.Min(horseList.Count, startPositions.Length);

            for (int i = 0; i < spawnCount; i++)
            {
                var data = horseList[i];
                var horseObj = SpawnHorse(data, startPositions[i]);
                horseObj.GetComponent<HorseGeneController>().ApplyGenome(data.genome);
            }
            RaceManager.Instance.BeginRace();
        });
    }

    GameObject SpawnHorse(HorseData data, Transform startPos)
    {
        GameObject prefabToUse = fallbackPrefab;

        if (!string.IsNullOrEmpty(data.model_name) && modelLookup.TryGetValue(data.model_name, out var found))
        {
            prefabToUse = found;
        }
        else
        {
            Debug.LogWarning($"モデル '{data.model_name}' が見つからないため、デフォルトモデルを使用します。");
        }

        var obj = Instantiate(prefabToUse, startPos.position, startPos.rotation);
        var tracker = obj.GetComponent<HorseRaceTracker>();
        tracker.horseName = data.horse_name;
        tracker.modelName = data.model_name;
        tracker.isPlayer = !string.IsNullOrEmpty(data.owner_user_id)
            && data.owner_user_id == SupabaseAuth.Instance.UserId;

        RaceManager.Instance.horses.Add(tracker);
        return obj;
    }
}