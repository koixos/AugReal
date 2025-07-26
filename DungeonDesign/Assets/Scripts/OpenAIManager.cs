using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenAIManager : MonoBehaviour
{
    [Header("UI Configurations")]
    public GameObject player;
    public GameObject aiPrefab;
    public TextMeshProUGUI response;
    public Button sendBtn;

    [Header("OpenAI Configurations")]
    private readonly string apiKey = "";
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

    const string userDefinition = "User is on a wheelchair, age 29, male, mobility is low.";
    const string aiDefinition = "You are an intelligent guide to lead the specified user. Interpret the information you received in a short and meaningful way.";

    public void OnSendBtnClicked()
    {
        StartCoroutine(SendToAI());
    }

    private IEnumerator SendToAI()
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
                    ""content"": ""{userDefinition}""
                }}
            ]
        }}";

        UnityWebRequest req = new(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);

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
