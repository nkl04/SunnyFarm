using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Player;
using SunnyFarm.Game.Inventory;
using SunnyFarm.Game.Managers.GameInput;
using SunnyFarm.Game.State.Player;
using SunnyFarm.Game.StateMachine;

public class StatePlayerFishing : StatePlayer
{
    public StatePlayerFishing(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
    {
    }

    public override void CheckSwitchState()
    {
        if (!this.player.IsFishingPressed)
        {
            this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
        }
    }

    public override void Enter()
    {
        this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_X, this.player.LastMovementInput.x);

        this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_Y, this.player.LastMovementInput.y);

        GameInputManager.Instance.CanToggleInventory = false;

        InventoryManager.Instance.CanChangeSelectedInventorySlot = false;
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
