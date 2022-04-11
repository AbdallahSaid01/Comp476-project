using UnityEngine;

namespace AI.States
{
    public class Idle : State
    {
        public Idle(Enemy enemy) : base(enemy)
        {
            name = StateName.Idle;
        }

        public override void Enter()
        {
            enemy.Animator.SetInteger("State", 0);
            
            base.Enter();
        }

        public override void Update()
        {
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            
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
            
            // Chase
            nextState = new Chase(enemy);
            stage = StateEvent.Exit;
        }
    }
}
