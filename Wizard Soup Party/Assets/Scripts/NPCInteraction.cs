using UnityEngine;
using System.Collections.Generic;

public class NPCInteraction : MonoBehaviour
{
 [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public List<string> dialogueLines = new List<string>()
    {
        "Hello there, traveler!",
        "The forest is full of surprises.",
        "Be sure to gather mushrooms!"
    };

    [Header("Prompt UI (Optional)")]
    public GameObject promptUI;  // Assign a "Press E to talk" UI object in the inspector

    private bool playerInRange = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }
}
