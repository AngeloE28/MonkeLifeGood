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
    public GameObject uiPauseWindow;
    public FPSCamera myCam;

    [Header("Gameplay Loop")]
    public bool isGameRunning; //Is the gameplay part of the game currently active?
    public bool isGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
        isGameRunning = true; // Game is running
        myCam = GameObject.Find("Main Camera").GetComponent<FPSCamera>(); // Gets the FPScamera script
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resumes game
    public void Resume()
    {
        uiPauseWindow.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myCam.enabled = true;
    }

    // Pauses game
    public void Pause()
    {
        uiPauseWindow.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        myCam.enabled = false;
    }

    // Restarts the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Goes back to the main menu
    public void QuitGame()
    {
        Time.timeScale = 1f;
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
