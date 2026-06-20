using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseAuth : MonoBehaviour
{
    public static SupabaseAuth Instance;

    public string AccessToken { get; private set; }
    public string UserId { get; private set; }
    public bool IsSignedIn { get; private set; }

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

    public void SignInAnonymously(System.Action onComplete, System.Action<string> onError = null)
    {
        StartCoroutine(SignInRoutine(onComplete, onError));
    }

    IEnumerator SignInRoutine(System.Action onComplete, System.Action<string> onError)
    {
        string url = $"{SupabaseConfig.Url}/auth/v1/signup";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", SupabaseConfig.AnonKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            AccessToken = response.access_token;
            UserId = response.user.id;
            IsSignedIn = true;
            onComplete?.Invoke();
        }
        else
        {
            string errorMsg = request.error + " / " + request.downloadHandler.text;
            Debug.LogError("“½–¼ƒTƒCƒ“ƒCƒ“Ž¸”s: " + errorMsg);
            onError?.Invoke(errorMsg);
        }
    }
}

[System.Serializable]
public class AuthResponse
{
    public string access_token;
    public string refresh_token;
    public AuthUser user;
}

[System.Serializable]
public class AuthUser
{
    public string id;
}