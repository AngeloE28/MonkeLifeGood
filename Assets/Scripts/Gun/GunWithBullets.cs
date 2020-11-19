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
    public Camera playerCam; // Ref to main camera
    public Transform attackPoint;   // where bullets will come out of

    // Sounds
    public AudioSource gunAudioSource;
    public AudioClip reloadSound;
    public AudioClip shootSound;

    // Statistics of the gun
    public float timeBetweenSprays;
    public float bulletSpread;
    public float timeTillMaxSpread;
    public float timeTillMS_Standing;
    public float timeTillMS_Crouching;
    public float reloadTime;
    public float fireRate;
    public float recoilCooldown;
    public float coolDownSpeed;
    public int bulletsLeft;
    public int magSize;
    public int bulletsPerClick;
    public bool allowGunToSpray;
    public float totalReloadTime = 4f;
    public float reloadBarTimer;

    private int countBullet;
    private int magCapacity;
    private int bulletsShot;
    private float accuracy;
    private bool shooting; // how can player shoot?
    private bool readyToShoot;  // Can player shoot?
    private bool reloading; // is player reloading?

    public bool allowInvoke = true;

    // position of the gun
    public Vector3 aimDownSight;
    public Vector3 hipFire;

    public float adsSpeed;  // How fast does the gun move when player aim down sight?
    private int normalFieldOfView = 60;
    private int adsFieldOfView = 50;

    // Start is called before the first frame update
    void Start()
    {
        reloadBarTimer = totalReloadTime;
        hipFire = this.transform.localPosition;
        magCapacity = magSize;
        readyToShoot = true;
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script
        gunAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myGameManager.isGameRunning)
        {
            PlayerInput();

            if(reloading)
            {
                if (reloadBarTimer > 0)
                {
                    reloadBarTimer -= Time.deltaTime;
                }
            }
            // Display ammo
            if (ammoDisplay != null)
            {
                ammoDisplay.SetText("Ammo: " + magCapacity / bulletsPerClick + " / " + bulletsLeft / bulletsPerClick);
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
        if (Input.GetKeyDown(KeyCode.R) && magCapacity < bulletsLeft && !reloading)
        {
            Reload();
        }
        // Player is forced to reload
        if (readyToShoot && shooting && !reloading && magCapacity <= 0)
        {
            Reload();
        }

        // Increases the inaccuracy of the gun
        coolDownSpeed += Time.deltaTime * 60f;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Increases the inaccuracy of the gun
            accuracy += Time.deltaTime * 4f;
            if (coolDownSpeed >= fireRate)
            {
                coolDownSpeed = 0;
                recoilCooldown = 1;
            }
        }
        else
        {
            // Resets the accuracy so its 100% accurate
            recoilCooldown -= Time.deltaTime;
            if (recoilCooldown <= 1)
            {
                accuracy = 0f;
            }
        }

        // Player fires gun
        if (readyToShoot && shooting && !reloading && magCapacity > 0)
        {
            bulletsShot = 0;
            muzzleFlash.Play();
            gunAudioSource.PlayOneShot(shootSound);
            Shoot();
        }

        AimDownSight();      
    }

    // Player aims down sight
    private void AimDownSight()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (playerCam.fieldOfView > adsFieldOfView)
            {
                playerCam.fieldOfView += (-45 * Time.deltaTime);
            }
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSight, adsSpeed * Time.deltaTime);
        }
        else
        {
            if (playerCam.fieldOfView < normalFieldOfView)
            {
                playerCam.fieldOfView += (45 * Time.deltaTime);
            }
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

        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);

        // Calculate how long before the gun becomes inaccurate when player is standing
        if (!player.GetComponent<Player>().isCrouched)
        {
            timeTillMaxSpread = timeTillMS_Standing;
        }
        else // Calculate spread when player is crouching
        {
            timeTillMaxSpread = timeTillMS_Crouching;
        }

        float currentSpread = Mathf.Lerp(0.0f, bulletSpread, accuracy / timeTillMaxSpread);

        fireRotation = Quaternion.RotateTowards(fireRotation, Random.rotation, Random.Range(0.0f, currentSpread));

        Vector3 targetPoint;
        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hitInfo, Mathf.Infinity))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        // Spawn the bullets
        GameObject currentBullet = Instantiate(bullet, attackPoint.transform.position, fireRotation);      
        currentBullet.GetComponent<Bullet>().hitPoint = targetPoint;
        // Destroy bullet after 2 sec
        Destroy(currentBullet.gameObject, 2f);

        magCapacity--;
        bulletsShot++;
        countBullet++;
        
        if (allowInvoke)
        {
            Invoke("ResetShot", fireRate);
            allowInvoke = false;
        }

        // Shooting for guns like shotguns or burst fire guns
        if (bulletsShot < bulletsPerClick && magCapacity > 0)
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
        //bulletsLeft -= countBullet; might add ammo pick up later
        showReloading.gameObject.SetActive(true);
        // create a bar
        // play reload anim
        gunAudioSource.PlayOneShot(reloadSound);
        showReloading.SetText("Reloading...");
        Invoke("ReloadFinish", reloadTime); // Calls a function with delay             
    }

    // Refills the magazine                                                            
    private void ReloadFinish()
    {
        reloadBarTimer = totalReloadTime;
        showReloading.gameObject.SetActive(false);
        magCapacity = magSize;
        reloading = false;
    }
}
