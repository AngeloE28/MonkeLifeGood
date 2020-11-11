using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class path_test : MonoBehaviour
{
    public Transform[] wayPoint;
    private int current = 0;
    public float minRemainingDistance = 0.5f;

    public NavMeshAgent agent;  // Navemesh agent of the enemy
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoToNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < minRemainingDistance)
        {
            GoToNextPoint();
        }
    }

    // The agent searching for the waypoints
    private void GoToNextPoint()
    {
        // Makes sure there is a path
        if (wayPoint.Length == 0)
        {
            return;
        }

        agent.destination = wayPoint[current].position;
        current = (current + 1) % wayPoint.Length;
    }
}
