namespace SunnyFarm.Game.State.Player
{
    using Entities.Player;
    using StateMachine;
    using SunnyFarm.Game.Inventory;
    using SunnyFarm.Game.Managers.GameInput;

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
            this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_X, this.player.LastMovementInput.x);

            this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_Y, this.player.LastMovementInput.y);

            this.player.Animator.SetBool(Constant.Player.IS_WATERING, true);

            GameInputManager.Instance.CanToggleInventory = false;

            InventoryManager.Instance.CanChangeSelectedInventorySlot = false;
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(Constant.Player.IS_WATERING, false);
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}
