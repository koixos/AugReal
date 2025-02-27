using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectSpawner : MonoBehaviour
{
    private Camera cam;
    private GameObject selectedPrefab;
    [SerializeField] private XROrigin sessionOrigin;
    [SerializeField] private float spawnDistance = 2f;

    void Start()
    {
        cam = sessionOrigin.Camera;
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
    }

    public GameObject SpawnObject()
    {
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;

        GameObject spawnedObject = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        if (!spawnedObject.TryGetComponent<Collider>(out _))
        {
            BoxCollider collider = spawnedObject.AddComponent<BoxCollider>();
                
            Renderer[] renderers = spawnedObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
                collider.center = spawnedObject.transform.InverseTransformPoint(bounds.center);
                collider.size = bounds.size;
            }
        }

        spawnedObject.AddComponent<ARAnchor>();

        XRGrabInteractable interactable = spawnedObject.AddComponent<XRGrabInteractable>();
        interactable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        interactable.throwOnDetach = false;

        spawnedObject.AddComponent<XRScaleInteractable>();

        spawnedObject.tag = "Furniture";
        return spawnedObject;
    }
}
