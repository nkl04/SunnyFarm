using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public class AxeBehaviour : ToolBehaviour
{
    public AxeBehaviour(ToolDetail _toolDetail, Player _player) : base(_toolDetail, _player)
    {
    }

    public override void OnHold(ref bool isUsing)
    {
        if (!isUsing)
        {
            isUsing = true;
            player.IsAxePressed = true;
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

    }

    public override void Reactivate()
    {
        player.IsAxePressed = false;
    }
}
