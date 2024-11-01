using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public class PickaxeBehaviour : ToolBehaviour
{
    public PickaxeBehaviour(ToolDetail _toolDetail, Player _player) : base(_toolDetail, _player)
    {
    }

    public override void OnHold(ref bool isUsing)
    {
        if (!isUsing)
        {
            isUsing = true;
            player.IsPickaxePressed = true;
        }
    }


    public override void OnPress()
    {
    }

    public override void OnRelease()
    {

    }

    public override void Use(List<GridPropertiesDetail> tileDetails)
    {
        if (!tileDetails[0].HasCrop)
        {
            GridPropertiesController.Instance.SetLandGround(tileDetails[0]);
        }
    }

    public override void Reactivate()
    {
        player.IsPickaxePressed = false;
    }
}
