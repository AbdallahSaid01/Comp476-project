using UnityEngine;

namespace AI
{
    public class Formation : State
    {
        public Formation(Enemy enemy) : base(enemy)
        {
            stateName = StateName.Formation;
        }

        public override void Enter()
        {
            var random = Random.Range(0, 2);
            enemy.Animator.SetInteger("Random", random);
            enemy.Animator.SetInteger("State", 1);
            
            base.Enter();
        }
        
        public override void Update()
        {
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distanceToPlayer <= enemy.ChaseDistance)
            {
                nextState = new Chase(enemy);
                stage = StateEvent.Exit;
            }
        }
    }
}
