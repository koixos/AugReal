using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ARPlacementManager placementManager;

    void Start()
    {
        GameObject xrOrigin = GameObject.Find("XR Origin");
        if (xrOrigin != null)
        {
            placementManager = xrOrigin.GetComponent<ARPlacementManager>();
        }
    }

    public void OnPrefabSelected(GameObject prefab)
    {
        placementManager.SetSelectedPrefab(prefab);
    }

    public void TogglePlanes(bool show)
    {
        //placementManager.TogglePlaneVisualization(show);
    }

    public void SetupARInteraction(Camera cam)
    {
        cam.gameObject.AddComponent<XRRayInteractor>();
        cam.gameObject.AddComponent<XRInteractorLineVisual>();

        XRController controller = cam.gameObject.AddComponent<XRController>();
        controller.updateTrackingType = XRController.UpdateType.Update;

        cam.gameObject.AddComponent<ScreenSpaceController>();
    }
}
