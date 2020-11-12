using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRb;   // Bullet of the gun
    public GameObject impact;

    private EnemyAi enemy;
    private Boss boss;
    private Trailer trailer;

    public float damage;    // How much damage does one bullet do?
    public Vector3 hitPoint;
    public float shootForce;
    private void Start()
    {
        // Moves the bullet
        this.GetComponent<Rigidbody>().AddForce((hitPoint - this.transform.position).normalized * shootForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            HitEffect(impact);

            enemy = collision.transform.GetComponent<EnemyAi>();
            enemy.EnemyTakeDamage(damage);

            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Boss")
        {
            HitEffect(impact);

            boss = collision.transform.GetComponent<Boss>();
            boss.BossTakeDamage(damage);

            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Trailer")
        {
            HitEffect(impact);
            trailer = collision.transform.GetComponent<Trailer>();
            trailer.TrailerTakeDamage(damage);

        }
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Defend")
        {
            HitEffect(impact);

            Destroy(this.gameObject);
        }
    }

    // Hit effect
    private void HitEffect(GameObject effect)
    {
        GameObject impactGo = Instantiate(effect, transform.position, transform.rotation.normalized);
        Destroy(impactGo, 2f);
    }
}
