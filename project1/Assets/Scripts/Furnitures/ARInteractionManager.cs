using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ARRaycastManager raycastManager;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (raycastManager == null) raycastManager = FindObjectOfType<ARRaycastManager>();

        SetupARInteraction(cam);
    }

    public void SetupARInteraction(Camera cam)
    {
        if (!cam.TryGetComponent<XRRayInteractor>(out _))
        {
            cam.gameObject.AddComponent<XRRayInteractor>();
            cam.gameObject.AddComponent<XRInteractorLineVisual>();
        }

        if (!cam.TryGetComponent<ActionBasedController>(out _))
        {
            cam.gameObject.AddComponent<ActionBasedController>();
        }

        if (!cam.TryGetComponent<ScreenSpaceController>(out _))
        {
            cam.gameObject.AddComponent<ScreenSpaceController>();
        }
    }

    public GameObject CreateInteractableObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, position, rotation);

        if (obj.GetComponent<Collider>() == null)
        {
            BoxCollider collider = obj.AddComponent<BoxCollider>();

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
                collider.center = obj.transform.InverseTransformPoint(bounds.center);
                collider.size = bounds.size;
            }
        }

        obj.AddComponent<ARAnchor>();

        XRGrabInteractable grabInteractable = obj.AddComponent<XRGrabInteractable>();
        grabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        grabInteractable.throwOnDetach = false;

        obj.AddComponent<XRScaleInteractable>();

        obj.tag = "Furniture";
        return obj;
    }

    public void DeleteSelectedObject()
    {
        var interactionManager = FindObjectOfType<XRInteractionManager>();
        if (interactionManager == null) return;

        var interactors = FindObjectsOfType<XRBaseInteractor>();
        foreach (var interactor in interactors)
        {
            if (interactor is XRRayInteractor rayInteractor)
            {
                if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    GameObject hitObj = hit.collider.gameObject;
                    XRGrabInteractable grabInteractable = hitObj.GetComponentInParent<XRGrabInteractable>();
                    if (grabInteractable != null)
                    {
                        Destroy(grabInteractable.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
