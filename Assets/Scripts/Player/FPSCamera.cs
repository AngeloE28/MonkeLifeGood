using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float mouseSensitivity;  // The aim sensitiviy
    public float hipFireSensitivity = 100f; // The aim sensitivity when player is not aiming
    private float aimDownSightSensitivity = 2f; // divides the hip fire sensitivity by 2
    public Transform player;    // Reference to player

    private float xRotation = 0f;   // Max rotation of the camera

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume_Slider", AudioListener.volume);
        hipFireSensitivity = PlayerPrefs.GetFloat("Sensitivity_Slider", hipFireSensitivity);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        // Slows down mouse sensitivity when player is aiming down sight
        if (Input.GetButton("Fire2"))
        {
            mouseSensitivity = hipFireSensitivity / aimDownSightSensitivity;
        }
        else
        {
            mouseSensitivity = hipFireSensitivity;
        }
    }
}
