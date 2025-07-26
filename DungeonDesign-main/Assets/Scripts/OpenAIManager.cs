using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAIManager : MonoBehaviour
{
    public static OpenAIManager Instance { get; private set; }
    public TextMeshProUGUI response;

    [Header("OpenAI Configurations")]
    private readonly string apiKey = "";
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

    const string userDefinition = "User is on a wheelchair, age 29, male, mobility is low.";
    const string aiDefinition = "You are an intelligent guide to lead the specified user. Interpret the information you received in a short and meaningful way.";

    public void RequestAIResp(string dynamicUsrInfo)
    {
        StartCoroutine(SendToAI(dynamicUsrInfo));
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private IEnumerator SendToAI(string info)
    {
        string requestBody = $@"
        {{
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [
                {{
                    ""role"": ""system"",
                    ""content"": ""{aiDefinition}"",
                }},
                {{
                    ""role"": ""user"",
                    ""content"": ""{userDefinition} {info}""
                }}
            ]
        }}";

        UnityWebRequest req = new(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);

        response.gameObject.SetActive(false);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            Debug.Log("Response: " + req.downloadHandler.text);
            HandleResponse(req.downloadHandler.text);
        }
    }

    private void HandleResponse(string json)
    {
        var resp = JsonUtility.FromJson<OpenAIResponse>(json);
        if (resp != null && resp.choices.Length > 0)
        {
            string msg = resp.choices[0].msg.content;
            if (response != null)
            {
                response.text = msg;
                response.gameObject.SetActive(true);
            }
        }
    }
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class Choice
{
    public Message msg;
}

[System.Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}
