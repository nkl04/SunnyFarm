using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using static SunnyFarm.Game.Constant.Enums;

public class FishingBehaviour : ToolBehaviour
{
    private bool isThrowBubber;
    public FishingBehaviour(ConfigItemTool _toolDetail, Player _player) : base(_toolDetail, _player)
    {
        EventHandlers.OnFishingEnd += CallAnimationFishingEnd;
    }

    public override void OnHold(ref bool isUisng)
    {
        if (!isUisng)
        {
            isUisng = true;

        }
    }

    public override void OnPress()
    {
        if (player.IsFishingPressed && !isThrowBubber)
        {
            // start the fishing game if possible

            EventHandlers.CallOnOpenFishingGame();
        }
        else
        {
            isThrowBubber = true;
            player.IsFishingPressed = true;
        }
    }

    private void CallAnimationFishingEnd()
    {
        this.player.Animator.SetBool(Constant.Player.IS_FISHING, false); // need to modify
        this.player.Animator.SetBool(Constant.Player.IS_END_FISHING, true); // need to modify
    }

    public override void OnRelease()
    {

    }

    public override void Reactivate()
    {
        this.player.Animator.SetBool(Constant.Player.IS_END_FISHING, false); // need to modify
        player.IsFishingPressed = false;

    }

    public override void Use(List<GridPropertiesDetail> tileDetails)
    {
        this.player.Animator.SetBool(Constant.Player.IS_START_FISHING, false); // need to modify

        if (tileDetails[0].TileType == TileType.Water)
        {
            isThrowBubber = false;
            EventHandlers.CallOnFishingStart();
            this.player.Animator.SetBool(Constant.Player.IS_FISHING, true); // need to modify
        }

        else
        {
            EventHandlers.CallOnResetFishingWhenTileNotWater();
            player.IsFishingPressed = false;
        }
    }
}
