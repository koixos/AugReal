using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DetectedObject
{
    public string id;
    public Vector3 position;
    public float distance;
    public float approach_speed;
    public float time_to_impact;
    public string status;   // "colliding", "approaching", "leaving"
}

[System.Serializable]
public class ProximityLog
{
    public string timestamp;
    public Vector3 player_position;
    public Vector3 player_velocity;
    public List<DetectedObject> detected_objects = new();
}

public class ProximityLogger : MonoBehaviour
{
    public float movementTh = 0.01f;
    private Vector3 lastPosition;
    private Vector3 velocity;
    private List<GameObject> targets;
    private ProximityLog latestLog;

    void Start()
    {
        lastPosition = transform.position;
        targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
    }

    void Update()
    {
        Vector3 currPos = transform.position;
        velocity = (currPos - lastPosition) / Time.deltaTime;

        if ((currPos - lastPosition).sqrMagnitude < movementTh * movementTh) return;

        lastPosition = currPos;

        ProximityLog log = new()
        {
            timestamp = System.DateTime.UtcNow.ToString("o"),
            player_position = currPos,
            player_velocity = velocity
        };

        foreach (GameObject target in targets)
        {
            Vector3 dirToObj = target.transform.position - currPos;
            float distance = dirToObj.magnitude;
            float approach_speed = Vector3.Dot(velocity.normalized, dirToObj.normalized) * velocity.magnitude;

            DetectedObject obj = new()
            {
                id = target.name,
                position = target.transform.position,
                distance = distance,
                approach_speed = approach_speed,
            };

            if (distance < 1f)
            {
                obj.time_to_impact = 0f;
                obj.status = "colliding";
            }
            else if (approach_speed > 0f)
            {
                obj.time_to_impact = distance / approach_speed;
                obj.status = "approaching";
            }
            else
            {
                obj.time_to_impact = -1f;
                obj.status = "leaving";
            }

            log.detected_objects.Add(obj);
        }

        latestLog = log;

        string json = JsonUtility.ToJson(log, true);
        string path = Path.Combine(Application.persistentDataPath, "proximity_log.json");
        File.WriteAllText(path, json);
    }

    public ProximityLog GetLatestLog()
    {
        return latestLog;
    }
}
