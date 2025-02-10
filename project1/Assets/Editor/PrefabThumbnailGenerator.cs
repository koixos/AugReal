using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabThumbnailGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate Prefab Thumbnail")]
    public static void GeneratePrefabThumbnail()
    {
        string prefabFoldersPath = "Assets/Assets/BigFurniturePack/Prefabs";
        string outFolderPath = "Assets/Assets/BigFurniturePack/PrefabThumbnails";
        string iconFolderPath = "Assets/Images/Furnitures/Icons";

        if (!Directory.Exists(outFolderPath))
        {
            Directory.CreateDirectory(outFolderPath);
        }

        string[] prefabSubDirectoryPaths = Directory.GetDirectories(prefabFoldersPath);

        foreach (var path in prefabSubDirectoryPaths)
        {
            string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { path });

            foreach (string guid in prefabGUIDs)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefab == null) continue;

                PrefabThumbnail prefabThumbnail = ScriptableObject.CreateInstance<PrefabThumbnail>();
                prefabThumbnail.prefab = prefab;

                string iconPath = $"{iconFolderPath}/{prefab.name}.png";
                prefabThumbnail.icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);

                string outPath = $"{outFolderPath}/{prefab.name}.asset";
                AssetDatabase.CreateAsset(prefabThumbnail, outPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
