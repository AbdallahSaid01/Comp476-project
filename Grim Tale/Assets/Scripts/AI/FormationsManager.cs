using UnityEngine;

namespace AI
{
    public class FormationsManager : MonoBehaviour
    {
        public static FormationsManager Instance;

        [SerializeField] private FormationPoint skeletonFormationPoint;
        [SerializeField] private FormationPoint goblinShamanFormationPoint;
        [SerializeField] private Transform enemiesParent;

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        public FormationPoint GetFormationPoint(EnemyType type)
        {
            return type switch
            {
                EnemyType.Skeleton => skeletonFormationPoint,
                EnemyType.GoblinShaman => goblinShamanFormationPoint, 
                _ => null
            };
        }

        // Properties
        
        public Transform EnemiesParent
        {
            get => enemiesParent;
        }
    }
}
