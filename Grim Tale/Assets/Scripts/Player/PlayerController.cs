using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private LightProjectile lightProjectile;
    [SerializeField] private HeavyProjectile heavyProjectile;
    [SerializeField] private float lightProjectileSpeed;
    [SerializeField] private float heavyProjectileSpeed;
    [SerializeField] private float heavyAttackTime = 0.5f;

    [HideInInspector] public PlayerInput input;
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    
    private Camera cam;
    private Vector3 camOffset;

    private bool heavyAttackIsUsable = true;
    private float timeRemaining;

    private int maxHealth = 5;
    private int health;
    private int maxMana = 5;
    private int mana;
    private int gold;

    private float lightSpell;
    private float heavySpell;
    
    private static readonly int Direction = Animator.StringToHash("Direction");

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
        timeRemaining = heavyAttackTime;
        health = maxHealth;
        mana = maxMana;
    }

    private void Update()
    {
        View();
        Move();

        if (Time.timeScale != 0)
        {
            Rotate();
            Animate();
        }
        
    }

    public void Damage(int amount)
    {
        health = Mathf.Max(health - amount, 0);
        
        Debug.Log(health);
        if (health.Equals(0))
        {
            // Die
        }
    }

    public void Loot(int amount)
    {
        gold += amount;
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }

    public void RegenMana(int amount)
    {
        mana = Mathf.Min(mana + amount, maxMana);
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
            animator.SetInteger(Direction, 0);
        }
        else if (angle > -15f && angle <= 15f)
        {
            animator.SetInteger(Direction, 1);
        }
        else if (angle > 15f && angle <= 75f)
        {
            animator.SetInteger(Direction, 2);
        }
        else if (angle > 75f && angle <= 105f)
        {
            animator.SetInteger(Direction, 3);
        }
        else if (angle > 105f && angle <= 165f)
        {
            animator.SetInteger(Direction, 4);
        }
        else if (angle > 165f || angle <= -165f)
        {
            animator.SetInteger(Direction, 5);
        }
        else if (angle > -165f && angle <= -105f)
        {
            animator.SetInteger(Direction, 6);
        }
        else if (angle > -105f && angle <= -75f)
        {
            animator.SetInteger(Direction, 7);
        }
        else if (angle > -75f && angle <= -15f)
        {
            animator.SetInteger(Direction, 8);
        }
    }

    private void LightSpell()
    {
        Vector3 start = transform.position;
        start += new Vector3(0, 1, 0);
        LightProjectile projectile = Instantiate(lightProjectile, start, transform.rotation);
        projectile.SetSpeed(lightProjectileSpeed);
    }

    private void HeavySpell()
    {
        Vector3 start = transform.position;
        start += new Vector3(0, 1, 0);
        HeavyProjectile projectile = Instantiate(heavyProjectile, start, transform.rotation);
        projectile.SetSpeed(heavyProjectileSpeed);
    }
    
    #region Input

    private void OnEnable()
    {
        input.Controls.Enable();
        input.Controls.Move.performed += ReadMoveInput;
        input.Controls.Move.canceled += ReadMoveInput;
        input.Controls.SmallSpell.performed += ReadLightSpellInput;
        input.Controls.SmallSpell.canceled += ReadLightSpellInput;
        input.Controls.BigSpell.performed += ReadHeavySpellInput;
        input.Controls.BigSpell.canceled += ReadHeavySpellInput;

        if (heavySpell == 0)
        {
            timeRemaining = heavyAttackTime;
            heavyAttackIsUsable = true;
        }
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

    private void ReadLightSpellInput(InputAction.CallbackContext context)
    {   
        lightSpell = context.action.ReadValue<float>();

        if (Math.Abs(lightSpell - 1) < 0.01f)
        {
            LightSpell();
        }
    }

    private void ReadHeavySpellInput(InputAction.CallbackContext context)
    {
        heavySpell = context.action.ReadValue<float>();

        if (Math.Abs(heavySpell - 1) < 0.01f)
        {
            HeavySpell();
        }
    }

    #endregion
}
