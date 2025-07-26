using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OpenAIGPTAdapter : ILanguageModel
{
    private string apiKey;
    private string model;

    public OpenAIGPTAdapter(string apiKey, string model = "gpt-4")
    {
        this.apiKey = apiKey;
        this.model = model;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var payload = new
            {
                model = model,
                msgs = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            string jsonPayload = JsonUtility.ToJson(payload);
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            string result = await response.Content.ReadAsStringAsync();

            var parsed = JsonUtility.FromJson<string>(result);
            return parsed;
        }
    }
}
