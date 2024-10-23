using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewMovement : MonoBehaviour
{
    private Rigidbody rb; // Player's Rigidbody
    public float moveSpeed;
    private Vector3 _moveDirection;

    // Input actions
    public InputActionReference move;
    public InputActionReference interact;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Automatically get the player's Rigidbody
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned! Please add a Rigidbody to this GameObject.");
        }
    }

    private void OnEnable()
    {
        move.action.Enable();
        interact.action.Enable();
        interact.action.performed += _ => Interact();
    }

    private void OnDisable()
    {
        move.action.Disable();
        interact.action.Disable();
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (rb != null) // Ensure rb is not null before accessing it
        {
            Vector3 movement = new Vector3(_moveDirection.x, 0, _moveDirection.y) * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
            }
        }
    }

    private void Interact()
    {
        Debug.Log("Interact action performed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CombatTrigger")) // Change to the correct tag for your trigger
        {
            SceneManager.LoadScene("Combat");
        }
    }
}
