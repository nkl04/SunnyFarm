namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities;
    public class StatePlayerPickaxe : StatePlayer
    {
        public StatePlayerPickaxe(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        {
        }

        public override void CheckSwitchState()
        {
            if (!this.player.IsPickaxePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
            this.player.Animator.SetBool(Constant.Player.IS_PICKAXING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(Constant.Player.IS_PICKAXING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}