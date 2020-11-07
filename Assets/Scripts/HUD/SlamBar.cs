using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlamBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxCharge(float maxCharge)
    {
        slider.maxValue = maxCharge;
    }

    public void SetCharge(float charge)
    {
        slider.value = charge;
    }
}
