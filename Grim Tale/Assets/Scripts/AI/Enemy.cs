using System;
using UnityEngine;
using AI.Pathfinding;
using AI.States;

namespace AI
{
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private float chaseDistance = 20f;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] protected int damage = 1;
        [SerializeField] private float damageCooldown = 1f;
        [SerializeField] protected float health = 50f;
        [SerializeField] protected float maxHealth = 50f;
        [SerializeField] protected float damageByLightAttack = 10f;
        [SerializeField] protected float damageByHeavyAttack = 30f;
        [SerializeField] private bool hasAnimationAttack;
        [SerializeField] private EnemyType type;
        [SerializeField] private EnemyType upgradeType;
        [SerializeField] protected ParticleSystem killParticleSystem;
        [SerializeField] protected ParticleSystem damageParticleSystem;

        protected PathfindingAgent agent;
        protected State state;
        protected Animator animator;
        protected bool stateBlocked;
        protected PlayerController player;
        
        private float attackTimer;

        private float re = .75f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<PathfindingAgent>();
            player = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            state = new Idle(this);
        }
        
        protected virtual void Update()
        {
            if(!stateBlocked)
                state = state?.Process();

            attackTimer -= Time.deltaTime;

            isTooClose();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "LightProjectile")
            {
                Vector3 positionVector = other.transform.position;
                Instantiate(damageParticleSystem, positionVector, transform.rotation);
                Destroy(other.gameObject);
                health -= damageByLightAttack;

                if (health <= 0)
                {
                    Instantiate(killParticleSystem, positionVector, transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (other.transform.tag == "HeavyProjectile")
            {
                Vector3 positionVector = other.transform.position;
                Instantiate(damageParticleSystem, positionVector, transform.rotation);
                Destroy(other.gameObject);
                health -= damageByHeavyAttack;

                if (health <= 0)
                {
                    Instantiate(killParticleSystem, positionVector, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }

        private void isTooClose()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, re);

            foreach (var hitCollider in hitColliders)
            {
                //Add other enemy types.
                if ((hitCollider.transform.tag == "skelly" || hitCollider.transform.tag == "warchief" || hitCollider.transform.tag == "shaman" || hitCollider.transform.tag == "charger") && hitCollider.transform != gameObject.transform)
                {
                    if((hitCollider.transform.position - this.transform.position).magnitude < re)
                    {
                        Vector3 direction = ((hitCollider.transform.position - this.transform.position).normalized) * re;
                        hitCollider.transform.position = this.transform.position + direction;
                    }
                    
                }

            }

        }

        public void Heal(int amount)
        {
            health = Mathf.Min(health + amount, maxHealth);
        }

        public virtual void Attack()
        {
            player.Damage(damage);
        }

        public void ResetAttackTimer()
        {
            attackTimer = damageCooldown;
        }

        public Animator Animator => animator;
        public PathfindingAgent Agent => agent;
        public PlayerController Player => player;
        public float AttackDistance => attackDistance;
        public float ChaseDistance => chaseDistance;
        public int Damage => damage;
        public bool HasAnimationAttack => hasAnimationAttack;
        public EnemyType Type => type;
        public EnemyType UpgradeType => upgradeType;
        public bool CanUpgrade => state.name.Equals(StateName.Formation);
        public bool CanAttack => attackTimer < 0f;
        public bool StateBlocked { set => stateBlocked = value; }
    }
}

public enum EnemyType { Skeleton, MutantCharger, GoblinShaman, GoblinWarchief }