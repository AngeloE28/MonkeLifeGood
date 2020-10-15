using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float gunDamage = 10f;   // Damage per bullet
    public float gunRange = 100f;   // How far can player shoot

    public ParticleSystem muzzleFlash;  // Muzzle flash when player shoots
    public Camera gunCam;   // Reference to main camera
    public GameObject impact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetButtonDown("Fire1"))
        {

            ShootGun();
        }
    }

    void ShootGun()
    {
        muzzleFlash.Play();

        RaycastHit hitInfo;
        if(Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hitInfo, gunRange))
        {
            print(hitInfo.transform.name);

            DefensePoint target = hitInfo.transform.GetComponent<DefensePoint>();

            if (target != null)
            {
                target.DefenseTakeDamage(gunDamage);
            }

            GameObject impactGo = Instantiate(impact, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            Destroy(impactGo, 2f);
        }

       
    }
}
