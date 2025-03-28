using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ARPointCloudManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public float frameTime = 0.028f; // almost 36 FPS
    public float scaleFactor = 5f;
    public float pointSize = 0.2f;
    public int pointsPerSec = 40;

    private readonly string filePath = "./Data/File 05-01-2023 at 10.29.05 103.csv";
    private readonly Dictionary<int, List<GameObject>> frameData = new();
    private readonly Dictionary<GameObject, float> velocitiesByPoint = new();
    private readonly Queue<GameObject> activePoints = new();
    private int maxFrame = 0;
    private bool startSimulation = false;

    void Start()
    {
        LoadAndNormalizeData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !startSimulation)
        {
            startSimulation = true;
            StartCoroutine(UpdatePoints());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && startSimulation)
        {
            startSimulation = false;
            StopAllCoroutines();
        }
    }

    private void LoadAndNormalizeData()
    {
        Dictionary<int, List<(Vector3, float)>> positionsByFrame = new();

        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        string _filePath = Path.Combine(Application.dataPath, filePath);
        if (!File.Exists(_filePath))
        {
            Debug.LogError("No such file found: " + _filePath);
            return;
        }

        string[] lines = File.ReadAllLines(_filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 5)
            {
                Debug.LogWarning("Skipping line " + (i + 1) + ": Not enough values.");
                continue;
            }

            if (!int.TryParse(values[0], out int frame) ||
                !float.TryParse(values[1], out float x) ||
                !float.TryParse(values[2], out float y) ||
                !float.TryParse(values[3], out float z) ||
                !float.TryParse(values[4], out float velocity))
            {
                Debug.LogWarning("Skipping line " + (i + 1) + ": Invalid float values.");
                continue;
            }

            if (!positionsByFrame.ContainsKey(frame))
            {
                positionsByFrame[frame] = new List<(Vector3, float)>();
            }
            positionsByFrame[frame].Add((new Vector3(x, y, z), velocity));
            if (frame > maxFrame) maxFrame = frame;

            if (x < minX) minX = x;
            if (y < minY) minY = y;
            if (z < minZ) minZ = z;
            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
            if (z > maxZ) maxZ = z;
        }

        // normalize and instantiate objects
        foreach (var kvp in positionsByFrame)
        {
            int frame = kvp.Key;
            frameData[frame] = new List<GameObject>();

            foreach (var (pos, vel) in kvp.Value)
            {
                float normalizedX = (pos.x - minX) / (maxX - minX);
                float normalizedY = (pos.y - minY) / (maxY - minY);
                float normalizedZ = (pos.z - minZ) / (maxZ - minZ);

                Vector3 normalizedPosition = new(normalizedX * scaleFactor, normalizedY * scaleFactor, normalizedZ * scaleFactor);
                
                GameObject point = Instantiate(pointPrefab, normalizedPosition, Quaternion.identity);
                point.SetActive(false);
                point.transform.localScale = new(pointSize, pointSize, pointSize);

                velocitiesByPoint[point] = vel;
                frameData[frame].Add(point);

                ApplyVelocityColor(point, vel);
            }
        }

        Debug.Log(frameData.Count + " frames of point data is loaded.");
    }

    private IEnumerator UpdatePoints()
    {
        for (int i = 0; i <= maxFrame; i++)
        {
            if (frameData.ContainsKey(i))
            {
                foreach (var point in frameData[i])
                {
                    point.SetActive(true);
                    StartCoroutine(MovePoints(point));
                    activePoints.Enqueue(point);
                }
            }

            while (activePoints.Count > pointsPerSec)
            {
                GameObject firstPoint = activePoints.Dequeue();
                firstPoint.SetActive(false);
            }

            yield return new WaitForSeconds(frameTime);

            /*if (i > 0 && frameData.ContainsKey(i - 1))
            {
                foreach (var point in frameData[i - 1])
                {
                    point.SetActive(false);
                }
            }*/
        }
    }

    private void ApplyVelocityColor(GameObject point, float velocity)
    {
        if (velocity < 0) velocity *= -1;

        if (point.TryGetComponent<Renderer>(out var renderer))
        {
            if (velocity == 0)
            {
                renderer.material.color = Color.green; // still
            }
            else if (velocity > 0 && velocity < 0.5f)
            {
                renderer.material.color = Color.yellow; // slow
            }
            else
            {
                renderer.material.color = Color.red; // fast
            }
        }
    }

    private IEnumerator MovePoints(GameObject point)
    {
        while (startSimulation)
        {
            if (velocitiesByPoint.TryGetValue(point, out float velocity))
            {
                point.transform.position += new Vector3(0, 0, velocity * Time.deltaTime); // move in z-axis
            }
            yield return null; // works for each frame
        }
    }
}
