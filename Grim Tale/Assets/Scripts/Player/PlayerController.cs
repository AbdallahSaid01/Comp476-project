using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private LightProjectile lightProjectile;
    [SerializeField] private HeavyProjectile heavyProjectile;
    [SerializeField] private float lightProjectileSpeed;
    [SerializeField] private float heavyProjectileSpeed;
    [SerializeField] private float lightProjectileDamage;
    [SerializeField] private float heavyProjectileDamage;
    //If these are changed, change the text in the buy menu!
    [SerializeField] private float scaleAttackDMG;
    [SerializeField] private float scaleAttackSPD;
    ////////////////////////////////////////////////////////
    [SerializeField] private float heavyAttackTime = 0.5f;
    [SerializeField] private int heavyManaCost = 2;
    private float attackSPDIncrementLight;
    private float attackSPDIncrementHeavy;
    private float attackDMGincrementLight;
    private float attackDMGincrementHeavy;

    [HideInInspector] public PlayerInput input;
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    
    private Camera cam;
    private Vector3 camOffset;

    private bool heavyAttackIsUsable = true;
    private float timeRemaining;
    private float lightSpell;
    private float heavySpell;

    [SerializeField] private float healHPScale;
    [SerializeField] private float healManaScale;
    private float HpIncrement;
    private float ManaIncrement;
    public HealthBar healthBar;
    public ManaBar manaBar;
    private float maxHealth = 5.0f;
    private float health;
    private float maxMana = 5.0f;
    private float mana;
    private goldText goldtext;
    private int gold;

    private bool isDead;
    
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Direction = Animator.StringToHash("Direction");

    //Static variables for transition
    private static int goldStatic;
    private static float healthStatic;
    private static float manaStatic;
    private static float lightProjectileSpeedStatic;
    private static float heavyProjectileSpeedStatic;
    private static float lightProjectileDamageStatic;
    private static float heavyProjectileDamageStatic;


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

        //Fixed increments for scaling
        attackSPDIncrementLight = scaleAttackSPD * lightProjectileSpeed;
        attackSPDIncrementHeavy = scaleAttackSPD * heavyProjectileSpeed;
        attackDMGincrementLight = scaleAttackDMG * lightProjectileDamage;
        attackDMGincrementHeavy = scaleAttackDMG * heavyProjectileDamage;

        //UI elements to initialize
        HpIncrement = maxHealth * healHPScale;
        ManaIncrement = maxMana * healManaScale;
        goldtext = GameObject.FindGameObjectWithTag("goldtext").GetComponent<goldText>();

        if (SceneManager.GetActiveScene().buildIndex <= 1)
        {
            healthBar.SetMaxHealth(maxHealth);
            manaBar.SetMaxMana(maxMana);
            health = maxHealth;
            mana = maxMana;
            gold = 0;
        }
        else
        {
            healthBar.SetMaxHealth(maxHealth);
            manaBar.SetMaxMana(maxMana);
            health = healthStatic;
            mana = manaStatic;
            healthBar.SetHealth(health);
            manaBar.SetMana(mana);

            lightProjectileSpeed = lightProjectileSpeedStatic;
            heavyProjectileSpeed = heavyProjectileSpeedStatic;
            lightProjectileDamage = lightProjectileDamageStatic;
            heavyProjectileDamage = heavyProjectileDamageStatic;

            gold = goldStatic;
            goldtext.updateGoldText(gold);
        }
        
    }

    private void Update()
    {
        //Update static variable
        healthStatic = health;
        manaStatic = mana;
        lightProjectileSpeedStatic = lightProjectileSpeed;
        heavyProjectileSpeedStatic = heavyProjectileSpeed;
        lightProjectileDamageStatic = lightProjectileDamage;
        heavyProjectileDamageStatic = heavyProjectileDamage;
        goldStatic = gold;

        //Debug.Log(health);
        //Debug.Log(mana);


        if (isDead) return;
        
        View();
        Move();

        if(Time.timeScale != 0)
        {
            Rotate();
            Animate();
        }

    }

    public void Damage(int amount)
    {
        health = Mathf.Max(health - amount, 0);
        healthBar.SetHealth(health);
        if (isDead || !health.Equals(0)) return;
        
        isDead = true;
        animator.SetTrigger(Die);
    }

    public void Loot(int amount)
    {
        gold += amount;
    }

    public int getGold()
    {
        return gold;
    }

    public void setGold(int amount)
    {
        gold = amount;
        goldtext.updateGoldText(gold);
    }

    public void incrementLightAttackDMG()
    {
        lightProjectileDamage += attackDMGincrementLight;
    }

    public void incrementHeavyAttackDMG()
    {
        heavyProjectileDamage += attackDMGincrementHeavy;
    }

    public void incrementLightAttackSPD()
    {
        lightProjectileSpeed += attackSPDIncrementLight;
    }

    public void incrementHeavyAttackSPD()
    {
        heavyProjectileSpeed += attackSPDIncrementHeavy;
    }

    public void Heal()
    {
        health = Mathf.Min(health + HpIncrement, maxHealth);
        healthBar.SetHealth(health);
    }

    public void RegenMana()
    {
        mana = Mathf.Min(mana + ManaIncrement, maxMana);
        manaBar.SetMana(mana);
    }

    private void heavySpellCasted(int amount)
    {
        if(amount <= mana)
            mana -= amount;

        manaBar.SetMana(mana);
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
        if (isDead) return;
        
        var start = transform.position;
        start += new Vector3(0, 1, 0);
        var projectile = Instantiate(lightProjectile, start, transform.rotation);
        projectile.SetSpeed(lightProjectileSpeed);
    }

    private void HeavySpell()
    {
        if (isDead) return;
        
        var start = transform.position;
        start += new Vector3(0, 1, 0);
        var projectile = Instantiate(heavyProjectile, start, transform.rotation);
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

        if (Math.Abs(heavySpell - 1) < 0.01f && mana > heavyManaCost)
        {
            HeavySpell();
            heavySpellCasted(heavyManaCost);
        }
    }

    #endregion
}
