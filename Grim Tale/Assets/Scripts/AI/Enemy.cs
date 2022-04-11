using System;
using UnityEngine;
using AI.Pathfinding;
using AI.States;

namespace AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 20f;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float damageCooldown = 1f;
        [SerializeField] private bool hasAnimationAttack;
        [SerializeField] private EnemyType type;
        [SerializeField] private EnemyType upgradeType;

        protected PathfindingAgent agent;
        protected State state;
        protected Animator animator;
        protected bool stateBlocked;

        private PlayerController player;
        private float attackTimer;
        
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

        // private void OnTriggerEnter(Collider other)
        // {
        //     var enemy = other.GetComponent<Enemy>();
        //     if (!enemy) return;
        //
        //     agent.FormationVector += (transform.position - enemy.transform.position).normalized;
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     var enemy = other.GetComponent<Enemy>();
        //     if (!enemy) return;
        //
        //     agent.FormationVector -= (transform.position - enemy.transform.position).normalized;
        // }

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