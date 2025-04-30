using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Camera Rotation
    [Header("Movement")]
    [SerializeField] private float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    // Ground Movement
    private Rigidbody rb;
    [SerializeField] private float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    [SerializeField] private float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    private bool wasGroundedLastFrame; 
    [SerializeField] private LayerMask groundLayer;
    
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    [Header("Audio")]
    [SerializeField] private float footstepInterval = 0.4f; // Adjust based on walk/run if we implment that 
    private float footstepTimer = 0f; 

    private PlayerSoundController soundController;

    private float health = 100;
    private float damageCooldownCount;
    private bool damageCooldown;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraTransform = Camera.main.transform;

        // Player Sound Managaer
        soundController = GetComponent<PlayerSoundController>();


        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (health <= 0) KillPlayer();  // Loads Main Menu once health reaches 0

        if (damageCooldownCount > 0 && damageCooldown == true)
            damageCooldownCount -= Time.deltaTime;
        else if (damageCooldownCount <= 0)
            damageCooldown = false;

            moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        RotateCamera();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        wasGroundedLastFrame = isGrounded;

        // Checking when we're on the ground and keeping track of our ground check delay
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        if (!wasGroundedLastFrame && isGrounded)
        {
            soundController.PlayLand();
        }

    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;

        // --- Play footsteps if moving ---
        if (isGrounded && movement.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                soundController.PlayFootstep();
                footstepTimer = footstepInterval; // e.g., 0.4f for normal walk speed
            }
        }

        // If we aren't moving and are on the ground, stop velocity so we don't slide
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void RotateCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Initial burst for the jump
        soundController.PlayJump();
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.velocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidingObject = other.gameObject;

        if (collidingObject.CompareTag("Enemy") && damageCooldown == false)  // Taking Damage
        {
            health -= 25;
            damageCooldown = true;
            damageCooldownCount = 1f;
        }
    }
}
