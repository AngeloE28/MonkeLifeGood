using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    void Start()
    {
        sensitivitySlider.maxValue = 1000f;
        sensitivitySlider.value = 100f;
        volumeSlider.value = 1f;
    }

    // Goes back to the Main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Sets the volume and saves it
    public void SetVoume(float volume)
    {
        PlayerPrefs.SetFloat("Volume_Slider", volume);
    }

    // Sets the player's sensitivity
    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity_Slider", sensitivity);
    }
}
