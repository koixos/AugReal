using System.Collections.Generic;
using UnityEngine;

public class AIPromptGenerator : MonoBehaviour
{
    public List<string> GeneratePrompts(ProximityLog log)
    {
        List<string> prompts = new();
        foreach (var obj in log.detected_objects)
        {
            string prompt = AnalyzeObj(obj);
            if (!string.IsNullOrEmpty(prompt))
                prompts.Add(prompt);
        }
        return prompts;
    }

    private string AnalyzeObj(DetectedObject obj)
    {
        if (obj.distance < 1f)
            return $"Warning: '{obj.id}' has been collided!";
        
        if (obj.approach_speed > 0f)
        {
            if (obj.time_to_impact < 1f)
                return $"Alert: '{obj.id}' is colliding within 1 sec, slow down!";
            else if (obj.time_to_impact < 3f)
                return $"Caution: '{obj.id}' is colliding within {obj.time_to_impact:F1} sec.";
        }

        return null;
    }
}
