using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")] 
    private float _moveSpeed;
    public float normalSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public Animator animator;
    private bool _readyToJump;
    public bool acceptPlayerInput = true;

    [Header("Key binds")] 
    public KeyCode jumpKey;
    public KeyCode sprintKey;
    
    
    [Header("Ground Check")] 
    public float playerHeight;

    public LayerMask whatIsGround;
    private bool _grounded;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rb;

    private MovementState _movementState;
    private enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Air
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _readyToJump = true;
    }

    private void InputProcess()
    {
        if (!acceptPlayerInput) return;
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void Update()
    {
        GroundCheck();
        InputProcess();
        SpeedControl();
        MovementStateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        if(_grounded)
            _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f), ForceMode.Force);
        else if(!_grounded)
            _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f * airMultiplier), ForceMode.Force);
    }
    
    private void SpeedControl()
    {
        Vector3 velocity = _rb.velocity;
        Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
        
    }

    private void GroundCheck()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, .1f, whatIsGround);
        if (_grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }
    
    private void Jump()
    {
        // Reset vertical velocity and add force to make the player jump
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void MovementStateHandler()
    {
        if (_grounded && Input.GetKey(sprintKey))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            _movementState = MovementState.Sprinting;
            _moveSpeed = sprintSpeed;
        }
        else if (_grounded && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            _movementState = MovementState.Walking;
            _moveSpeed = normalSpeed;
        }
        else if(_grounded)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            _movementState = MovementState.Idle;
            _moveSpeed = normalSpeed;
        }
        else
        {
            _moveSpeed = normalSpeed;
            _movementState = MovementState.Air;
        }
    }
}
