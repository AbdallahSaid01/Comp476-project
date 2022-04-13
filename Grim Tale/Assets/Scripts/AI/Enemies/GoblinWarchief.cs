using System.Collections;
using System.Linq;
using UnityEngine;

namespace AI.Enemies
{
    public class GoblinWarchief : Enemy
    {
        private Enemy target;
        
        public override void Attack()
        {
            target = FindObjectsOfType<Enemy>().Where(x => x != this).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First(); // TODO Take from an eventual GameManager
            if (!target) return;

            StartCoroutine(RotateTowardsTarget());
        }

        private IEnumerator RotateTowardsTarget()
        {
            var position = transform.position;
            var targetPosition = target.transform.position;
            var adjustedTargetPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);
            var targetLookRotation = adjustedTargetPosition - position;
            do
            {
                var lookRotation = Quaternion.LookRotation(targetLookRotation, Vector3.up);
                var lerpRotation = Quaternion.Lerp(transform.rotation, lookRotation, 20f * Time.deltaTime);
                transform.rotation = lerpRotation;

                yield return null;
            } 
            while (Vector3.Angle(targetLookRotation, transform.forward) > 2f);
            
            animator.SetTrigger("Attack");
        }
        
        // Animation event
        private void InstantiateProjectile()
        {
            
        }
    }
}
