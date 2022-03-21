using UnityEngine;

namespace AI
{
    public class AllowChangingState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Enemy>().StateBlocked = false;
        }
    }
}
