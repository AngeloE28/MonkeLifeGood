using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{
    // Health of the defense point
    public float health = 200f;

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
        health -= amount;
        if (health <= 0f)
        {
            player.EndGame(false);  
        }
    }
}
