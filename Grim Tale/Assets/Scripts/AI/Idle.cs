using UnityEngine;

namespace AI
{
    public class Idle : State
    {
        public Idle(Enemy enemy) : base(enemy)
        {
            stateName = StateName.Idle;
        }

        public override void Enter()
        {
            enemy.Animator.SetInteger("State", 0);
            
            base.Enter();
        }

        public override void Update()
        {
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distanceToPlayer > enemy.ChaseDistance)
            {
                nextState = new Formation(enemy);
                stage = StateEvent.Exit;

                return;
            }

            nextState = new Chase(enemy);
            stage = StateEvent.Exit;
        }
    }
}
