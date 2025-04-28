using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private WebCamTexture wcTexture;

    void Start()
    {
        wcTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = wcTexture;
        wcTexture.Play();
    }
}
