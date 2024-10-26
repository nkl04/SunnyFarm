using System;
using UnityEngine.Events;

[Serializable]
public class AnimationEvent
{
    public string EventName;
    public UnityEvent OnAnimationUnityEvent;

    public event Action OnAnimationEvent;

    public void InvokeEvent()
    {
        OnAnimationUnityEvent?.Invoke();
        OnAnimationEvent?.Invoke();
    }
}
