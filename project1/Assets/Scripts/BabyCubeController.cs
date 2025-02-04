using UnityEngine;

public class BabyCubeController : MonoBehaviour
{
    public float speed = 0.4f;
    public float rotationDamping = 4f;
    public Transform MamaCube;

    private bool isFrozen = false;

    void Start()
    {
        MamaCube = GameObject.FindGameObjectWithTag("MamaCube").GetComponent<Transform>();

        GameObject[] mamaCubes = GameObject.FindGameObjectsWithTag("MamaCube");
        int randomMama = Random.Range(0, mamaCubes.Length);
        MamaCube = mamaCubes[randomMama].GetComponent<Transform>();
    }

    void Update()
    {
        if (isFrozen) return;

        var rotation = Quaternion.LookRotation(MamaCube.transform.position - transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

        float step = speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(transform.position, MamaCube.position, step);
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
