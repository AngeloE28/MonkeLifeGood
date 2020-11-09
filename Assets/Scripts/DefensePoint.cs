using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefensePoint : MonoBehaviour
{
    // Health of the defense point
    public float currentDefenseHealth;
    public float maxDefenseHealth = 200f;
    public int fireCount;

    public GameObject fire;
    public Transform[] fireSpawnPoints;
    public GameManager myGameManager; // Ref to game Manager
    public Player player;
    public HealthBar healthBar;

    void Start()
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
        if ((currentDefenseHealth == (7 * maxDefenseHealth) / 8))
        {
            SpawnFire(fire, fireSpawnPoints[0]);
        }
        if (currentDefenseHealth == (3 * maxDefenseHealth / 4))
        {
            SpawnFire(fire, fireSpawnPoints[1]);
        }
        if ((currentDefenseHealth == (5 * maxDefenseHealth) / 8))
        {
            SpawnFire(fire, fireSpawnPoints[2]);
        }
        if (currentDefenseHealth == (maxDefenseHealth / 2))
        {
            SpawnFire(fire, fireSpawnPoints[3]);
        }
        if ((currentDefenseHealth == (3 * maxDefenseHealth) / 8))
        {
            SpawnFire(fire, fireSpawnPoints[4]);
        }
        if (currentDefenseHealth == (maxDefenseHealth / 4))
        {
            SpawnFire(fire, fireSpawnPoints[5]);
        }
        if (currentDefenseHealth == (maxDefenseHealth / 8))
        {
            SpawnFire(fire, fireSpawnPoints[6]);
        }
        if (currentDefenseHealth <= 0f)
        {
            player.EndGame(false);  
        }
    }

    // Spawns fire
    private void SpawnFire(GameObject spawnEnemy, Transform sp_point)
    {
        for (int k = 0; k < fireCount; k++)
        {
            // Spawns enemies in random spawn points
            sp_point = fireSpawnPoints[Random.Range(0, fireSpawnPoints.Length)];
            Instantiate(spawnEnemy, sp_point.position, sp_point.rotation);
        }
    }
}
