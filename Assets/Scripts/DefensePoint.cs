using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{
    // Health of the defense point
    public float defenseHealth = 200f;

    public GameManager myGameManager; // Ref to game Manager
    public Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>(); // Finds gameobject with tag of player
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script
    }

    // Defense point takes damage
    public void DefenseTakeDamage(float amount)
    {
        defenseHealth -= amount;
        if (defenseHealth <= 0f)
        {
            player.EndGame(false);  
        }
    }
}
