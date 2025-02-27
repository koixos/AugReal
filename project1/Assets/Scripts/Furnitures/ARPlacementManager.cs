using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlacementManager : MonoBehaviour
{
    private readonly List<ARRaycastHit> hits = new();
    private GameObject selectedPrefab;
    private Pose placementPose;
    private bool validPlacement = false;

    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject placementIndicator;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            Vector2 screenCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);
            validPlacement = raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

            if (validPlacement)
            {
                placementPose = hits[0].pose;
                if (placementIndicator != null)
                {
                    placementIndicator.SetActive(true);
                    placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
                }
            }
        }        
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
    }

    public void SpawnObject()
    {
        if (selectedPrefab != null && validPlacement)
        {
            GameObject spawnedObject = Instantiate(selectedPrefab, placementPose.position, placementPose.rotation);

            spawnedObject.AddComponent<ARAnchor>();
            spawnedObject.AddComponent<ObjectManipulator>();

            if (!spawnedObject.TryGetComponent<BoxCollider>(out var collider))
            {
                collider = spawnedObject.AddComponent<BoxCollider>();
            }

            if (spawnedObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                collider.size = meshRenderer.bounds.size;
                collider.center = meshRenderer.bounds.center;
            }
            
            /*Vector3 eulerAngles = cam.transform.eulerAngles;
            spawnedObject.transform.eulerAngles = new Vector3(-90, eulerAngles.y, -90);
        */}
    }

    public void TogglePlaneVisualization(bool show)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(show);
        }
    }
}
