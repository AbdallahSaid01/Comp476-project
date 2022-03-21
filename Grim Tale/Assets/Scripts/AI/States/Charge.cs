using UnityEngine;

namespace AI.States
{
    public class Charge : State
    {
        private Vector3 chargeDirection;
        
        public Charge(Enemy enemy) : base(enemy)
        {
            name = StateName.Charge;
        }
        
        public override void Enter()
        {
            enemy.Agent.MaximumSpeed = 6f;
            
            enemy.Animator.SetInteger("State", 2);

            var playerPosition = enemy.Player.transform.position;
            var enemyPosition = enemy.transform.position + Vector3.up * 0.1f;
            var adjustedPlayerPosition = new Vector3(playerPosition.x, enemyPosition.y, playerPosition.z);
            chargeDirection = adjustedPlayerPosition - enemyPosition;
            
            Debug.DrawLine(enemyPosition, enemyPosition + chargeDirection * 50f, Color.black, 50f);
            if (!Physics.Raycast(enemyPosition, chargeDirection, out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("Obstacle"))) return;
            
            enemy.Agent.SetDestination(hitInfo.point);
            
            base.Enter();
        }

        public Vector3 ChargeDirection
        {
            get => chargeDirection;
        }
        
        // private IEnumerator PrepareCharge()
        // {
        //     do
        //     {
        //         var lookRotation = Quaternion.LookRotation(chargeDirection, Vector3.up);
        //         var lerpRotation = Quaternion.Lerp(enemy.transform.rotation, lookRotation, 50f * Time.deltaTime);
        //         enemy.transform.rotation = lerpRotation;
        //
        //         yield return null;
        //     } 
        //     while (Vector3.Angle(chargeDirection, enemy.transform.forward) > 10f);
        //     
        //     Physics.Raycast(enemy.transform.position, chargeDirection, out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("Obstacle"));
        //     enemy.Agent.SetDestination(hitInfo.point);
        //     
        //     enemy.Animator.SetInteger("State", 2);
        //     enemy.Agent.MaximumSpeed = 6f;
        // }
    }
}
