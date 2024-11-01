using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public class PickaxeBehaviour : IToolBehaviour
{
    private ToolDetail toolDetail;
    private Player player;
    public PickaxeBehaviour(ToolDetail _toolDetail, Player _player)
    {
        toolDetail = _toolDetail;
        player = _player;
    }
    public void OnHold(ref bool isUsing)
    {
        if (!isUsing)
        {
            isUsing = true;
            player.IsPickaxePressed = true;
        }
    }


    public void OnPress()
    {
    }

    public void OnRelease()
    {

    }

    public void Use(List<GridPropertiesDetail> tileDetails)
    {
        if (!tileDetails[0].HasCrop)
        {
            GridPropertiesController.Instance.SetLandGround(tileDetails[0]);
        }
    }

    public void Reactivate()
    {
        player.IsPickaxePressed = false;
    }
}
