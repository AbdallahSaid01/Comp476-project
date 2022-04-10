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
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distanceToPlayer > enemy.AttackDistance)
            {
                nextState = new Chase(enemy);
                stage = StateEvent.Exit;

                return;
            }

            if (!enemy.CanAttack) return;

            if (enemy.HasAnimationAttack)
            {
                enemy.Agent.SetDestination(enemy.transform.position, true);
                enemy.Agent.IsStopped = true;
            }
            else
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                enemy.Agent.IsStopped = false;
            }
            
            enemy.Player.Damage(enemy.Damage);
            enemy.ResetAttackTimer();
        }
    }
}
