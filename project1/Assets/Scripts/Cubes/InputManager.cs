using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Toggle Toggle;
    public GameObject MamaCubePrefab;
    public GameObject BabyCubePrefab;
    public Camera Camera;

    public GameObject SelectionUI;
    public Button KillButton;
    public Button ChangeColorButton;

    private GameObject SpawnedCubePrefab;
    private GameObject SelectedPrefab;

    void Start()
    {
        SelectionUI.SetActive(false);
        KillButton.onClick.AddListener(Kill);
        ChangeColorButton.onClick.AddListener(ChangeColor);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            
            if (IsPointerOverUI(Input.mousePosition)) return;

            GameObject clickedPrefab = GetClickedObject(ray, out _);

            if (clickedPrefab != null)
            {
                if (clickedPrefab.CompareTag("MamaCube") || clickedPrefab.CompareTag("BabyCube"))
                {
                    SelectObject(clickedPrefab);
                }

            }
            else
            {
                SpawnedCubePrefab = Instantiate(WhichBird(), ray.origin, Quaternion.identity);
                SpawnedCubePrefab.GetComponent<Rigidbody>().AddForce(ray.direction * 20);

                if (SelectedPrefab != null)
                {
                    DeselectObject();
                }
            }
        }
    }

    public void Kill()
    {
        if (SelectedPrefab != null)
        {
            Destroy(SelectedPrefab);
            DeselectObject();
        }
    }

    public void ChangeColor()
    {
        if (SelectedPrefab != null)
        {
            SelectedPrefab.GetComponent<Renderer>().material.color = Random.ColorHSV();
            DeselectObject();
        }
    }

    private GameObject WhichBird()
    {
        return Toggle.isOn ? MamaCubePrefab : BabyCubePrefab;
    }

    private GameObject GetClickedObject(Ray ray, out RaycastHit hit)
    {
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private bool IsPointerOverUI(Vector2 finger)
    {
        if (EventSystem.current == null) return false;

        PointerEventData ped = new(EventSystem.current)
        {
            position = finger
        };

        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(ped, raycastResults);
        return raycastResults.Count > 0;
    }

    private void SelectObject(GameObject obj)
    {
        if (SelectedPrefab != null)
        {
            DeselectObject();
        }

        SelectedPrefab = obj;
        
        if (SelectedPrefab.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = true;
        }

        if (SelectedPrefab.TryGetComponent<BabyCubeController>(out var babyCubeController))
        {
            babyCubeController.Freeze();
        }
        else if (SelectedPrefab.TryGetComponent<MamaCubeController>(out var mamaCubeController))
        {
            mamaCubeController.Freeze();
        }

        StartCoroutine(RotateObject(SelectedPrefab));
        SelectionUI.SetActive(true);
    }

    private void DeselectObject()
    {
        if (SelectedPrefab.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = false;
        }

        if (SelectedPrefab.TryGetComponent<BabyCubeController>(out var babyCubeController))
        {
            babyCubeController.Unfreeze();
        }
        else if (SelectedPrefab.TryGetComponent<MamaCubeController>(out var mamaCubeController))
        {
            mamaCubeController.Unfreeze();
        }

        StopAllCoroutines();
        SelectedPrefab = null;
        SelectionUI.SetActive(false);
    }

    IEnumerator RotateObject(GameObject obj)
    {
        while (SelectedPrefab == obj)
        {
            if (obj == null) yield break;
            obj.transform.Rotate(20 * Time.deltaTime * Vector3.up);
            yield return null;
        }
    }
}
