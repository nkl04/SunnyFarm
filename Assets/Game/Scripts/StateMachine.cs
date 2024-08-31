namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using UnityEngine;

    public class StateMachine
    {
        private State currentState;

        public void TransitionTo(State newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

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
