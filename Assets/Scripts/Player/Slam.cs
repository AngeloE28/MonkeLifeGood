using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slam : MonoBehaviour
{
    public float upwardFore;
    public float radius;
    public float force;
    public bool allowInvoke = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("slamma");
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
                        rb.AddForce(Vector3.up * upwardFore, ForceMode.Impulse);
                        rb.AddExplosionForce(force, transform.position, radius);
                    }

                }
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
