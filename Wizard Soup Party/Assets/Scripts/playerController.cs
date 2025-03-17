using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public CharacterController characterController; 
    public PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
            inputActions.Player.Enable();
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero; 
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        MoveCharacter(); 
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
