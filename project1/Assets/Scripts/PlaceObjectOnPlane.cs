using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Vector3 camPosition = Camera.main.transform.position;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
        {
            return;
        }

        PlaceObject(finger);
    }

    private void PlaceObject(EnhancedTouch.Finger finger)
    {
        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (var hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(prefab, pose.position, pose.rotation);

                if (planeManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                {
                    Vector3 position = obj.transform.position;
                    Vector3 direction = camPosition - position;
                    Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
                    Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized); // (0, 1, 0)

                    Quaternion targetRotation = Quaternion.Euler(scaledEuler);
                    obj.transform.rotation = obj.transform.rotation * targetRotation;
                }
            }
        }
    }
}
