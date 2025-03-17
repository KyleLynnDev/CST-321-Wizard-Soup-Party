using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    
    //input
  
    public CharacterController characterController; 
    public PlayerInputActions inputActions;
    
    //movement 
    
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    
    //jumping

    public float jumpHeight = 2.5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;

    private Vector3 velocity;
    private bool isGrounded; 
    
    //Gliding
    [Header("Gliding Settings")] 
    public bool isGliding = false;
    public float glideGravity = -2f;
    public float maxGlideTime = 2f;
    private float currentGlideTime; 
    
    

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
            inputActions.Player.Enable();
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player.Jump.performed += ctx => Jump();
            inputActions.Player.Jump.canceled += ctx => CutJump();
            
            // Glide input handling
            inputActions.Player.Glide.performed += ctx => StartGlide();
            inputActions.Player.Glide.canceled += ctx => StopGlide();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        MoveCharacter(); 
    }

    private void ApplyGravity()
    {
        // Use CharacterController's built-in ground detection
        bool controllerGrounded = characterController.isGrounded;

        // Use a Raycast to confirm grounding (extra margin for error)
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float raycastDistance = groundCheckDistance + 0.5f;
        bool raycastGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance);
        
        // Combine both checks
        bool wasGrounded = isGrounded;
        isGrounded = controllerGrounded || raycastGrounded;
        
        // Debug: Visualize the Raycast
        Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, isGrounded ? Color.green : Color.red);

        
        
        
        
        // if (isGrounded && velocity.y < 0)
        // {
        //     velocity.y = -2f; // Keep player on ground
        // }
        // else
        // {
        //     velocity.y += gravity * Time.deltaTime;
        // }

        // Only reset velocity when landing, to avoid interfering with jumps
        if (isGrounded)
        {
            if (!wasGrounded) // This ensures we don't reset mid-air
            {
                velocity.y = -2f; // Keep player on ground only when landing
            }
            isGliding = false; // Stop gliding when landing
        }
        else if (isGliding)
        {
            // Apply slow gravity while gliding
            velocity.y = Mathf.Lerp(velocity.y, glideGravity, Time.deltaTime * 5f);

            // Reduce glide time
            currentGlideTime -= Time.deltaTime;
            if (currentGlideTime <= 0) StopGlide(); // Stop gliding when out of time
        }
        else
        {
            // Apply normal gravity while falling
            velocity.y += gravity * Time.deltaTime;
        }
        
        
        
        characterController.Move(velocity * Time.deltaTime);
    }

    private void StartGlide()
    {
        if (!isGrounded) // Only allow gliding if player is airborne
        {
            isGliding = true;
            currentGlideTime = maxGlideTime; // Reset glide time
            velocity.y = 0f; // Reset downward momentum for smooth glide
        }
    }

    private void StopGlide()
    {
        isGliding = false;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            //physics based jump calculation:
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void CutJump()
    {
        if (velocity.y > 0) // Only stop jump if still moving upward
        {
            velocity.y *= 0.5f; // Reduces upward velocity for a shorter jump
        }
    }
    
    private void MoveCharacter()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        
        
        moveDirection = (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
