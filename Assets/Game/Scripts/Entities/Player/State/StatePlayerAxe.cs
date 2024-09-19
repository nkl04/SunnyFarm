namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;

    public class StatePlayerAxe : StatePlayer
    {
        public StatePlayerAxe(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        {
        }

        public override void CheckSwitchState()
        {
            if (!this.player.IsAxePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
            this.player.Animator.SetBool(player.IS_AXING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(player.IS_AXING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}