using System.Collections.Generic;
using UnityEngine;

public class RadarDataGenerator : MonoBehaviour
{
    public int totalPoints = 1000;
    public Vector3 bounds = new(5f, 3f, 5f);
    public string fileName = "pointdata.txt";

    private List<Vector3> allPoints = new();

    void Start()
    {
        GenerateRandomPoint();
        SavePointToFile();
    }

    void GenerateRandomPoint()
    {
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

    void SavePointToFile()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        System.IO.StreamWriter writer = new(path);

        foreach (Vector3 point in allPoints)
        {
            writer.WriteLine($"{point.x},{point.y},{point.z}");
        }

        writer.Close();
    }
}
