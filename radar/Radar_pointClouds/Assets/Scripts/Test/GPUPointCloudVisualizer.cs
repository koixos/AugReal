using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPointCloudVisualizer : MonoBehaviour
{
    public Material material;
    public int totalPoints = 1000;
    public Vector3 bounds = new(5f, 3f, 5f);
    public int pointsPerSec = 40;
    public int maxVisiblePoints = 400;
    public float pointSize = 0.05f;
    public bool startSimulation = false;

    private List<Vector3> allPoints = new();
    private List<Vector3> activePoints = new();
    private ComputeBuffer pointsBuffer;

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
        if (Input.GetKeyDown(KeyCode.Space) && !startSimulation)
        {
            startSimulation = true;
            if (allPoints.Count == 0)
            {
                GenerateRandomPointCloud();
            }
            StartCoroutine(UpdatePointsCoroutine());

            if (activePoints.Count > 0)
            {
                RenderPoints();
            }
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
                activePoints.Add(allPoints[currIndex]);
                ++currIndex;

                if (currIndex >= allPoints.Count)
                {
                    currIndex = 0;
                }
            }

            if (activePoints.Count > maxVisiblePoints)
            {
                int removeCount = activePoints.Count - maxVisiblePoints;
                activePoints.RemoveRange(0, removeCount);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void RenderPoints()
    {
        pointsBuffer?.Release();

        pointsBuffer = new ComputeBuffer(activePoints.Count, sizeof(float) * 3);
        pointsBuffer.SetData(activePoints.ToArray());

        material.SetBuffer("_PointsBuffer", pointsBuffer);
        material.SetFloat("_PointSize", pointSize);

        material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, activePoints.Count);
    }

    void OnDestroy()
    {
        if (pointsBuffer != null)
        {
            pointsBuffer.Release();
            pointsBuffer = null;
        }    
    }
}
