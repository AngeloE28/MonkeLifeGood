using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Refer to last term assesment for the ui window and stuff
    [Header("UI")]
    public GameObject uiGameOverWindow; // Displays the  Game OVer window with the uiGameOverMsg
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Restarts the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Goes back to the main menu
    public void Quit()
    {
        SceneManager.LoadScene(0); // Main menu index is 0 in the build settings
    }

    // Control to end the game
    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            // Display a win message
        }
        else
        {
            // Display a lose message
        }
        uiGameOverWindow.SetActive(true);
    }
}
