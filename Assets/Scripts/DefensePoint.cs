using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefensePoint : MonoBehaviour
{
    // Health of the defense point
    public float currentDefenseHealth;
    public float maxDefenseHealth = 200f;

    public GameManager myGameManager; // Ref to game Manager
    public Player player;
    public HealthBar healthBar;

    private void Start()
    {
        currentDefenseHealth = maxDefenseHealth;
        healthBar.SetMaxhealth(maxDefenseHealth);
        player = GameObject.Find("Player").GetComponent<Player>(); // Finds gameobject with tag of player
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script
    }

    // Defense point takes damage
    public void DefenseTakeDamage(float amount)
    {
        currentDefenseHealth -= amount;

        healthBar.SetHealth(currentDefenseHealth);
        if (currentDefenseHealth <= 0f)
        {
            player.EndGame(false);  
        }
    }
}
