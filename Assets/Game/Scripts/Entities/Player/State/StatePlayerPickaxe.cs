namespace SunnyFarm.Game.State.Player
{
    using Entities.Player;
    using StateMachine;
    using SunnyFarm.Game.Inventory;
    using SunnyFarm.Game.Managers.GameInput;

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
            this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_X, this.player.LastMovementInput.x);

            this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_Y, this.player.LastMovementInput.y);

            this.player.Animator.SetBool(Constant.Player.IS_PICKAXING, true);

            GameInputManager.Instance.CanToggleInventory = false;

            InventoryManager.Instance.CanChangeSelectedInventorySlot = false;
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