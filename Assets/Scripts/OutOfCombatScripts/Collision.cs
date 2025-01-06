using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject textboxPanel;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            // Show the textbox
            if (textboxPanel != null)
            {
                textboxPanel.SetActive(true);
            }
        }
    }
}
