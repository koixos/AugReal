using UnityEngine;

public class SimplePointTest : MonoBehaviour
{
    public GameObject pointPrefab;
    public int numOfPoints = 100;
    public float spread = 5f;
    public bool createPoints = false;

    void Update()
    {
        if (createPoints || Input.GetKeyDown(KeyCode.Space))
        {
            createPoints = false;
            CreatePoints();
        }    
    }

    void CreatePoints()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numOfPoints; i++)
        {
            Vector3 randPos = new(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread)
            );

            GameObject point = Instantiate(pointPrefab, randPos, Quaternion.identity, transform);
            point.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            if (point.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                );
            }
        }
    }
}
