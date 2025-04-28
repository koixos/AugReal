using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;

public class UDPPointCloudReceiver : MonoBehaviour
{
    public GameObject pointPrefab;
    public int maxPoints = 200;

    UdpClient udpClient;
    IPEndPoint endPoint;

    private Thread pointDeletionThread;
    private bool isProcessing = false;
    private readonly List<GameObject> spawnedPoints = new();

    void Start()
    {
        udpClient = new UdpClient(5055);
        endPoint = new IPEndPoint(IPAddress.Any, 5055);
        udpClient.BeginReceive(ReceiveData, null);
    }

    void Update()
    {
        if (isProcessing)
        {
            pointDeletionThread.Join();
            isProcessing = false;
        }
    }

    void ReceiveData(System.IAsyncResult ar)
    {
        byte[] data = udpClient.EndReceive(ar, ref endPoint);
        string json = System.Text.Encoding.UTF8.GetString(data);

        Debug.Log("Gelen veri: " + json);

        try
        {
            List<List<float>> points = JsonConvert.DeserializeObject<List<List<float>>>(json);
            MainThreadDispatcher.Enqueue(() => UpdatePoints(points));
        }
        catch (JsonException e)
        {
            Debug.LogError("JSON deserialization error: " + e.Message);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }

        udpClient.BeginReceive(ReceiveData, null);
    }

    void UpdatePoints(List<List<float>> points)
    {
        while (spawnedPoints.Count > maxPoints)
        {
            Destroy(spawnedPoints[0]);
            spawnedPoints.RemoveAt(0);
        }

        float screenWidth = 640f;
        float screenHeight = 480f;

        foreach (var point in points)
        {
            // normalize points, range 0 to 1
            float xNorm = point[0] / screenWidth;
            float yNorm = point[1] / screenHeight;

            // convert to screen coordinates, range -1 to 1
            float xScreen = (xNorm - 0.5f) * 2f;
            float yScreen = (0.5f - yNorm) * 2f;

            // fake depth
            float zScreen = Random.Range(1.0f, 1.5f);

            Vector3 position = new(xScreen, yScreen, zScreen);
            GameObject pointObject = Instantiate(pointPrefab, position, Quaternion.identity);
            spawnedPoints.Add(pointObject);
        }
    }
}
