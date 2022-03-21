using UnityEngine;

namespace AI.States
{
    public class Idle : State
    {
        private FormationPoint formationPoint;

        public Idle(Enemy enemy) : base(enemy)
        {
            name = StateName.Idle;
        }

        public override void Enter()
        {
            formationPoint = FormationsManager.Instance.GetFormationPoint(enemy.Type);
            enemy.Animator.SetInteger("State", 0);
            
            base.Enter();
        }

        public override void Update()
        {
            var distanceToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distanceToPlayer > enemy.ChaseDistance && formationPoint.Ready && enemy.Type != enemy.UpgradeType)
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
