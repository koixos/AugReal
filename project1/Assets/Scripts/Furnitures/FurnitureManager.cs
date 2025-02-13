using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> prefabDic = new();
    public Dictionary<string, PrefabThumbnail> prefabThumbnailsDic = new();

    void Awake()
    {
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        string[] categories = { "Bathroom", "Beds", "Cabinets&Racks", "Lights", "Mirrors", "Modular Kitchen", "Sofas&Chairs", "Tables", "Vases" };

        foreach (string category in categories)
        {
            GameObject[] prefabs = Resources.LoadAll<GameObject>($"BigFurniturePack/Prefabs/{category}");
            prefabDic.Add(category, prefabs);

            foreach (GameObject prefab in prefabs)
            {
                PrefabThumbnail prefabThumbnail = Resources.Load<PrefabThumbnail>($"BigFurniturePack/PrefabThumbnails/{prefab.name}");
                if (prefabThumbnail != null)
                {
                    prefabThumbnailsDic.Add(prefab.name, prefabThumbnail);
                }
            }
        }
    }
}
