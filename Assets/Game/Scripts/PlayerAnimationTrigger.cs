using System;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    public static event Action OnAnimationTrigger;
    public static event Action OnAnimationFinished;

    private void AnimationTrigger()
    {
        OnAnimationTrigger?.Invoke();
    }

    private void AnimationFinished()
    {
        OnAnimationFinished?.Invoke();
    }
}
