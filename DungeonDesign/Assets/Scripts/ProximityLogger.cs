using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProximityLogger : MonoBehaviour
{
    public float movementThreshold = 0.01f;

    private Vector3 lastPos;
    private Vector3 velocity;
    private ProximityLog lastLog;
    private List<GameObject> targets;

    void Start()
    {
        lastPos = transform.position;
        targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("target"));
    }

    void Update()
    {
        Vector3 currentPos = transform.position;

        var moveAmount = currentPos - lastPos;
        velocity = moveAmount / Time.deltaTime;

        if (moveAmount.sqrMagnitude < movementThreshold * movementThreshold)
        {
            return;
        }

        lastPos = currentPos;

        ProximityLog log = new()
        {
            timestamp = System.DateTime.UtcNow.ToString("o"),
            playerPos = currentPos,
            playerVelocity = velocity
        };

        foreach (GameObject target in targets)
        {
            Vector3 directionToObj = target.transform.position - currentPos;
            float distance = directionToObj.magnitude;
            float approachSpeed = velocity.magnitude * Vector3.Dot(velocity.normalized, directionToObj.normalized);

            DetectedObject obj = new()
            {
                id = target.name,
                pos = target.transform.position,
                distance = distance,
                approachSpeed = approachSpeed
            };

            if (distance < 1f)
            {
                obj.timeToImpact = 0f;
                obj.status = "colliding";
            }
            else if (approachSpeed > 0f)
            {
                obj.timeToImpact = distance / approachSpeed;
                obj.status = "approaching";
            }
            else
            {
                obj.timeToImpact = -1f;
                obj.status = "leaving";
            }

            log.detectedObjs.Add(obj);
        }

        lastLog = log;

        string json = JsonUtility.ToJson(log, true);
        string path = Path.Combine(Application.persistentDataPath, "ProximityLogs.json");
        File.WriteAllText(path, json);
    }

    public ProximityLog GetLastLog()
    {
        return lastLog;
    }
}

[System.Serializable]
public class DetectedObject
{
    public string id;
    public Vector3 pos;
    public float distance;
    public float approachSpeed;
    public float timeToImpact;
    public string status;   /* colliding, approaching, leaving */
}

[System.Serializable]
public class ProximityLog
{
    public string timestamp;
    public Vector3 playerPos;
    public Vector3 playerVelocity;
    public List<DetectedObject> detectedObjs = new();
}
