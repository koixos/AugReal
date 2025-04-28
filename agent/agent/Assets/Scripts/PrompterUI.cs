
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrompterUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public ProximityLogger logger;
    public AIPromptGenerator promptGenerator;

    void Update()
    {
        ProximityLog log = logger.GetLatestLog();
        if (log == null) return;

        List<string> prompts = promptGenerator.GeneratePrompts(log);
        if (prompts.Count == 0)
        {
            promptText.text = "";
            promptText.gameObject.SetActive(false);
            return;
        }

        string combined = string.Join("\n", prompts);
        promptText.text = combined;
        promptText.gameObject.SetActive(true);
    }
}
