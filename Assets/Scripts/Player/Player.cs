using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController playerController;  // Character Controller component of player
    public GameManager myGameManager; // Ref to game manager
    public DefensePoint myDp;
    public FPSCamera myCam;
    public HealthBar playerHealthBar;

    // Damage feedback overlay
    public Image damageOverlay;
    private Color alpha;

    // Sounds
    public AudioSource playerAudioSource;
    public AudioClip playerHurt;

    // Player statistics
    public float currentPlayerHealth;  // How much health does player have
    public float maxPlayerHealth = 100;
    public float playerSpeed;   // How fast can player move
    public float walkSpeed = 5f;   // Speed player can move while walking
    public float runSpeed = 2f;   // Speed player can move while sprinting
    public float crouchSpeed = 2.5f;    // Speed player can move while crouching
    public float gravity = -9.81f;   // How strong is gravity?
    public float jumpHeight = 3f;   // How high can player jump?
    private float playerStandingHeight = 2f;    // How tall is player while standing
    private float playerCrouchHeight = 1f;  // How tall is player while crouching

    public Transform groundCheck;   // Checks for the ground
    public float groundDistance = 0.4f; // Radius of sphere created
    public LayerMask groundMask;    // The condition to be checked for objects that has ground layer
    public LayerMask defenseMask;

    private Vector3 velocity;   // Player's physics
    public bool isGrounded;    // is player grounded?
    public bool isOnDefensePoint;
    public bool isCrouched;    // is player crouched?
    public bool isPlayerAlive; // Is player alive?
    public bool isPlayerSprinting = false;

    private void Start()
    {
        alpha = damageOverlay.color;
        currentPlayerHealth = maxPlayerHealth;
        playerHealthBar.SetMaxhealth(maxPlayerHealth);
        isPlayerAlive = true;
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Gets the GameManager script
        myCam = GameObject.Find("Main Camera").GetComponent<FPSCamera>(); // Gets the FPScamera script
        myDp = GameObject.FindGameObjectWithTag("Defend").GetComponent<DefensePoint>(); // Gets the DefensePoint script
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myGameManager.isGameRunning)
        {
            if (isPlayerAlive)
            {   // Player can move since game is running and player is alive
                Move();
            }
            else
            {   // Game is not running and player has lost the game
                EndGame(false);
            }
        }
    }

    // Game over window pops up
    public void EndGame(bool isWin)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        myCam.enabled = false;
        myGameManager.isGameRunning = false;
        myGameManager.GameOver(isWin);
    }

    // Player takes damage
    public void PlayerTakeDamage(float amount)
    {
        playerAudioSource.PlayOneShot(playerHurt);
        currentPlayerHealth -= amount;
        if(alpha.a <=100f)
        {
            alpha.a += .1f;
            damageOverlay.color = alpha;
        }

        Invoke("ResetDamageOverlay", 1f);

        playerHealthBar.SetHealth(currentPlayerHealth);
        if (currentPlayerHealth <= 0f)
        {
            isPlayerAlive = false;
        }
    }

    // Creates a feedblack when player is hit
    private void ResetDamageOverlay()
    {
        if (alpha.a >= 0)
        {
            alpha.a -= .1f;
            damageOverlay.color = alpha;
        }

    }

    // Player moves
    private void Move()
    {
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        // Direction player wants to move
        Vector3 movement = transform.right * xDir + transform.forward * zDir;

        playerController.Move(movement * playerSpeed * Time.deltaTime);

        Jump();
        Crouch();
        Sprint();
    }


    // Player jumps
    private void Jump()
    {
        // Checks if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //isOnDefensePoint = Physics.CheckSphere(groundCheck.position, groundDistance, defenseMask);


        // Resets velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //if (isOnDefensePoint && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}


        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouched)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //if (Input.GetButtonDown("Jump") && isOnDefensePoint && !isCrouched)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //}

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }

    // Player crouches
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerController.height = playerCrouchHeight;
            playerSpeed = crouchSpeed;
            isCrouched = true;
        }
        else
        {
            playerController.height = Mathf.Lerp(playerController.height, playerStandingHeight, 10 * Time.deltaTime);
            playerSpeed = walkSpeed;
            isCrouched = false;
        }

    }
   
    // Player Sprints
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
        {
            playerSpeed = runSpeed;
            isPlayerSprinting = true;
        }
        else
        {
            isPlayerSprinting = false;
        }
        if(isPlayerSprinting)
        {
            // Play sound
        }
    }
}