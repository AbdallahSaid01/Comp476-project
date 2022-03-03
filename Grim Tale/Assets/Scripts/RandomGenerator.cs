using UnityEngine;

public class RandomGenerator : StateMachineBehaviour
{
    [SerializeField, Tooltip("Inclusive")] private int min, max;
    private static readonly int Random1 = Animator.StringToHash("Random");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var random = Random.Range(min, max + 1);
        animator.SetInteger(Random1, random);
    }
}
