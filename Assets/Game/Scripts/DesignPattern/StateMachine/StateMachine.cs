namespace SunnyFarm.Game.StateMachine
{
    using UnityEngine;

    public class StateMachine<T> where T : State
    {
        private T currentState;

        public void TransitionTo(T newState)
        {

            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Tick()
        {
            if (currentState != null)
            {
                currentState.Tick();
            }
        }
    }
}
