using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    public Boss boss;
    public bool takeDamage;

    private void Update()
    {
        if(boss.bossTakeDamage)
        {
            takeDamage = true;
        }
        else
        {
            takeDamage = false;
        }
    }
    public void TrailerTakeDamage(float amount)
    {
        if (takeDamage)
        {
            boss.bossHealth -= amount;
        }
    }
}
