using System.Collections;
using System.Linq;
using UnityEngine;

namespace AI.Allies
{
    public class Healer : Ally
    {
        [Header("Healer")] 
        [SerializeField] [Min(0)] private int healingValue = 10;
        
        private GameObject target;
        
        public override void Attack()
        {
            var allies = FindObjectsOfType<Ally>().Where(x => x != this).ToArray();
            if (allies.Length != 0)
            {
                var ally = allies.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
                target = ally.gameObject;
            }
            
            if (!target || player.IsDamaged())
            {
                target = player.gameObject;
            }

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
            target.GetComponent<IHealable>().Heal(10);
        }
    }
}
