using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trailer : MonoBehaviour
{
    public Boss boss;
    public bool takeDamage;

    // Trailer path
    public NavMeshAgent agent;
    public GameObject followTarget;

    // Colour changer
    public Material metal;
    public Material original;

    // Sound
    public AudioSource trailerSoundSource;
    public AudioClip hitSound;
    public AudioClip metalHitSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<Renderer>().material = original;
        trailerSoundSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        agent.SetDestination(followTarget.transform.position);

        if (boss.bossCanTakeDamage)
        {
            GetComponent<Renderer>().material = original;
            takeDamage = true;
        }
        else
        {
            GetComponent<Renderer>().material = metal;
            takeDamage = false;
        }
    }

    //// The agent searching for the waypoints
    //private void GoToNextPoint()
    //{
    //    // Makes sure there is a path
    //    if (wayPoint.Length == 0)
    //    {
    //        return;
    //    }

    //    agent.destination = wayPoint[current].position;
    //    current = (current + 1) % wayPoint.Length;
    //}

    public void TrailerTakeDamage(float amount)
    {
        if (takeDamage)
        {
            trailerSoundSource.PlayOneShot(hitSound);
            boss.bossCurrentHealth -= amount;
        }
        else
        {
            trailerSoundSource.PlayOneShot(metalHitSound);
        }        
    }
}
