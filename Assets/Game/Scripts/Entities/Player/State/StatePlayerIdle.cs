namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;

    public class StatePlayerIdle : StatePlayer
    {
        public StatePlayerIdle(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        { }

        public override void CheckSwitchState()
        {
            if (this.player.IsMovePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerMove(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedTick()
        {
        }

        public override void Tick()
        {
            Debug.Log("Player is idle");
            CheckSwitchState();
        }
    }
}
