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
    private Vector3 velocity;

    private void Awake()
    {
        input = new PlayerInput();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        controller.Move(velocity * Time.deltaTime * speed);
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
