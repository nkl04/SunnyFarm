namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using UnityEngine;
    public class FactoryPlayerState : MonoBehaviour
    {
        private StateMachine stateMachine;

        public FactoryPlayerState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public State Idle()
        {
            return new StatePlayerIdle(stateMachine);
        }

        public State Move()
        {
            return new StatePlayerMove(stateMachine);
        }
    }
}