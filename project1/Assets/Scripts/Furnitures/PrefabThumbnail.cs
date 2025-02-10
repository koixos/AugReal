using UnityEngine;

[CreateAssetMenu(fileName = "NewPrefabThumbnail", menuName = "Prefab Thumbnail")]
public class PrefabThumbnail : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
}
