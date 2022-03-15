using UnityEngine;

namespace AI
{
    public class FormationsManager : MonoBehaviour
    {
        public static FormationsManager Instance;

        [SerializeField] private FormationPoint skeletonFormationPoint;

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
                _ => null
            };
        }

        // Properties
        
        public FormationPoint SkeletonFormationPoint
        {
            get => skeletonFormationPoint;
        }
    }
}
