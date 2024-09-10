namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;
    public class StatePlayerDig : StatePlayer
    {
        public StatePlayerDig(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        { }

        public override void CheckSwitchState()
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}
