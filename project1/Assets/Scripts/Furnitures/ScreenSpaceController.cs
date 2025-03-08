using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenSpaceController : MonoBehaviour
{
    private ActionBasedController controller;
    private XRRayInteractor interactor;

    void Start()
    {
        controller = GetComponent<ActionBasedController>();
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
                if (controller != null && controller.selectAction != null)
                {
                    controller.selectAction.action.Enable();
                   // controller.selectAction.action.started.Invoke(new UnityEngine.InputSystem.InputAction.CallbackContext());
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (controller != null && controller.selectAction != null)
                {
                    //controller.selectAction.action.canceled.Invoke(new UnityEngine.InputSystem.InputAction.CallbackContext());
                }
            }
        }        
    }
}
