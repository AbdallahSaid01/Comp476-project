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
        
        private State state;
        private Animator animator;
        private PathfindingAgent agent;
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
        
        private void Update()
        {
            state = state?.Process();
            Debug.Log(gameObject + ": " + state?.ToString().ToUpper());
        }

        public Animator Animator => animator;
        public PathfindingAgent Agent => agent;
        public PlayerController Player => player;
        public float ChaseDistance => chaseDistance;
        public EnemyType Type => type;
        public EnemyType UpgradeType => upgradeType;
    }
}

public enum EnemyType { Skeleton, Charger }