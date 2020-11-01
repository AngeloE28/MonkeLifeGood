using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Linq;

public class EnemyAi : MonoBehaviour
{
    public float currentEnemyHealth;
    public float maxEnemyHealth = 100;
    public HealthBar healthBar;

    public GameObject player;   // Reference to player
    private Player attackPlayer;

    public GameObject defensePoint; // Reference to defense point
    private DefensePoint attackDP;  

    public NavMeshAgent agent;  // Navemesh agent of the enemy

    // Sounds
    public AudioSource enemyAudioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public LayerMask groundmask;    // Where can enemy walk on
    public LayerMask playerMask;    // Finds objects with layer of player

    public float sightRange;    // How far can enemy see?
    public bool playerInSightRange; // Is player within enemy sight?

    public float playerDamage;  // How much damage does it do to the player?
    public float defensePDamage;    // How much damage does it do to the defense point?

    // Start is called before the first frame update
    void Start()
    {        
        currentEnemyHealth = maxEnemyHealth;
        healthBar.SetMaxhealth(maxEnemyHealth);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player"); // Finds gameobject with tag of player
        defensePoint = GameObject.FindGameObjectWithTag("Defend");  // Finds gameobject with tag of Defend
        enemyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (agent.enabled)
        {
            if (!playerInSightRange)
            {
                AttackDefensePoint();
            }
            if (playerInSightRange)
            {
                ChasePlayer();
            }
        }
        else { return; }
    }

    // Enemy takes damage
    public void EnemyTakeDamage(float amount)
    {
        enemyAudioSource.PlayOneShot(hitSound);
        currentEnemyHealth -= amount;

        healthBar.SetHealth(currentEnemyHealth);
        if (currentEnemyHealth <= 0f)
        {
            agent.enabled = false;
            enemyAudioSource.PlayOneShot(deathSound);
            Destroy(this.gameObject,1f);
        }
    }

    private void OnDestroy()
    {
        print("Charge + 1");
        player.GetComponent<Slam>().slamCharge++;
    }

    // Chase the player
    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    // Chase the defense point
    private void AttackDefensePoint()
    {
        agent.SetDestination(defensePoint.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "player")
        {
            attackPlayer = collision.transform.GetComponent<Player>();
            attackPlayer.PlayerTakeDamage(playerDamage);
        }

        if (collision.gameObject.tag == "Defend")
        {
            attackDP = collision.transform.GetComponent<DefensePoint>();
            attackDP.DefenseTakeDamage(defensePDamage);
        }
    }
}
