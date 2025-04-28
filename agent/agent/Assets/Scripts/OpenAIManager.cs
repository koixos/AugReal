using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenAIManager : MonoBehaviour
{
    [Header("OpenAI Settings")]
    private readonly string apiKey = "";
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

    [Header("Unity Objects")]
    public GameObject gandalfPrefab;
    public Transform spawnPoint;
    public TextMeshProUGUI responseText;
    public Button sendButton;
    public GameObject player;

    public void OnSendButtonClicked()
    {
        StartCoroutine(Send());
    }

    private IEnumerator Send()
    {
        string userInput = "User is on a wheelchair, age 29, mobility is low. There is a stairs object 1.8 meters ahead.";

        string requestBody = $@"
        {{
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [
                {{
                    ""role"": ""system"",
                    ""content"": ""You are an intelligent guide. Interpret the information you received in a short and meaningful way.""
                }},
                {{
                    ""role"": ""user"",
                    ""content"": ""{userInput}""
                }}
            ]
        }}";

        UnityWebRequest request = new(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            HandleResponse(request.downloadHandler.text);
        }
    }

    private void HandleResponse(string json)
    {
        var response = JsonUtility.FromJson<OpenAIResponse>(json);
        if (response != null && response.choices.Length > 0)
        {
            string message = response.choices[0].message.content;
            
            if (responseText != null)
                responseText.text = message;

            if (gandalfPrefab != null && spawnPoint != null)
            {
                var gandalf = Instantiate(gandalfPrefab, spawnPoint.position, spawnPoint.rotation, player.transform);
                gandalf.transform.localPosition += new Vector3(1.5f, 0f, 1.5f);
                gandalf.transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }
    }
}

[System.Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}
