using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseHorseLoader : MonoBehaviour
{

    public void LoadAllHorses(System.Action<List<HorseData>> onLoaded, System.Action<string> onError = null)
    {
        StartCoroutine(GetHorses(onLoaded, onError));
    }

    IEnumerator GetHorses(System.Action<List<HorseData>> onLoaded, System.Action<string> onError)
    {
        string url = $"{SupabaseConfig.Url}/rest/v1/horses?select=*";

        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", SupabaseConfig.AnonKey);
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseAuth.Instance.AccessToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrappedJson = "{\"items\":" + request.downloadHandler.text + "}";
            var wrapper = JsonUtility.FromJson<HorseDataListWrapper>(wrappedJson);
            onLoaded(wrapper.items);
        }
        else
        {
            string errorMsg = request.error + " / " + request.downloadHandler.text;
            Debug.LogError("ì«Ç›çûÇ›é∏îs: " + errorMsg);
            onError?.Invoke(errorMsg);
        }
    }
}