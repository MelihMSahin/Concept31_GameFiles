using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour
{
    public Rigidbody rb;  // Change from Rigidbody2D to Rigidbody
    public float moveSpeed;
    private Vector3 _moveDirection;
    public InputActionReference move;

    private void Update()
    {
        // Read the input as a Vector2
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Create a movement vector in the 3D space (use y for vertical movement if needed)
        Vector3 movement = new Vector3(_moveDirection.x, 0, _moveDirection.y) * moveSpeed;

        // Apply the movement to the Rigidbody
        rb.velocity = movement;  // Directly set the velocity
    }
}