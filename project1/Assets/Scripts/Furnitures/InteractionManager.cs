using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public Camera Cam;
    public GameObject CategorySelectionUI;
    public GameObject PrefabSelectionUI;
    public Button KillButton;

    private GameObject SelectedPrefab;

    void Start()
    {
        KillButton.onClick.AddListener(Kill);    
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            GameObject clickedPrefab = GetClickedObject(ray, out _);

            if (clickedPrefab != null && clickedPrefab.CompareTag("Furniture"))
            {
                SelectObject(clickedPrefab);
            }
            else
            {
                if (!IsPointerOverUI(Input.mousePosition))
                {
                    CategorySelectionUI.SetActive(false);
                    PrefabSelectionUI.SetActive(false);
                }

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

    private GameObject GetClickedObject(Ray ray, out RaycastHit hit)
    {
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void SelectObject(GameObject obj)
    {
        if (SelectedPrefab != null)
        {
            DeselectObject();
        }

        SelectedPrefab = obj;
        KillButton.gameObject.SetActive(true);
    }

    private void DeselectObject()
    {
        SelectedPrefab = null;
        KillButton.gameObject.SetActive(false);
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
}
