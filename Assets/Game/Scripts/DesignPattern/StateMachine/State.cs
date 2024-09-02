using SunnyFarm.Game.Entities.Player;
using UnityEngine.Playables;

namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick();
        public abstract void FixedTick();
        public abstract void Exit();
        public abstract void CheckSwitchState();
    }

    public abstract class StatePlayer : State
    {
        protected StateMachine<StatePlayer> stateMachine;

        protected Player player;

        public StatePlayer(Player player, StateMachine<StatePlayer> stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }
    }

}
