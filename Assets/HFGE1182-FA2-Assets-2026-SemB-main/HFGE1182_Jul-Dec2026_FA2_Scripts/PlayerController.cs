using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions InputAction;
    
    [Header("MOVEMENT PARAMETERS")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public Rigidbody rb;
    public bool isSprinting;
    private Vector2 moveDirection;
    
    [Header("CAMERA PARAMETERS")]
    [SerializeField]
    private Camera playerCamera;
    public float mouseSensitivity;
    public float lookLimitX;
    private float cameraAngle = 0f;
    private Vector2 lookInput;
    
    [Header("CROUCHING")]
    public CapsuleCollider hitbox;
    [SerializeField]
    private bool isCrouching;
    [SerializeField]
    private float crouchHeight = 1f;
    [SerializeField]
    private float standingHeight = 2f;
    [SerializeField]
    private float cameraHeight = 1.5f;
    
    
    [Header("GROUND DETECTION")]
    [SerializeField]
    private bool isGrounded = true;
    public float groundCheckDistance;
    public LayerMask groundLayer;


    private PlayerAnimationHandler animationHandler;
    
    private void Awake()
    {
       rb = GetComponent<Rigidbody>();
       animationHandler = GetComponent<PlayerAnimationHandler>();
       InputAction = new PlayerInputActions();
    }

    #region BostonCity Campus
    /* This code is property of Boston City Campus, and should not be used, or copied for any other subject.
     If you would copy and or use this in an assessment outside the assessment that this was supplied for
     action will be taken against you for plaigirism.
    */
    #endregion
    
    private void OnEnable()
    {
        InputAction.Player.Enable();
        
        InputAction.Player.Move.performed += context => moveDirection = context.ReadValue<Vector2>();
        InputAction.Player.Move.canceled += context => moveDirection = Vector2.zero;
        
        InputAction.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>();
        InputAction.Player.Look.canceled += context => lookInput = Vector2.zero;

        InputAction.Player.Jump.performed += OnJump;
        InputAction.Player.Crouch.performed += OnCrouch;
        InputAction.Player.Sprint.performed += OnSprint;
        InputAction.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        InputAction.Player.Jump.performed -= OnJump;
        InputAction.Player.Crouch.performed -= OnCrouch;
        InputAction.Player.Sprint.performed -= OnSprint;
        InputAction.Player.Sprint.canceled -= OnSprint;
        InputAction.Player.Disable();  
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        LookHandler();
    }

    private void FixedUpdate()
    {
        MoveHandler();
        GroundDetection();
    }

    public void MoveHandler()
    {
        Vector3 moveDir = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = moveDir * currentSpeed;
        
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        if (moveDirection.magnitude > 0.1f)
        {
            animationHandler.SetIsMoving(true); //Animation Call
        }
        else
        {
            animationHandler.SetIsMoving(false); //Animation Call
        }
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = !isSprinting;
    }

    
    public void LookHandler()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        
        
        cameraAngle -= mouseY;
        cameraAngle = Mathf.Clamp(cameraAngle, -lookLimitX, lookLimitX);

        
        playerCamera.transform.localRotation = Quaternion.Euler(cameraAngle, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            animationHandler.Jump(); //Animation Call
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (isCrouching)
        {
            Crouch();
        }
    }

    public void Crouch()
    {
        float targetHeight = isCrouching ? crouchHeight:standingHeight;
        hitbox.height = Mathf.Lerp(hitbox.height, targetHeight, 0.2f);
        
        Vector3 camLocalPos = playerCamera.transform.localPosition;
        camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetHeight - 0.25f, Time.deltaTime * .2f);
        hitbox.height = targetHeight;
        playerCamera.transform.localPosition = camLocalPos;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = !isCrouching;
        animationHandler.SetIsCrouching(isCrouching); //Animation Call
        Crouch();
    }

    public void GroundDetection()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(start, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
        animationHandler.SetIsGrounded(isGrounded); //Animation Call
    }
}
