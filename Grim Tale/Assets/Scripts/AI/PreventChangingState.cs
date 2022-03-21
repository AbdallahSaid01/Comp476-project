using UnityEngine;

namespace AI
{
    public class PreventChangingState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Enemy>().StateBlocked = true;
        }
    }
}
