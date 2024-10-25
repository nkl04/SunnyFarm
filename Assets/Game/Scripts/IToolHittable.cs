using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using static SunnyFarm.Game.Constant.Enums;

public interface IToolHittable
{
    void Hit(Player player);

    bool CanBeHit(List<ResourceType> canBeHit);
}
