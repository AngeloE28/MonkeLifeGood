using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads the Options scene
    public void Options()
    {
        SceneManager.LoadScene(1); // Options menu index is 1
    }

    // Loads the Game scene
    public void LoadGame()
    {
        SceneManager.LoadScene(2); // Gmae Scene index is 2
    }

    // Quits the build application
    public void QuitGame()
    {
        Application.Quit();
    }
}
