using UnityEngine;

namespace AI.States
{
    public class Chase : State
    {
        private FormationPoint formationPoint;
        
        public Chase(Enemy enemy) : base(enemy)
        {
            stateName = StateName.Chase;
        }
        
        public override void Enter()
        {
            formationPoint = FormationsManager.Instance.GetFormationPoint(enemy.Type);
            
            var random = Random.Range(0, 2);
            enemy.Animator.SetInteger("Random", random);
            enemy.Animator.SetInteger("State", 1);
            
            base.Enter();
        }

        public override void Update()
        {
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distanceToPlayer > enemy.ChaseDistance && formationPoint.Ready)
            {
                nextState = new Formation(enemy);
                stage = StateEvent.Exit;

                return;
            }
            
            enemy.Agent.SetDestination(enemy.Player.transform.position);
        }
    }
}
