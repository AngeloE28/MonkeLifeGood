using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slam : MonoBehaviour
{
    public float upwardForce;
    public float radius;
    public float force;
    public bool allowInvoke = true;
    public int slamCharge;

    // Sounds
    public AudioSource myAudioSource;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        SlamAttack();
    }

    // Code block for the slam attack
    private void SlamAttack()
    {
        if(slamCharge > 4)
        {
            slamCharge = 4;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (slamCharge == 4)
            {
                print("slamma");
                // Play anim
                myAudioSource.PlayOneShot(explosionSound);
                slamCharge = 0;
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                foreach (Collider nearbyObject in colliders)
                {
                    if (GameObject.FindGameObjectWithTag("Enemy"))
                    {
                        NavMeshAgent agent = nearbyObject.GetComponent<NavMeshAgent>();
                        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                        // Disables navmesh agent to be able to apply forces to the enemy
                        if (agent != null)
                        {
                            agent.enabled = false;
                        }
                        // Sends the enemy upwards and also applies an explosion force
                        if (rb != null)
                        {
                            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
                            rb.AddExplosionForce(force, transform.position, radius);
                        }

                    }
                }
            }
            else
            {
                print("Not Ready");
            }

        }


        // Creates a 4 second delay before agent is re-enabled
        if (allowInvoke)
        {
            Invoke("ResetAgent", 4);
            allowInvoke = false;
        }
    }

    // Re-enables the navmesh agent and resets the allowInvoke bool
    private void ResetAgent()
    {
        allowInvoke = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject allEnemies in enemies)
        {
            NavMeshAgent agent = allEnemies.GetComponent<NavMeshAgent>();
            if(agent!= null && agent.enabled == false)
            {
                agent.enabled = true;
            }
        }
    }
}