using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        
    public Vector3 camOffset;          

    void LateUpdate()
    {
        
        Vector3 desiredPosition = player.position + player.rotation * camOffset;

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1f);

        
        transform.position = smoothedPosition;

        transform.LookAt(player);
    }
}
