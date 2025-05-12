using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{

    public string sceneToLoad;


  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Trigger");
            playerController controller = other.GetComponent<playerController>();
            if (controller != null)
            {
                
                controller.NextScene = sceneToLoad;
                controller.isPlayerInRange = true;
                UIManager.Instance?.ShowInteractionPrompt("Press B to enter");
            }
        }
    }

 private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Trigger");
            playerController controller = other.GetComponent<playerController>();
            if (controller != null)
            {
                controller.NextScene = null;
                controller.isPlayerInRange = false;
                UIManager.Instance?.HideInteractionPrompt();
            }
        }
    }

}
