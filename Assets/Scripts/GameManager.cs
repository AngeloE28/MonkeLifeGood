using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Refer to last term assesment for the ui window and stuff
    [Header("UI")]
    public GameObject uiGameOverWindow; // Displays the  Game OVer window with the uiGameOverMsg
    public TMP_Text uiGameOverMsg; // Displays the game over message depending on the outcome of the game (win or lose)
    public Image harambe; // Displays a memoriam of harambe

    [Header("Gameplay Loop")]
    public bool isGameRunning; //Is the gameplay part of the game currently active?

    // Start is called before the first frame update
    void Start()
    {
        isGameRunning = true; // Game is running
    }

    // Restarts the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Goes back to the main menu
    public void QuitGame()
    {
        SceneManager.LoadScene(0); // Main menu index is 0 in the build settings
    }

    // Control to end the game
    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            uiGameOverMsg.text = "Family saved! LIFE GOOD";
        }
        else
        {
            uiGameOverMsg.text = "Family gone...Regret.";
            harambe.gameObject.SetActive(true);
        }
        uiGameOverWindow.SetActive(true);
    }
}
