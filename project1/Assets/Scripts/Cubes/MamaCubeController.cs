using UnityEngine;

public class MamaCubeController : MonoBehaviour
{
    public float speed = 0.4f;
    public float rotationDamping = 4f;
    public Camera cam;

    private bool isFrozen = false;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (isFrozen) return;

        var rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

        this.transform.position = transform.position + speed * Time.deltaTime * Vector3.up;
    }

    public void Freeze()
    {
        this.isFrozen = true;
        speed = 0;
    }

    public void Unfreeze()
    {
        this.isFrozen = false;
        speed = 0.4f;
    }
}
