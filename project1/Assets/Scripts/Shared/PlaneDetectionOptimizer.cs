using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionOptimizer : MonoBehaviour
{
    private ARPlaneManager planeManager;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        foreach (var plane in planeManager.trackables)
        {
            ValidatePlane(plane);
        }
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            plane.gameObject.SetActive(true);
        }

        foreach (var plane in args.updated)
        {
            ValidatePlane(plane);
        }
    }

    void ValidatePlane(ARPlane plane)
    {
        if (plane.extents.x < 0.3f || plane.extents.y < 0.3f)
        {
            plane.gameObject.SetActive(false);
        }
    }

    void StopPlaneDetection()
    {
        planeManager.enabled = false;
    }

    void SimplifyPlaneVisual(ARPlane plane)
    {
        plane.GetComponent<MeshRenderer>().enabled = false;
    }
}
