namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;

    public class StatePlayerWater : StatePlayer
    {
        public StatePlayerWater(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        {
        }

        public override void CheckSwitchState()
        {
            if (!this.player.IsWaterPressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
            this.player.Animator.SetBool(player.IS_WATERING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(player.IS_WATERING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}