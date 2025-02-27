using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectManipulator : MonoBehaviour
{
    private Camera cam;
    private ARRaycastManager raycastManager;
    private readonly List<ARRaycastHit> hits = new();
    private Vector3 initialScale;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isSelected = false;
    private float previousDistance;

    [SerializeField] private float movementSmoothing = 0.1f;
    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float maxScale = 3f;

    void Start()
    {
        cam = Camera.main;
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == this.transform)
                    {
                        offset = transform.position - hit.point;
                        isSelected = true;
                        isDragging = true;
                    }
                }
            }
            else if (isDragging && isSelected && touch.phase == TouchPhase.Moved)
            {
                if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    Vector3 targetPos = hitPose.position + offset;
                    transform.position = Vector3.Lerp(transform.position, targetPos, movementSmoothing);
                }               
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                isSelected = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (!isSelected)
            {
                Ray ray0 = cam.ScreenPointToRay(t0.position);
                Ray ray1 = cam.ScreenPointToRay(t1.position);

                bool h0Success = Physics.Raycast(ray0, out RaycastHit h0);
                bool h1Success = Physics.Raycast(ray1, out RaycastHit h1);

                if (h0Success || h1Success)
                {
                    if ((h0Success && h0.transform == this.transform) ||
                        (h1Success && h1.transform == this.transform))
                    {
                        isSelected = true;
                        initialScale = transform.localScale;
                    }
                }

                if (isSelected)
                {
                    previousDistance = Vector2.Distance(t0.position, t1.position);
                    initialScale = transform.localScale;
                }
            }

            if (isSelected)
            {
                if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
                {
                    float currDistance = Vector2.Distance(t0.position, t1.position);
                    float scaleFactor = currDistance / previousDistance;

                    Vector3 newScale = transform.localScale * scaleFactor;
                    
                    transform.localScale = Vector3.Lerp(transform.localScale, newScale, movementSmoothing);

                    previousDistance = currDistance;
                }
            }
        }
        else
        {
            isDragging = false;
            isSelected = false;
        }
    }
}
