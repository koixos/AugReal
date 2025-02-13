using UnityEngine;
using UnityEngine.UI;

public class PrefabSelectionUI : MonoBehaviour
{
    public FurnitureManager furnitureManager;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public ObjectSpawner spawner;
    public ScrollRect scrollRect;

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

            newButton.GetComponent<Button>().onClick.AddListener(() => spawner.SetSelectedPrefab(prefab));
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonContainer as RectTransform);
    }
}
