using UnityEngine;

namespace AI.States
{
    public class Attack : State
    {
        public Attack(Enemy enemy) : base(enemy)
        {
            name = StateName.Attack;
        }

        public override void Update()
        {
            var playerPosition = enemy.Player.transform.position;
            var enemyPosition = enemy.transform.position;
            var distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);
            var directionToPlayer = playerPosition - enemyPosition;

            if (distanceToPlayer > enemy.AttackDistance || Physics.Raycast(enemyPosition + Vector3.up * 0.1f, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Obstacle")))
            {
                if (enemy.HasAnimationAttack)
                {
                    enemy.Animator.SetTrigger("InterruptAttack");
                }
                nextState = new Chase(enemy);
                stage = StateEvent.Exit;

                return;
            }
            
            if (!enemy.CanAttack) return;

            if (enemy.HasAnimationAttack)
            {
                enemy.Animator.SetInteger("State", 0);
                enemy.Agent.IsStopped = true;
                enemy.Agent.ControlRotation = false;

                enemy.Attack();
                enemy.ResetAttackTimer();
            }
            else
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                enemy.Agent.IsStopped = false;
                enemy.Agent.ControlRotation = true;
                enemy.Player.Damage(enemy.Damage);
                enemy.ResetAttackTimer();
            }
        }
    }
}
