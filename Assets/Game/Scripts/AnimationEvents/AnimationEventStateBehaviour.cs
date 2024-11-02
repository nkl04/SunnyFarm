using UnityEngine;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;

    bool hasTriggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime % 1f;

        // Reset `hasTriggered` if animation restarts (detecting a new loop)
        if (currentTime < 0.1f && hasTriggered)
        {
            hasTriggered = false;
        }

        if (!hasTriggered && currentTime >= triggerTime)
        {
            NotifyReceiver(animator);
            hasTriggered = true;
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered = false;
    }

    void NotifyReceiver(Animator animator)
    {
        AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();

        if (receiver != null)
        {
            receiver.OnAnimationEventTriggered(eventName);
        }
    }
}