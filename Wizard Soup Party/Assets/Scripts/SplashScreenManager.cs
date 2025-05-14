using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
public string menuSceneName = "MainMenu"; // Set this in inspector or hardcode

    void Update()
    {
        if (AnyKeyPressed())
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    private bool AnyKeyPressed()
    {
        return Input.anyKeyDown || 
               Input.GetAxis("Horizontal") != 0 ||
               Input.GetAxis("Vertical") != 0 ||
               Input.GetButtonDown("Submit");
    }
}
