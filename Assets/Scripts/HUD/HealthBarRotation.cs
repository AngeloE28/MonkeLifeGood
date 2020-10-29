using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarRotation : MonoBehaviour
{
    public Transform playerCam;

    private void Start()
    {
        playerCam = GameObject.Find("Main Camera").transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + playerCam.forward);
    }
}
