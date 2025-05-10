using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    
    //input
  
    public CharacterController characterController; 
    public PlayerInputActions inputActions;
    
    //movement 
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
        
    private Vector2 moveInput;
    private Vector3 moveDirection;
    
    //jumping
    [Header("Jump Settings")]
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
    
    //dashing
    [Header("Dashing Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime;
    
    //coyote time

    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter = 0f;
    

    [Header("Interaction Settings")]


    private GameObject nearbyPickup;
    private bool canInteract = false;

    public GameObject interactionPrompt;

    private NPCInteraction nearbyNPC;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueTextUI;

    private bool inDialogue = false;
    private int currentDialogueIndex = 0;
    private string[] currentDialogue;


    //for sprite animation
    private Animator animator; 

    [SerializeField] private Transform spriteTransform; 

    


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponentInChildren<Animator>(); // the animator is in visual, a child of player
    }

    private void OnEnable()
    {
            inputActions.Player.Enable();
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player.Jump.performed += ctx => Jump();
            inputActions.Player.Jump.canceled += ctx => CutJump();
            inputActions.Player.Interact.performed += ctx => TryInteract();
            inputActions.Player.Reset.performed += ctx => ResetScene();
            
            // Glide input handling
            inputActions.Player.Glide.performed += ctx => StartGlide();
            inputActions.Player.Glide.canceled += ctx => StopGlide();
            
            // Dash input handling
            inputActions.Player.Dash.performed += ctx => StartDash();
    }

    private void OnDisable()
    {
        inputActions.Player.Reset.performed -= ctx => ResetScene();
        inputActions.Player.Interact.performed -= ctx => TryInteract();
        inputActions.Player.Disable();
    }
    
    
    // Update is called once per frame
    void Update()
    {

    
        if (isDashing)
        {
            DashMovement();
        }
        else
        {
            ApplyGravity();
            MoveCharacter();
        }

        UpdateAnimationStates(); 
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
        

        // Only reset velocity when landing, to avoid interfering with jumps
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time
            if (!wasGrounded) 
            {
                velocity.y = -2f; // Keep player on ground only when landing
            }
            isGliding = false; // Stop gliding when landing
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Reduce coyote time if in the air
        }

        // If jumping, prevent gravity from instantly applying full force
        if (velocity.y > 0) 
        {
            velocity.y += (gravity * 0.5f) * Time.deltaTime; // Reduce gravity effect when going up
        }
        else if (isGliding)
        {
            velocity.y = Mathf.Lerp(velocity.y, glideGravity, Time.deltaTime * 5f);
            currentGlideTime -= Time.deltaTime;
            if (currentGlideTime <= 0) StopGlide();
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply normal gravity
        }
        
        characterController.Move(velocity * Time.deltaTime);
    }

    
    
    private void StartDash()
    {
        if (!isDashing && Time.time >= lastDashTime + dashCooldown) // Only dash if not on cooldown
        {
            animator.SetBool("isDashing", true);
            isDashing = true;
            dashTimeLeft = dashDuration;
            lastDashTime = Time.time;

            // Give initial burst of speed in the movement direction
            Vector3 dashDirection = moveDirection.magnitude > 0.1f ? moveDirection : transform.forward;
            velocity = new Vector3(dashDirection.x, 0, dashDirection.z) * dashSpeed;
            Debug.Log("Triggered");
        }
    }


    private void DashMovement()
    {
        Debug.Log("Dashing...");
        if (dashTimeLeft > 0)
        {
            characterController.Move(velocity * Time.deltaTime);
            dashTimeLeft -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            animator.SetBool("isDashing", false);
            isDashing = false;
            velocity = Vector3.zero;
            //velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 20f);
        }
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
        if (isGrounded || coyoteTimeCounter > 0f)
        {
            //physics based jump calculation:
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteTimeCounter = 0f; // Reset coyote time after jumping
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
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            nearbyPickup = other.gameObject;
            canInteract = true;
            if (interactionPrompt != null){
                interactionPrompt.SetActive(true); 
            }
        }

         if (other.CompareTag("NPC"))
        {
            nearbyNPC = other.GetComponent<NPCInteraction>();
        }


    }


    private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Mushroom"))
    {
        if (other.gameObject == nearbyPickup)
        {
            nearbyPickup = null;
            canInteract = false;
            if (interactionPrompt != null){
                interactionPrompt.SetActive(false); 
            }
        }
    }

    if (other.CompareTag("NPC"))
    {
        nearbyNPC = null;
    }


}

private void TryInteract()
{
    if (inDialogue)
    {
        AdvanceDialogue(); // Progress dialogue if already talking
        return;
    }

    if (canInteract && nearbyPickup != null)
    {
        MushroomPickup pickup = nearbyPickup.GetComponent<MushroomPickup>();
        if (pickup != null)
        {
            pickup.Collect(this);
        }

        nearbyPickup = null;
        canInteract = false;
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        return;
    }

    if (nearbyNPC != null && nearbyNPC.IsPlayerInRange())
    {
        StartDialogue(nearbyNPC.dialogueLines.ToArray());
    }


}




    private void CollectMushroom(GameObject mushroom)
    {
    // Optional: play sound, spawn effect, etc.
    jumpHeight += 0.5f; // Upgrade logic: boost jump height


    if (interactionPrompt != null)
        interactionPrompt.SetActive(false); 
        


    Destroy(mushroom);  // Remove mushroom from the scene

    Debug.Log("Mushroom collected! Jump upgraded.");
    }

    private void ShowDialogue(string text)
    {
        dialogueTextUI.text = text;
        dialoguePanel.SetActive(true);
    }




    private void StartDialogue(string[] lines)
    {
        currentDialogue = lines;
        currentDialogueIndex = 0;
        inDialogue = true;
        ShowDialogue(currentDialogue[currentDialogueIndex]);
    }

    private void AdvanceDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < currentDialogue.Length)
        {
            ShowDialogue(currentDialogue[currentDialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        inDialogue = false;
        dialoguePanel.SetActive(false);
    }


    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void UpdateAnimationStates(){
   
    Vector3 scale = spriteTransform.localScale;

        if (moveInput.x > 0.1f)
            {
                scale.x = Mathf.Abs(scale.x); // face right
            }
        else if (moveInput.x < -0.1f)
            {
                scale.x = -Mathf.Abs(scale.x); // face left
            }

        spriteTransform.localScale = scale;

        // Check if player is moving
        bool isMoving = moveInput.magnitude > 0.1f;

        // Determine vertical motion states
        bool isJumpingUp = !isGrounded && velocity.y > 0.1f;
        bool isFalling = !isGrounded && velocity.y < -0.1f;

        // Allow walk/idle only when grounded and not in special states
        bool groundedMovementAllowed = isGrounded && !isJumpingUp && !isGliding && !isDashing;

        // Set animation booleans
        animator.SetBool("isJumping", isJumpingUp);
        animator.SetBool("isFalling", isFalling && !isGliding && !isDashing);
        animator.SetBool("isGliding", isGliding);
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isWalking", groundedMovementAllowed && isMoving);


    }




    public void ModifyJumpHeight(float amount)
    {
        jumpHeight = Mathf.Max(0.5f, jumpHeight + amount);
    }



    public void ModifyDashSpeed(float amount)
    {
        dashSpeed = Mathf.Max(1f, dashSpeed + amount);
    }


    public void ModifyGlideControl(float amount){
        maxGlideTime += amount; 
    }

    public void ModifyClimbStrength(float amount){
        
    }



}
