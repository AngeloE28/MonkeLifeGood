using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public GameObject water_impact;

    private Player player;
    private DefensePoint defensePoint;

    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            HitEffect(water_impact);

            player = collision.transform.GetComponent<Player>();
            player.PlayerTakeDamage(damage);

            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Defend")
        {
            HitEffect(water_impact);

            defensePoint = collision.transform.GetComponent<DefensePoint>();
            defensePoint.DefenseTakeDamage(damage);

            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Ground")
        {
            HitEffect(water_impact);

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
