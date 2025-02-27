using UnityEngine;
using UnityEngine.UI;

public class PrefabSelectionUI : MonoBehaviour
{
    public FurnitureManager furnitureManager;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public ScrollRect scrollRect;

    private ARPlacementManager placementManager;

    void Start()
    {
        GameObject xrOrigin = GameObject.Find("XR Origin");
        if (xrOrigin != null)
        {
            placementManager = xrOrigin.GetComponent<ARPlacementManager>();
        }
    }

    public void PopulateUI(string category)
    {
        if (scrollRect == null || buttonContainer == null) return;

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        if (!furnitureManager.prefabDic.ContainsKey(category)) return;

        foreach (var prefab in furnitureManager.prefabDic[category])
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.name = $"Button_{prefab.name}";

            var rectTransform = newButton.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(100, 100);

            if (newButton.TryGetComponent<Image>(out var buttonImg))
            {
                var prefabThumbnail = furnitureManager.prefabThumbnailsDic[prefab.name];
                buttonImg.sprite = prefabThumbnail.icon;
                buttonImg.color = Color.white;
                buttonImg.raycastTarget = true;
            }

            GameObject prefabCopy = prefab;
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                placementManager.SetSelectedPrefab(prefabCopy);
                placementManager.SpawnObject();
            });
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonContainer as RectTransform);
    }
}
