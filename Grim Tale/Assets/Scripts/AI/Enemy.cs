using UnityEngine;
using AI.Pathfinding;

namespace AI
{
    public class Enemy : MonoBehaviour
    {
        private State state;
        private Animator animator;
        private PathfindingAgent agent;
        private PlayerController player;
        
        [SerializeField] private float chaseDistance = 20f;
        
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
        
        void Update()
        {
            state = state?.Process();
            Debug.Log(gameObject + ": " + state?.ToString().ToUpper());
        }

        public Animator Animator => animator;
        public PathfindingAgent Agent => agent;
        public PlayerController Player => player;
        public float ChaseDistance => chaseDistance;
    }
}
