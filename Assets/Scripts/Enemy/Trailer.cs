using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trailer : MonoBehaviour
{
    public Boss boss;
    public bool takeDamage;

    public NavMeshAgent agent;
    public GameObject followTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(followTarget.transform.position);

        if(boss.bossTakeDamage)
        {
            takeDamage = true;
        }
        else
        {
            takeDamage = false;
        }
    }
    public void TrailerTakeDamage(float amount)
    {
        if (takeDamage)
        {
            boss.bossHealth -= amount;
        }
    }
}
