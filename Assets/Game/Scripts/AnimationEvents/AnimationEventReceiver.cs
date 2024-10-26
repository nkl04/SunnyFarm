using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] List<AnimationEvent> animationEvents = new();

    public void OnAnimationEventTriggered(string eventName)
    {
        AnimationEvent matchingEvent = animationEvents.Find(se => se.EventName == eventName);
        matchingEvent?.InvokeEvent();
    }

    public void AddAnimationEvent(AnimationEvent animationEvent)
    {
        animationEvents.Add(animationEvent);
    }
}
