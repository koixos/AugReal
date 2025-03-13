using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PointCloudVisualizer : MonoBehaviour
{
    public GameObject pointPrefab;
    public string fileName = "pointdata.txt";
    public int pointsPerSec = 40;
    public int maxVisiblePoints = 400;
    public Color pointClr = Color.blue;
    public Vector3 arOffset = new(0, 0, 1f);

    private List<Vector3> allPoints = new();
    private List<GameObject> activePoints = new();

    void Start()
    {
        LoadPointDataFromFile();
        StartCoroutine(UpdatePointsCoroutine());
    }

    void LoadPointDataFromFile()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] coords = line.Split(',');
                if (coords.Length == 3)
                {
                    float x = float.Parse(coords[0]);
                    float y = float.Parse(coords[1]);
                    float z = float.Parse(coords[2]);

                    allPoints.Add(new Vector3(x, y, z));
                }
            }
        }
        else
        {
            Debug.LogError($"No such file found: {path}");
        }
    }

    IEnumerator UpdatePointsCoroutine()
    {
        int currIndex = 0;

        while (true)
        {
            for (int i = 0; i < pointsPerSec; i++)
            {
                if (currIndex < allPoints.Count)
                {
                    CreatePointVisual(allPoints[currIndex]);
                    ++currIndex;

                    if (currIndex >= allPoints.Count)
                    {
                        currIndex = 0;
                    }
                }
            }

            if (activePoints.Count > maxVisiblePoints)
            {
                int removeCount = activePoints.Count - maxVisiblePoints;
                for (int i = 0; i < removeCount; i++)
                {
                    Destroy(activePoints[0]);
                    activePoints.RemoveAt(0);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void CreatePointVisual(Vector3 position)
    {
        position += arOffset;

        GameObject point = Instantiate(pointPrefab, position, Quaternion.identity, transform);
        point.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        if (point.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = pointClr;
        }

        activePoints.Add(point);
    }
}
