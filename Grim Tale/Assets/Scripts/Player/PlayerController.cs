using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;    
    
    private PlayerInput input;
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    
    private Camera cam;
    private Vector3 camOffset;

    [HideInInspector] public static int playerHealth = 5;
    [HideInInspector] public static int playerMana = 5;
    [HideInInspector] public static int playerGold = 0;

    private void Awake()
    {
        input = new PlayerInput();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    private void Start()
    {
        camOffset = new Vector3(0f, 11.11f, -4.18f);
    }

    private void Update()
    {
        View();        
        Move();
        Rotate();
        Animate();
    }

    private void View()
    {
        cam.transform.position = transform.position + camOffset;
    }

    private void Move()
    {
        controller.Move(velocity * Time.deltaTime * speed);
    }

    private void Rotate()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var relativePosition = mousePosition - screenCenter;
        var position = transform.position;
        var lookDirection = (position + new Vector3(relativePosition.x, 0f, relativePosition.y)).normalized;
        var lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        
        transform.rotation = lookRotation;
    }

    private void Animate()
    {
        var angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
        if (velocity == Vector3.zero)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (angle > -15f && angle <= 15f)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (angle > 15f && angle <= 75f)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (angle > 75f && angle <= 105f)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (angle > 105f && angle <= 165f)
        {
            animator.SetInteger("Direction", 4);
        }
        else if (angle > 165f || angle <= -165f)
        {
            animator.SetInteger("Direction", 5);
        }
        else if (angle > -165f && angle <= -105f)
        {
            animator.SetInteger("Direction", 6);
        }
        else if (angle > -105f && angle <= -75f)
        {
            animator.SetInteger("Direction", 7);
        }
        else if (angle > -75f && angle <= -15f)
        {
            animator.SetInteger("Direction", 8);
        }
    }

    #region Input

    private void OnEnable()
    {
        input.Controls.Enable();
        input.Controls.Move.performed += ReadMoveInput;
        input.Controls.Move.canceled += ReadMoveInput;
    }

    private void OnDisable()
    {
        input.Controls.Disable();
    }

    private void ReadMoveInput(InputAction.CallbackContext context)
    {
        var vector = context.action.ReadValue<Vector2>();
        velocity = new Vector3(vector.x, 0f, vector.y);
    }

    #endregion
}
