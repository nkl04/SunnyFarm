namespace SunnyFarm.Game.State.Player
{
    using Entities.Player;
    using StateMachine;
    using SunnyFarm.Game.Inventory;
    using SunnyFarm.Game.Managers.GameInput;

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

            // just temparary for debugging
            else
            if (this.player.IsAxePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerAxe(this.player, this.stateMachine));
            }
            else if (this.player.IsPickaxePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerPickaxe(this.player, this.stateMachine));
            }
            else if (this.player.IsWaterPressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerWater(this.player, this.stateMachine));
            }
            else if (this.player.IsDigPressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerDig(this.player, this.stateMachine));
            }
            else if (this.player.IsFishingPressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerFishing(this.player, this.stateMachine));
            }

        }

        public override void Enter()
        {
            GameInputManager.Instance.CanToggleInventory = true;
            InventoryManager.Instance.CanChangeSelectedInventorySlot = true;
        }

        public override void Exit()
        {
        }

        public override void Tick()
        {
            CheckSwitchState();
        }
    }
}
