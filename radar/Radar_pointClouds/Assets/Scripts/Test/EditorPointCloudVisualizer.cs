using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPointCloudVisualizer : MonoBehaviour
{
    public GameObject pointPrefab;
    public int totalPoints = 1000;
    public Vector3 bounds = new(5f, 3f, 5f);
    public int pointsPerSec = 40;
    public int maxVisiblePoints = 400;
    public Color pointColor = Color.green;
    public bool startSimulation = false;
    public bool generateNewPoints = false;

    private List<Vector3> allPoints = new();
    private List<GameObject> activePoints = new();

    void Start()
    {
        if (startSimulation)
        {
            GenerateRandomPointCloud();
            StartCoroutine(UpdatePointsCoroutine());
        }
    }

    void Update()
    {
        if (generateNewPoints)
        {
            generateNewPoints = false;
            ClearAllPoints();
            GenerateRandomPointCloud();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !startSimulation)
        {
            startSimulation = true;
            if (allPoints.Count == 0)
            {
                GenerateRandomPointCloud();
            }
            StartCoroutine(UpdatePointsCoroutine());
        }
    }

    void GenerateRandomPointCloud()
    {
        allPoints.Clear();

        for (int i = 0; i < totalPoints; i++)
        {
            Vector3 point = new(
                Random.Range(-bounds.x, bounds.x),
                Random.Range(-bounds.y, bounds.y),
                Random.Range(-bounds.z, bounds.z)
            );

            allPoints.Add(point);
        }
    }

    IEnumerator UpdatePointsCoroutine()
    {
        int currIndex = 0;

        while (true)
        {
            for (int i = 0; i < pointsPerSec && currIndex < allPoints.Count; i++)
            {
                CreatePointVisual(allPoints[currIndex]);
                ++currIndex;

                if (currIndex >= allPoints.Count)
                {
                    currIndex = 0;
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
        GameObject point = Instantiate(pointPrefab, position, Quaternion.identity, transform);
        point.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        if (point.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = pointColor;
        }

        activePoints.Add(point);
    }

    void ClearAllPoints()
    {
        foreach (GameObject point in activePoints)
        {
            Destroy(point);
        }

        activePoints.Clear();
    }
}
