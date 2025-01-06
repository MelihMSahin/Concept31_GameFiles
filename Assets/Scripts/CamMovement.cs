using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public Vector3 offset;   // Offset between the camera and the player
    public float smoothSpeed = 0.125f; // Smooth movement speed

    private Vector3 targetPosition;

    void Start()
    {
        // Calculate the initial offset if not set in the Inspector
        if (offset == Vector3.zero && player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the target position based on the player's position and the offset
            targetPosition = player.position + offset;

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Keep the camera's rotation fixed (do not modify rotation)
        }
    }
}
