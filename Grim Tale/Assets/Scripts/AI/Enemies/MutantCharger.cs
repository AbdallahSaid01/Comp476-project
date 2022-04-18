using AI.States;
using UnityEngine;

namespace AI.Enemies
{
    public class MutantCharger : Enemy
    {
        [SerializeField] private ParticleSystem heavyAttackParticleSystemInChild;
        [SerializeField] private ParticleSystem lightAttackParticleSystemInChild;

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

            if (other.transform.tag == "LightProjectile")
            {
                Vector3 positionVector = other.transform.position;
                Instantiate(lightAttackParticleSystemInChild, positionVector, transform.rotation);
                Destroy(other.gameObject);
                Destroy(gameObject); //TODO: give each enemy a number of hit points it can take before dying. Right now the enemy just dies immediately
            }
            else if (other.transform.tag == "HeavyProjectile")
            {
                Vector3 positionVector = other.transform.position;
                Instantiate(heavyAttackParticleSystemInChild, positionVector, transform.rotation);
                Destroy(other.gameObject);
                Destroy(gameObject); //TODO: give each enemy a number of hit points it can take before dying
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
