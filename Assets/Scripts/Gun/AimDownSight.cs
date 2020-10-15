using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    // position of the gun
    public Vector3 aimDownSight;    
    public Vector3 hipFire;

    public float aimSpeed;

    // Start is called before the first frame update
    void Start()
    {
        hipFire = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Aims down sight
        if(Input.GetButton("Fire2"))
        {
            // this is for the reloading but i scrapped it refer back to here if i decide i want it back
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSight, aimSpeed * Time.deltaTime);
            if(transform.rotation.x != 12.6f && transform.rotation.y != -21.4f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(12.5f, -21.5f, 0), aimSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, aimSpeed * Time.deltaTime);
            if (transform.rotation.x != 0 && transform.rotation.y != 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), aimSpeed * Time.deltaTime);
            }
        }
    }
}
