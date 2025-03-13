using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenSpaceController : MonoBehaviour
{
    private ActionBasedController controller;
    private XRRayInteractor interactor;
    private bool isTouching = false;

    void Start()
    {
        controller = GetComponent<ActionBasedController>();
        interactor = GetComponent<XRRayInteractor>();

        if (controller != null && controller.selectAction != null)
        {
            controller.selectAction.action.Enable();
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began && !isTouching)
            {
                isTouching = true;
                if (controller != null && controller.selectAction != null)
                {
                    controller.selectAction.action.started.Invoke(new InputAction.CallbackContext());
                }
            }
            else if ((touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled) && isTouching)
            {
                isTouching = false;
                if (controller != null && controller.selectAction != null)
                {
                    controller.selectAction.action.canceled.Invoke(new InputAction.CallbackContext());
                }
            }
        }        
    }
}
