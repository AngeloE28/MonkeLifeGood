using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    public float bossHealth; // How much health does the boss have?

    // Which state is the spawner currently in?
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTINGDOWN
    };

    // Custom class of the wave that spawns the enemies
    [System.Serializable]
    public class Wave
    {
        public string waveName; // Which wave is it?
        public GameObject[] enemyPrefab; // The enemy being spawned
        public int enemyCount; // How many enemies?
        public float spawnRate; // How many enemies are being spawned?
    }

    public Wave[] waves; // How many waves?
    public Transform[] spawnPoints; // The spawnpoints of the enemy
    private int nextWave;
    public float timeBetweenWaves = 5f; // Time between each waves
    private float waveCountdown; // Countdown till the next wave
    private float checkIfEnemyAliveCountdown = 1f; // Time limit to search if enemies are still alive
    private SpawnState state = SpawnState.COUNTINGDOWN;

    public GameManager myGameManager;
    public GameObject player;
    public NavMeshAgent agent;  // Navemesh agent of the enemy


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player"); // Finds gameobject with tag of player
        nextWave = 0; // Start at the first wave
        waveCountdown = timeBetweenWaves;
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script        
    }

    // Update is called once per frame
    void Update()
    {        
        // Is game running?
        if (myGameManager.isGameRunning)
        {
            if (bossHealth <= 0f)
            {
                agent.isStopped = true;
                player.GetComponent<Player>().EndGame(true);
                myGameManager.isGameRunning = false;
            }
            // change the else statement to a follow path instead
            else { agent.SetDestination(player.transform.position); }

            // Check if there are enemies still alive
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    WaveCleared();
                    return;
                }
                else { return; }
            }
            // Interval between waves
            if (waveCountdown <= 0)
            {
                transform.GetComponent<BoxCollider>().enabled = false; // Enables box collider so player can damage boss
                if (state != SpawnState.SPAWNING)
                {
                    //start spawning wave
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                transform.GetComponent<BoxCollider>().enabled = true; // Set to false, so player won't be able to damage it at the start
                waveCountdown -= Time.deltaTime;
            }
        }
        else { return; }
    }

    // Boss takes damage
    public void BossTakeDamage(float amount)
    {
        bossHealth -= amount;
    }

    // If wave has been cleared, start the next one
    private void WaveCleared()
    {
        print("wave finito");

        state = SpawnState.COUNTINGDOWN;
        waveCountdown = timeBetweenWaves;

        // Check to see if final wave has been reached
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0; // Resets the index of the array so it endlessly spawns until boss is dead
        }
        else // Else it just moves on to the next wave
        {
            nextWave++;
        }
    }


    // Checks if enemies are alive
    private bool EnemyIsAlive()
    {
        checkIfEnemyAliveCountdown -= Time.deltaTime;

        if (checkIfEnemyAliveCountdown <= 0f)
        {
            checkIfEnemyAliveCountdown = 1f; // Resets countdown
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    // Spawns enemies, but needs to wait a certain amount of time before it spawns anything
    IEnumerator SpawnWave(Wave enemyWave)
    {
        print("Spawning wave" + enemyWave.waveName);
        state = SpawnState.SPAWNING; // Spawns the wave

        for (int i = 0; i < enemyWave.enemyCount; i++)
        {
            SpawnEnemey(enemyWave.enemyPrefab[Random.Range(0,enemyWave.enemyPrefab.Length)]);
            yield return new WaitForSeconds(1f / enemyWave.spawnRate); // The spawn rate for spawning the enemy
        }

        state = SpawnState.WAITING; // Waiting for player to kill all enemies

        yield break;
    }

    // Spawns Enemies
    private void SpawnEnemey(GameObject spawnEnemy)
    {
        // Spawns enemies in random spawn points
        Transform sP = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(spawnEnemy, sP.position, sP.rotation);
    }
}
