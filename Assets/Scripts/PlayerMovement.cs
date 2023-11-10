using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles the player's movement, including walking, sprinting, jumping, and camera rotation.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 1f;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float groundDrag = 1f;
    public float airDrag = 0.5f;
    public float airSpeedMultiplier = 1.1f; // 10% faster than regular speed
    
    
    [Header("Precision Jumping")]
    public float jumpForce = 0.5f;
    public float maxJumpForce = 5f;
    public float maxChargeTime = 5f; 
    internal bool isPreparingJump = false;
    private float jumpChargeTime = 0f;
    public float precisionJumpPrepMoveSpeed = 0.5f;
    public float forwardMomentumMultiplier = 0.5f; // Adjust this value to control the amount of forward momentum
    
    [Header("Camera Rotation")]
    public float rotationPower = 3f;
    public GameObject followTransform;
    [HideInInspector]
    public Vector2 look;
    
    [Header("Web Anchoring")]
    private WebAnchoring _webAnchoring;
    
    private float _moveInputHorizontal;
    private float _moveInputVertical;
    private Rigidbody _rigidbody;

    [Header("Focus Mode")]
    private float focusMultiplier = 1f;    
    #endregion

    #region UnityMethods

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _webAnchoring = GetComponent<WebAnchoring>();
        //Locking the cursor so it doesnt go boing boing around TODO:Move it to game manager maybe.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        _moveInputHorizontal = Input.GetAxisRaw("Horizontal");
        _moveInputVertical = Input.GetAxisRaw("Vertical");

        // Handle camera rotation
        look.x = Input.GetAxis("Mouse X");
        look.y = Input.GetAxis("Mouse Y");
        HandleCameraRotation();

        
        //Handle precision jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isPreparingJump = true;
            jumpChargeTime = 0f;
        }

        if (Input.GetButtonUp("Jump") && isPreparingJump)
        {
            isPreparingJump = false;
            OnPrecisionJump();
        }

        if (isPreparingJump)
        {
            jumpChargeTime = Mathf.Min(jumpChargeTime + Time.deltaTime * focusMultiplier, maxChargeTime);
        }

        
    }

    void FixedUpdate()
    {
        OnMove();
    }

    

    #endregion

    #region CustomMethods

    /// <summary>
    /// Handles the camera's rotation based on mouse input.
    /// </summary>
    void HandleCameraRotation()
    {
        // Horizontal rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);
        // Vertical rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.y * rotationPower, Vector3.right);
        

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        // Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    void OnMove()
    {
        float currentSpeed;
    
        if (isPreparingJump)
        {
            currentSpeed = precisionJumpPrepMoveSpeed;
        }
        else
        {
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        }
        
        currentSpeed *= focusMultiplier;
        
        _rigidbody.drag = IsGrounded() ? groundDrag : airDrag;

        if (_moveInputHorizontal != 0 || _moveInputVertical != 0)
        {
            // Desired forward direction
            Vector3 desiredForward = Vector3.Normalize(followTransform.transform.forward);
            desiredForward.y = 0; // Ensure no vertical component
            
            // Target rotation
            Quaternion targetRotation = Quaternion.LookRotation(desiredForward);

            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);  // 10f is a speed factor, adjust as necessary
            
            Vector3 lookTargetForward = followTransform.transform.forward; // Use the followTransform's forward direction
            Vector3 lookTargetRight = followTransform.transform.right; // Use the followTransform's right direction
            
            
            // Remove the y component
            lookTargetForward.y = 0;
            lookTargetRight.y = 0;

            // Normalize the vectors
            lookTargetForward.Normalize();
            lookTargetRight.Normalize();

            // Calculate the move direction based on the followTransform's orientation
            Vector3 moveDirection = lookTargetForward * _moveInputVertical + lookTargetRight * _moveInputHorizontal;
            moveDirection.Normalize();

            if (IsGrounded())
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
                Vector3 groundNormal = hit.normal;
                Vector3 projectedMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);
                _rigidbody.velocity = new Vector3(projectedMoveDirection.x * currentSpeed, _rigidbody.velocity.y, projectedMoveDirection.z * currentSpeed);
            }
            else
            {
                // Increase speed by airSpeedMultiplier when in air
                _rigidbody.velocity = new Vector3(moveDirection.x * currentSpeed * airSpeedMultiplier, _rigidbody.velocity.y, moveDirection.z * currentSpeed * airSpeedMultiplier);
            }
            
            
            
        }
    }

    /// <summary>
    /// Initiates a jump if the player is grounded.
    /// </summary>
    void OnPrecisionJump()
    {
        float chargeRatio = jumpChargeTime / maxChargeTime;
        float calculatedJumpForce = Mathf.Lerp(jumpForce, maxJumpForce, chargeRatio);
    
        // Calculate forward momentum based on charge ratio
        Vector3 forwardMomentum = followTransform.transform.forward * forwardMomentumMultiplier * chargeRatio;

        // Apply jump force and forward momentum
        _rigidbody.velocity = new Vector3(forwardMomentum.x, calculatedJumpForce, forwardMomentum.z);
    }

    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    /// <returns>True if the player is grounded, false otherwise.</returns>
    public bool IsGrounded()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
        return isGrounded;
        //Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
    }

    /// <summary>
    /// Connects to the UI to charge the slider
    /// </summary>
    public float GetJumpCharge()
    {
        return jumpChargeTime / maxChargeTime; // This returns a value between 0 and 1
    }
    
    
    /// <summary>
    /// Connected to the Visual Acuity script to change movement speed
    /// </summary>
    ///
    public void SetFocusMultiplier(float multiplier)
    {
        focusMultiplier = multiplier;
    }
    #endregion
}