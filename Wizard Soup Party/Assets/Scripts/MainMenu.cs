using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 
    public GameObject introOverlayImage; // Drag your image here
    public Button startButton;
    void Start()
    {
        // Automatically select the Start button when the scene loads
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }


    public void StartGame()
    {
        StartCoroutine(ShowIntroImageThenLoad());
        //SceneManager.LoadScene("Test - Environment imported"); // Replace with your real scene name
    }


    private IEnumerator ShowIntroImageThenLoad()
    {
        introOverlayImage.SetActive(true); // Show the story image
        EventSystem.current.SetSelectedGameObject(null); // Clear button focus

        // Wait for any input
        yield return new WaitUntil(() =>
            Input.anyKeyDown || 
            Input.GetAxis("Horizontal") != 0 || 
            Input.GetAxis("Vertical") != 0 || 
            Input.GetButtonDown("Submit")
        );

        SceneManager.LoadScene("Test - Environment imported");
    }


    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
