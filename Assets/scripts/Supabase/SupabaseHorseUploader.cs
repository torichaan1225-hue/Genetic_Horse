using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseHorseUploader : MonoBehaviour
{

    public void SaveHorse(HorseData data, System.Action onSuccess = null, System.Action<string> onError = null)
    {
        data.owner_user_id = SupabaseAuth.Instance.UserId;
        StartCoroutine(PostHorse(data, onSuccess, onError));
    }

    IEnumerator PostHorse(HorseData data, System.Action onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(data);
        string url = $"{SupabaseConfig.Url}/rest/v1/horses";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", SupabaseConfig.AnonKey);
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseAuth.Instance.AccessToken}");
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("ï€ë∂ê¨å˜: " + request.downloadHandler.text);
            onSuccess?.Invoke();
        }
        else
        {
            string errorMsg = request.error + " / " + request.downloadHandler.text;
            Debug.LogError("ï€ë∂é∏îs: " + errorMsg);
            onError?.Invoke(errorMsg);
        }
    }
}