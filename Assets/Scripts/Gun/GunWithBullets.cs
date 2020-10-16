using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GunWithBullets : MonoBehaviour
{
    public GameManager myGameManager; // Ref to the game manager
    public GameObject bullet;   // The bullet the gun shoots out
    public GameObject player;   // Ref to player
    public ParticleSystem muzzleFlash; // The muzzle flash when shooting
    public TextMeshProUGUI ammoDisplay; // Displays ammo
    public TextMeshProUGUI showReloading;   // Shows gun is reloading

    // How fast is the bullet
    public float shootingForce;

    // Statistics of the gun
    public float timeBetweenSprays;
    public float bulletSpread;
    public float bulletSpreadWhenCrouching;
    public float reloadTime;
    public float fireRate;
    public int magSize;
    public int bulletsPerClick;
    public bool allowGunToSpray;
    private int bulletsLeft;
    private int bulletsShot;

    // The variables to control the spray of bullets when player is standing or crouching
    private float xDir;
    private float yDir;

    private bool shooting; // how can player shoot?
    private bool readyToShoot;  // Can player shoot?
    private bool reloading; // is player reloading?

    public Camera playerCam; // Ref to main camera
    public Transform attackPoint;   // where bullets will come out of

    public bool allowInvoke = true;

    // position of the gun
    public Vector3 aimDownSight;
    public Vector3 hipFire;

    public float adsSpeed;  // How fast does the gun move when player aim down sight?

    // Start is called before the first frame update
    void Start()
    {
        hipFire = this.transform.localPosition;

        bulletsLeft = magSize;
        readyToShoot = true;
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script
    }

    // Update is called once per frame
    void Update()
    {
        if (myGameManager.isGameRunning)
        {
            PlayerInput();

            // Display ammo
            if (ammoDisplay != null)
            {
                ammoDisplay.SetText("Ammo: " + bulletsLeft / bulletsPerClick + " / " + magSize / bulletsPerClick);
            }
        }
        else { return; }
    }

    // Takes player input
    private void PlayerInput()
    {
        // Player can hold the left click to spray bullets
        if (allowGunToSpray)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else // Player needs to tap to shoot
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Player relaods
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }
        // Player is forced to reload
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        // Player fires gun
        if (readyToShoot && shooting & !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            muzzleFlash.Play();
            Shoot();
        }

        // Player aims down sight
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSight, adsSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, adsSpeed * Time.deltaTime);
        }
    }

    // Controls the shooting of the gun
    private void Shoot()
    {
        readyToShoot = false;

        // Ray going through the middle of the screen
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        Vector3 targetPoint;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitInfo))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100); // A point that is far away from the player
        }

        // Direction from attackpoint to targetpoint with no spread
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Calculate spread when player is standing
        if (!player.GetComponent<Player>().isCrouched)
        {
            xDir = Random.Range(-bulletSpread, bulletSpread);
            yDir = Random.Range(-bulletSpread, bulletSpread);
        }
        else // Calculate spread when player is crouching
        {
            xDir = Random.Range(-bulletSpreadWhenCrouching, bulletSpreadWhenCrouching);
            yDir = Random.Range(-bulletSpreadWhenCrouching, bulletSpreadWhenCrouching);
        }

        // Direction from attackpoint to targetpoint with the spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xDir, yDir, 0);

        // Spawn the bullets
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Destroy bullet after 2 sec
        Destroy(currentBullet.gameObject, 2f);

        // Shoot the bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootingForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", fireRate);
            allowInvoke = false;
        }

        // Shooting for guns like shotguns or burst fire guns
        if (bulletsShot < bulletsPerClick && bulletsLeft > 0)
        {
            Invoke("Shoot", fireRate);
        }
    }

    // Resets variables back to true
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    // Reloads the gun
    private void Reload()
    {
        reloading = true;
        showReloading.gameObject.SetActive(true);
        showReloading.SetText("Reloading...");
        Invoke("ReloadFinish", reloadTime); // Calls a function with delay             
    }

    // Refills the magazine                                                            
    private void ReloadFinish()
    {
        showReloading.gameObject.SetActive(false);
        bulletsLeft = magSize;
        reloading = false;
    }
}
