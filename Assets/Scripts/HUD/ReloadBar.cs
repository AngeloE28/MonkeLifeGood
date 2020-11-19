using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public GunWithBullets reload;
    public Image barTimer;
    // Start is called before the first frame update
    void Start()
    {
        barTimer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(reload.reloadTime > 0)
        {
            barTimer.fillAmount = reload.reloadBarTimer / reload.totalReloadTime;
        }
    }
}
