using UnityEngine;

namespace AI.States
{
    public class Chase : State
    {
        public Chase(Enemy enemy) : base(enemy)
        {
            name = StateName.Chase;
        }
        
        public override void Enter()
        {
            enemy.Agent.IsStopped = false;
            enemy.Agent.MaximumSpeed = GetTypeSpeed();
            
            var random = GetAnimationVariations();
            enemy.Animator.SetInteger("Random", random);
            enemy.Animator.SetInteger("State", 1);
            
            base.Enter();
        }

        public override void Update()
        {
            var playerPosition = enemy.Player.transform.position;
            var enemyPosition = enemy.transform.position;
            var distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);
            var directionToPlayer = playerPosition - enemyPosition;
            
            // Formation
            if (distanceToPlayer > enemy.ChaseDistance && formationPoint is {Ready: true} && enemy.Type != enemy.UpgradeType)
            {
                nextState = new Formation(enemy);
                stage = StateEvent.Exit;

                return;
            }
            
            // Attack
            if (distanceToPlayer < enemy.AttackDistance)
            {
                nextState = new Attack(enemy);
                stage = StateEvent.Exit;

                return;
            }
            
            // Charge
            if (enemy.Type == EnemyType.MutantCharger && !Physics.Raycast(enemyPosition + Vector3.up * 0.1f, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Obstacle")))
            {
                nextState = new Charge(enemy);
                stage = StateEvent.Exit;

                return;
            }
            
            // Chase
            enemy.Agent.SetDestination(enemy.Player.transform.position);
        }

        private float GetTypeSpeed()
        {
            switch (enemy.Type)
            {
                case EnemyType.Skeleton:
                    return 1.5f;
                case EnemyType.MutantCharger:
                    return 1.5f;
                default:
                    return 1.5f;
            }
        }

        private int GetAnimationVariations()
        {
            switch (enemy.Type)
            {
                case EnemyType.Skeleton:
                    return Random.Range(0, 2);
                case EnemyType.MutantCharger:
                    return Random.Range(0, 2);
                default:
                    return Random.Range(0, 1);
            }
        }
    }
}
