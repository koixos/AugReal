using UnityEditor;
using UnityEngine;

public class AlignToGround : EditorWindow
{
    [MenuItem("Tools/Align To Ground %g")]
    static void DropToGround()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RaycastHit hit;
            if (Physics.Raycast(obj.transform.position + Vector3.up * 2f, Vector3.down, out hit, 100f))
            {
                Undo.RecordObject(obj.transform, "Align To Ground");
                obj.transform.position = hit.point;
            }
        }
    }
}
