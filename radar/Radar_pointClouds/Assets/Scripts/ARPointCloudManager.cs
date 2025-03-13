using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ARPointCloudManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public string filePath = "./Data/radar_data.csv";
    public int pointsPerSec = 40;

    private bool startSimulation = false;
    private int currIndex = 0;
    private readonly List<GameObject> points = new();

    private void Start()
    {
        LoadPoints();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !startSimulation)
        {
            startSimulation = true;
            StartCoroutine(UpdatePoints());
        }
    }

    private void LoadPoints()
    {
        string _filePath = Path.Combine(Application.dataPath, filePath);
        if (!File.Exists(_filePath))
        {
            Debug.LogError("No such file found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(_filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 3) continue;

            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float z = float.Parse(values[2]);

            Vector3 position = new(x, y, z);
            GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
            points.Add(point);
        }

        Debug.Log(points.Count + " points are placed to the scene.");
    }

    private IEnumerator UpdatePoints()
    {
        int iterNum = points.Count / pointsPerSec;
        for (int i = 0; i < 1; i++)
        {
            if (i > 0) DestroyOldPoints((i - 1) * 40);
            ShowPointsPerSecond(i * 40);
            yield return new WaitForSeconds(1f);
        }
    }

    private void ShowPointsPerSecond(int currIndex)
    {
        while (currIndex < currIndex + pointsPerSec)
        {
            if (currIndex >= points.Count) break;
            points[currIndex].SetActive(true);
            currIndex++;
        }
    }

    private void DestroyOldPoints(int prevIndex)
    {
        while (prevIndex < prevIndex + pointsPerSec)
        {
            if (prevIndex >= points.Count) break;
            points[prevIndex].SetActive(false);
            prevIndex++;
        }
    }
}
