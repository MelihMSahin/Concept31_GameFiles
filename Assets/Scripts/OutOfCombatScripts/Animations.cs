using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animations : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Vector2 moveInput; // Stores movement input

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Input System callback for movement using InputValue
    public void OnMove(InputValue value)
    {
        // Read the movement input (Vector2)
        moveInput = value.Get<Vector2>();
        Debug.Log("OnMove called with input: " + moveInput); // Debug for testing
    }

    public void Update()
    {
        // Convert moveInput to a Vector3 for 3D movement
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        // Check if there is any movement
        bool isWalking = movement.magnitude > 0;

        if (isWalking)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("idle", true);

            // Forcefully transition to idle animation
            animator.CrossFade("Idle", 0f); // Replace "Idle" with the actual name of your idle animation
        }

        Debug.Log("Update Move Input: " + moveInput + ", isWalking: " + isWalking + ", idle: " + !isWalking);
    }
}




