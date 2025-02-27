using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenSpaceController : MonoBehaviour
{
    private XRController controller;
    private XRRayInteractor interactor;

    void Start()
    {
        controller = GetComponent<XRController>();
        interactor = GetComponent<XRRayInteractor>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            interactor.TryGetCurrent3DRaycastHit(out _);

            if (touch.phase == TouchPhase.Began)
            {
                controller.
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                controller.
            }
        }        
    }
}
