using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private Transform groundCheck;
    
    [SerializeField]
    private Transform cameraTransform;


    private Animator _animator;
    private bool _isGrounded = true;
    private Rigidbody _rigidbody;
    
    private Vector2 _movementInput;
    private Vector2 _rawMovementInput;
    
    private Vector3 _smoothMoveVelocity;
    
    private Vector3 _smoothedRotation;
    
    private float _smoothedRotationVelocity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayers);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // get movement input
        _movementInput = new Vector2(horizontal, vertical);
        
        // move the player based on where the camera is facing
        Vector3 moveDirection = cameraTransform.forward * _movementInput.y + cameraTransform.right * _movementInput.x;
        
        //Vector3 moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        moveDirection.Normalize();
        
        Debug.Log(moveDirection);
        
        // set the y value to 0 to prevent player from moving up and down
        moveDirection.y = 0;
        
        // check if jump is pressed and player is grounded
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _animator.SetTrigger("Jump");
        }
        
        // move the player
        Vector3 movementVelocity = moveDirection * speed;
        _rigidbody.velocity = new Vector3(movementVelocity.x, _rigidbody.velocity.y, movementVelocity.z);
    }
    
    public void AddJumpForce()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void Update()
    {
        // smooth change in movement direction
        // use Vector2.SmoothDamp to smooth the movement input
        // change fully in 0.1 seconds with deltaTime
        //_smoothMoveVelocity = Vector2.SmoothDamp(_smoothMoveVelocity, _movementInput, ref _smoothMoveVelocity, 0.1f * Time.deltaTime);

        //_smoothMoveVelocity = _movementInput;
        
        // then set MoveX and MoveY for the animator to blend between animations (-1 is left, 1 is right) (-1 is back, 1 is forward).
        // use speed to determine the value of MoveX and MoveY and clamp
        // base it on _smoothMoveVelocity
        _animator.SetFloat("MoveX", _movementInput.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("MoveY", _movementInput.y, 0.1f, Time.deltaTime);
        
        // set animator parameters. Set isMoving if player is moving
        _animator.SetBool("isMoving", _rigidbody.velocity.magnitude > 0.1f);
        
        // if there is movement input, rotate the player to the direction of the camera
        if (_movementInput.magnitude > 0.1f)
        {
            float newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTransform.eulerAngles.y, ref _smoothedRotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);
        }
    }
}