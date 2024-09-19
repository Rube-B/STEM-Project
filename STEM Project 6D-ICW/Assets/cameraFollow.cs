using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;        // The target the camera will follow (e.g., your airplane)
    public Vector3 offset = new Vector3(0, 5, -10); // Offset of the camera relative to the target
    public float smoothSpeed = 0.125f;  

    void Update()
    {
        // Desired position is the target's position plus the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionally, make the camera look at the target
        transform.LookAt(target);
    }
}
