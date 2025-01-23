using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private Vector3 prefabOffset;

    private GameObject dragon;
    private ARTrackedImageManager imageManager;

    private void OnEnable()
    {
        imageManager = gameObject.GetComponent<ARTrackedImageManager>();
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage img in obj.added)
        {
            dragon = Instantiate(dragonPrefab, img.transform);
            dragon.transform.position += prefabOffset;
        }
    }
}
