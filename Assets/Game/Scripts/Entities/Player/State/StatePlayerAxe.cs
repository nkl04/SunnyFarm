namespace SunnyFarm.Game.State.Player
{
    using Entities.Player;
    using StateMachine;

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
            this.player.Animator.SetBool(Constant.Player.IS_AXING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(Constant.Player.IS_AXING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}