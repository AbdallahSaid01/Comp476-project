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
            
            enemy.Player.Damage(enemy.Damage);
            enemy.ResetAttackTimer();
        }
    }
}
