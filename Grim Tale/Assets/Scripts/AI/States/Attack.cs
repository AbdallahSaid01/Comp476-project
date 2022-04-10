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
            
            Debug.Log(enemy.CanAttack);
            if (!enemy.CanAttack) return;

            if (enemy.HasAnimationAttack)
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                enemy.Agent.IsStopped = true;
                
                // TODO Rotate towards player first
                enemy.Animator.SetInteger("State", 0);
                enemy.Animator.SetTrigger("Attack");
                enemy.ResetAttackTimer();
            }
            else
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                enemy.Agent.IsStopped = false;
                enemy.Player.Damage(enemy.Damage);
                enemy.ResetAttackTimer();
            }
        }
    }
}
