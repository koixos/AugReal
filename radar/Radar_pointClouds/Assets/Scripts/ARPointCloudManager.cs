using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ARPointCloudManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public int pointsPerSec = 40;

    private float scaleFactor = 5f;
    private bool startSimulation = false;
    private readonly string filePath = "./Data/File 05-01-2023 at 10.29.05 103.csv";
    private readonly List<GameObject> points = new();

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
    }

    private void LoadAndNormalizeData()
    {
        List<Vector3> positions = new();

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

            if (!float.TryParse(values[1], out float x) ||
                !float.TryParse(values[2], out float y) ||
                !float.TryParse(values[3], out float z))
            {
                Debug.LogWarning("Skipping line " + (i + 1) + ": Invalid float values.");
                continue;
            }

            positions.Add(new Vector3(x, y, z));

            if (x < minX) minX = x;
            if (y < minY) minY = y;
            if (z < minZ) minZ = z;
            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
            if (z > maxZ) maxZ = z;
        }

        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 pos = positions[i];
            float normalizedX = (pos.x -  minX) / (maxX - minX);
            float normalizedY = (pos.y - minY) / (maxY - minY);
            float normalizedZ = (pos.z - minZ) / (maxZ - minZ);

            Vector3 normalizedPosition = new(normalizedX * scaleFactor, normalizedY * scaleFactor, normalizedZ * scaleFactor);
            GameObject point = Instantiate(pointPrefab, normalizedPosition, Quaternion.identity);
            point.SetActive(false);
            point.transform.localScale = new(0.1f, 0.1f, 0.1f);
            points.Add(point);
        }

        Debug.Log(points.Count + " points are placed to the scene.");
    }

    private IEnumerator UpdatePoints()
    {
        int iterNum = points.Count / pointsPerSec;
        for (int i = 0; i < iterNum; i++)
        {
            if (i > 0) DestroyOldPoints((i - 1) * 40);
            ShowPointsPerSecond(i * 40);
            yield return new WaitForSeconds(1f);
        }
    }

    private void ShowPointsPerSecond(int currIndex)
    {
        int i = currIndex;
        while (i < currIndex + pointsPerSec)
        {
            if (i >= points.Count) break;
            points[i].SetActive(true);
            i++;
        }
    }

    private void DestroyOldPoints(int prevIndex)
    {
        int i = prevIndex;
        while (i < prevIndex + pointsPerSec)
        {
            if (i >= points.Count) break;
            points[i].SetActive(false);
            i++;
        }
    }
}
