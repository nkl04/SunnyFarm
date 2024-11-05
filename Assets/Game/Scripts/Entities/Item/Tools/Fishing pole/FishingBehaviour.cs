using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using UnityEngine;

public class FishingBehaviour : ToolBehaviour
{
    private FishingManager fishingManager;
    public FishingBehaviour(ConfigItemTool _toolDetail, Player _player, FishingManager _fishingManager) : base(_toolDetail, _player)
    {
        fishingManager = _fishingManager;
    }

    public override void OnHold(ref bool isUisng)
    {
        isUisng = true;
        fishingManager?.UpdateMeter(Time.deltaTime);
    }

    public override void OnPress()
    {
        fishingManager?.StartFishing();
    }

    public override void OnRelease()
    {
        fishingManager?.HideMeter();
    }

    public override void Reactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void Use(List<GridPropertiesDetail> tileDetails)
    {
        throw new System.NotImplementedException();
    }
}
