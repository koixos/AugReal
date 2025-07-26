using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float moveForce = 50f;

    private Rigidbody rb;
    private Vector3 inpDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inpDir = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.TransformDirection(inpDir);

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movement * moveForce);
        }
    }
}
