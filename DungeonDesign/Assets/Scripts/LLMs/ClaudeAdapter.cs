using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClaudeAdapter : ILanguageModel
{
    private string apiKey;

    public ClaudeAdapter(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var payload = new
            {
                model = "claude-3-sonnet-20240229",
                msgs = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 512,
                temperature = 0.7
            };

            string jsonPayload = JsonUtility.ToJson(payload);
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.anthropic.com/v1/messages", content);
            string result = await response.Content.ReadAsStringAsync();

            var parsed = JsonUtility.FromJson<string>(result);
            return parsed;
        }
    }
}
