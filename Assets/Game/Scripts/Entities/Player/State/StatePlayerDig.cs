namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Constant;
    using UnityEngine;
    public class StatePlayerDig : StatePlayer
    {
        public StatePlayerDig(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        { }

        public override void CheckSwitchState()
        {
            if (!this.player.IsDigPressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
            this.player.Animator.SetBool(Constant.Player.IS_DIGGING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(Constant.Player.IS_DIGGING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}
