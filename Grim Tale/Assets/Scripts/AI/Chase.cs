using UnityEngine;

namespace AI
{
    public class Chase : State
    {
        public Chase(Enemy enemy) : base(enemy)
        {
            stateName = StateName.Chase;
        }
        
        public override void Enter()
        {
            enemy.Animator.SetInteger("State", 1);
            
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
            
            enemy.Agent.SetDestination(enemy.Player.transform.position);
        }
    }
}
