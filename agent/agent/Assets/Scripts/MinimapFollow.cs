using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new(0, 20, 0);

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = player.position + offset;
            newPosition.y = offset.y;
            transform.position = newPosition;
            transform.LookAt(player);
        }
    }
}
