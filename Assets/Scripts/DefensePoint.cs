using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{
    // Health of the defense point
    public float health = 200f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Defense point takes damage
    public void DefenseTakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
