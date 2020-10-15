using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public float enemyHealth;

    public GameObject player;   // Reference to player
    private Player attackPlayer;
    
    public GameObject defensePoint; // Reference to defense point
    private DefensePoint attackDP;

    public NavMeshAgent agent;  // Navemesh agent of the enemy

    public LayerMask groundmask;    // Where can enemy walk on
    public LayerMask playerMask;    // Finds objects with layer of player
    public LayerMask defenseMask;   // Finds objects with layer of defend

    public float sightRange;    // How far can enemy see?
    public bool playerInSightRange; // Is player within enemy sight?

    public float playerDamage;  // How much damage does it do to the player?
    public float defensePDamage;    // How much damage does it do to the defense point?

    // Start is called before the first frame update
    void Start()
    {

    }
    

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (!playerInSightRange)
        {
            AttackDefensePoint();
        }
        if (playerInSightRange)
        {
            ChasePlayer();
        }

    }

    // Enemy takes damage
    public void EnemyTakeDamage(float amount)
    {
        enemyHealth -= amount;
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Chase the player
    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
    }

    // Chase the defense point
    private void AttackDefensePoint()
    {
        GetComponent<NavMeshAgent>().SetDestination(defensePoint.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "player")
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
