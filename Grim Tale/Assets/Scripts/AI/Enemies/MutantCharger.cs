using AI.States;
using UnityEngine;

namespace AI.Enemies
{
    public class MutantCharger : Enemy
    {
        private bool knockedDown;
        private Vector3 knockedDownDirection;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || state.name != StateName.Charge) return;
            
            if (!knockedDown)
            {
                knockedDownDirection = -((Charge) state).ChargeDirection.normalized;
                
                agent.IsStopped = true;
                agent.Path = null;
                knockedDown = true;
                animator.SetTrigger("KnockedDown");
                PushBack();
            }
        }

        protected override void Update()
        {
            if (agent.Arrived && !knockedDown && state.name == StateName.Charge)
            {
                knockedDownDirection = -((Charge) state).ChargeDirection.normalized;

                agent.IsStopped = true;
                agent.Path = null;
                knockedDown = true;
                animator.SetTrigger("KnockedDown");
                PushBack();
            }
            
            base.Update();
        }

        public void StandUp()
        {
            agent.IsStopped = false;
            knockedDown = false;
            
            if(!stateBlocked)
                state = new Idle(this);
        }

        private void PushBack()
        {
            var rb = GetComponent<Rigidbody>();
            rb.AddForce(knockedDownDirection * 20f, ForceMode.Impulse);
        }
    }
}
