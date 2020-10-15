using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRb;   // Bullet of the gun
    public GameObject impact;

    private EnemyAi enemy;
    
    public float damage;    // How much damage does one bullet do?

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // Hit effect
            GameObject impactGo = Instantiate(impact, transform.position,
                transform.rotation.normalized);
            Destroy(impactGo, 2f);

            enemy = collision.transform.GetComponent<EnemyAi>();

            enemy.EnemyTakeDamage(damage);

            Destroy(this.gameObject);

        }
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Defend")
        {

            GameObject impactGo = Instantiate(impact, transform.position,
               transform.rotation.normalized);
            impact.GetComponent<ParticleSystem>().Play();
            Destroy(impactGo, 2f);

            Destroy(this.gameObject);
        }
    }
}
