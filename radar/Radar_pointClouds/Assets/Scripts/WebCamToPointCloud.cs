using UnityEngine;

public class WebCamToPointCloud : MonoBehaviour
{
    public GameObject pointPrefab;
    public int pointDensity = 10;   // 10 point per pixel

    private WebCamTexture webCamTexture;

    void Start()
    {
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
    }

    void Update()
    {
        if (webCamTexture.didUpdateThisFrame)
        {
            ShowPointCloud(webCamTexture);
        }
    }

    void ShowPointCloud(WebCamTexture webCamTexture)
    {
        Color32[] pixels = webCamTexture.GetPixels32();
        int width = webCamTexture.width;
        int height = webCamTexture.height;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int y = 0; y < height; y += pointDensity)
        {
            for (int x = 0; x < width; x += pointDensity)
            {
                int index = x + y * width;
                Color32 pixelColor = pixels[index];
                if (pixelColor.r + pixelColor.g + pixelColor.b > 400)
                {
                    Vector3 position = new(x/100f, y/100f, Mathf.Sin(Time.time + x*0.1f) * 0.2f);
                    GameObject point = Instantiate(pointPrefab, position, Quaternion.identity, transform);
                    point.transform.localScale = Vector3.one * 0.02f;
                    point.GetComponent<Renderer>().material.color = pixelColor;
                    
                }
            }
        }
    }
}
