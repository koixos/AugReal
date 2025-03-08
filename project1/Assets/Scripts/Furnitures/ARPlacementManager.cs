using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlacementManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARInteractionManager interactionManager;
    [SerializeField] private GameObject currPrefab;
    [SerializeField] private Camera cam;

    private static readonly List<ARRaycastHit> hits = new();

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (raycastManager == null) raycastManager = FindObjectOfType<ARRaycastManager>();
        if (interactionManager == null) interactionManager = FindObjectOfType<ARInteractionManager>();
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        currPrefab = prefab;
    }

    public void SpawnObject()
    {
        if (currPrefab == null) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            interactionManager.CreateInteractableObject(currPrefab, pose.position, pose.rotation);
            
            /*Vector3 eulerAngles = cam.transform.eulerAngles;
            spawnedObject.transform.eulerAngles = new Vector3(-90, eulerAngles.y, -90);
        */}
    }

    /*public void TogglePlaneVisualization(bool show)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(show);
        }
    }*/
}
