using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public abstract class ToolBehaviour
{
    protected ConfigItemTool toolDetail;
    protected Player player;
    public ToolBehaviour(ConfigItemTool _toolDetail, Player _player)
    {
        toolDetail = _toolDetail;
        player = _player;
    }

    public abstract void OnPress();
    public abstract void OnHold(ref bool isUisng);
    public abstract void OnRelease();
    public abstract void Use(List<GridPropertiesDetail> tileDetails);
    public abstract void Reactivate();
}
