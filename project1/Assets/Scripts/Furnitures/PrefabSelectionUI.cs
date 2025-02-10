using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class PrefabSelectionUI : MonoBehaviour
{
    public FurnitureManager furnitureManager;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public ObjectSpawner spawner;
    
    public void PopulateUI(string category)
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        if (!furnitureManager.prefabDic.ContainsKey(category)) return;

        foreach (var prefab in furnitureManager.prefabDic[category])
        {
            if (!furnitureManager.prefabThumbnailsDic.ContainsKey(prefab.name)) continue;

            var prefabThumbnail = furnitureManager.prefabThumbnailsDic[prefab.name];

            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.GetComponent<Image>().sprite = prefabThumbnail.icon;

            newButton.GetComponent<Button>().onClick.AddListener(() => spawner.SetSelectedPrefab(prefab));
        }
    }
}
