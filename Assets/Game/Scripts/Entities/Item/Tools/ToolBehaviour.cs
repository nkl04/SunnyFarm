using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;

public abstract class ToolBehaviour
{
    protected ToolDetail toolDetail;
    protected Player player;
    public ToolBehaviour(ToolDetail _toolDetail, Player _player)
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
