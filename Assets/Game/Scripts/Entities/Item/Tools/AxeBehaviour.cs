using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public class AxeBehaviour : IToolBehaviour
{
    private float holdTime;
    private ToolDetail toolDetail;
    private Player player;
    public AxeBehaviour(ToolDetail _toolDetail, Player _player)
    {
        toolDetail = _toolDetail;
        player = _player;
    }
    public void OnHold(ref bool isUsing)
    {
        if (!isUsing)
        {
            isUsing = true;
            player.IsAxePressed = true;
        }
    }


    public void OnPress()
    {
        holdTime = 0f;  // Reset hold time when pressed
    }

    public void OnRelease()
    {

    }

    public void Use(List<GridPropertiesDetail> tileDetails)
    {

    }

    public void Reactivate()
    {
        player.IsAxePressed = false;
    }
}
