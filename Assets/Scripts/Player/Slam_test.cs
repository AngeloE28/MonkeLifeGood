﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EZCameraShake;

public class Slam_test : MonoBehaviour
{
    public CameraShaker camShaker;

    // Slam stats
    public float upwardForce;
    public float radius;
    public float force;
    public bool allowInvoke = true;
    public int slamCharge;
    public int maxCharge;
    public int damage;

    // Camera shake
    public float cs_magn;
    public float cs_rough;
    public float cs_fadeIn;
    public float cs_fadeOut;

    // Sounds
    public AudioSource myAudioSource;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        camShaker = GameObject.Find("Main Camera").GetComponent<CameraShaker>();
    }
    // Update is called once per frame
    void Update()
    {
        SlamAttack();
    }

    // Code block for the slam attack
    private void SlamAttack()
    {
        if(slamCharge > maxCharge)
        {
            slamCharge = maxCharge;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (slamCharge == maxCharge)
            {
                camShaker.enabled = true;
                print("slamma");
                // Play anim
                CameraShaker.Instance.ShakeOnce(cs_magn, cs_rough, cs_fadeIn, cs_fadeOut);
                myAudioSource.PlayOneShot(explosionSound);
                slamCharge = 0;
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                foreach (Collider nearbyObject in colliders)
                {
                    if (nearbyObject.gameObject.tag == "Enemy")
                    {
                        NavMeshAgent agent = nearbyObject.GetComponent<NavMeshAgent>();
                        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                        EnemyAi enemy = nearbyObject.GetComponent<EnemyAi>();
                        // Disables navmesh agent to be able to apply forces to the enemy
                        if (agent != null)
                        {
                            agent.enabled = false;
                        }
                        // Sends the enemy upwards and also applies an explosion force
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                            rb.useGravity = true;
                            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
                            rb.AddExplosionForce(force, transform.position, radius);
                        }
                        // Enemies take damage
                        if(enemy != null)
                        {
                            enemy.EnemyTakeDamage(damage);
                        }

                    }
                }
                //camShaker.enabled = false;
                Invoke("ResetShaker", 1f);
            }
            else
            {
                print("Not Ready");
            }
            // Creates a delay before agent is re-enabled
            if (allowInvoke)
            {
                Invoke("ResetAgent", 1.5f);
                allowInvoke = false;
            }
        }
    }

    // Disables the EzCamera Shaker script
    private void ResetShaker()
    {
        camShaker.enabled = false;
    }

    // Re-enables the navmesh agent and resets the allowInvoke bool
    // Resets the rigidbody to be kinematic and disable gravity
    private void ResetAgent()
    {
        allowInvoke = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject allEnemies in enemies)
        {
            if (allEnemies.gameObject.tag == "Enemy")
            {
                NavMeshAgent agent = allEnemies.GetComponent<NavMeshAgent>();
                Rigidbody rb = allEnemies.GetComponent<Rigidbody>();
                if (agent != null && agent.enabled == false)
                {
                    agent.enabled = true;
                }
                if(rb!= null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
        }
    }
}
