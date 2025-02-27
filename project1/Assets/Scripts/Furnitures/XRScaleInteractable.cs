using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRScaleInteractable : MonoBehaviour
{
    public float minScale = 0.3f;
    public float maxScale = 3f;
    public float scaleSensitivity = 0.1f;

    private float initialTouchDist;
    private Vector3 initialScale;
    private bool isScaling = false;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();

                if (interactable != null && interactable.isSelected)
                {
                    initialTouchDist = Vector2.Distance(t0.position, t1.position);
                    initialScale = transform.localScale;
                    isScaling = true;
                }
                else if ((t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved) && isScaling)
                {
                    float currDist = Vector2.Distance(t0.position, t1.position);
                    float scaleFactor = currDist / initialTouchDist;

                    Vector3 newScale = initialScale * scaleFactor;

                    newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                    newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                    newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

                    transform.localScale = newScale;
                }
                else if (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended)
                {
                    isScaling = false;
                }
            }
        }
    }
}
