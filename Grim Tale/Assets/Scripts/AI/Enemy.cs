using UnityEngine;
using AI.Pathfinding;
using AI.States;

namespace AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 20f;
        [SerializeField] private EnemyType type;
        [SerializeField] private EnemyType upgradeType;

        protected PathfindingAgent agent;
        protected State state;
        protected Animator animator;
        protected bool stateBlocked;

        private PlayerController player;
        
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
            
            Debug.Log(gameObject + ": " + state?.ToString().ToUpper());
        }

        public Animator Animator => animator;
        public PathfindingAgent Agent => agent;
        public PlayerController Player => player;
        public float ChaseDistance => chaseDistance;
        public EnemyType Type => type;
        public EnemyType UpgradeType => upgradeType;

        public bool StateBlocked
        {
            get => stateBlocked;
            set => stateBlocked = value;
        }
    }
}

public enum EnemyType { Skeleton, MutantCharger }