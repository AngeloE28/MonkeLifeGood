using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterController playerController;  // Character Controller component of player

    public float playerHealth;  // How much health does player have
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

    private Vector3 velocity;   // Player's physics
    public bool isGrounded;    // is player grounded?
    public bool isCrouched;    // is player crouched?

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    // Player takes damage
    public void PlayerTakeDamage(float amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Player moves
    void Move()
    {
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        // Direction player wants to move
        Vector3 movement = transform.right * xDir + transform.forward * zDir;

        playerController.Move(movement * playerSpeed * Time.deltaTime);

        Jump();
        Crouch();
    }


    // Player jumps
    void Jump()
    {
        // Checks if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Resets velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouched)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }

    // Player crouches
    void Crouch()
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
        // Player Sprints
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
        {
            playerSpeed = runSpeed;
        }
    }
}