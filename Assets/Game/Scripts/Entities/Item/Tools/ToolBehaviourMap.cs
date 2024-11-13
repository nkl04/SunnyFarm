using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using static SunnyFarm.Game.Constant.Enums;

public class ToolBehaviourMap
{
    Dictionary<ToolType, ToolBehaviour> behaviours = new Dictionary<ToolType, ToolBehaviour>();
    public ToolBehaviourMap()
    {
    }

    public ToolBehaviour GetToolBehaviour(ConfigItemTool toolDetail, Player player)
    {
        if (behaviours.TryGetValue(toolDetail.ToolType, out ToolBehaviour behaviour))
        {
            return behaviour;
        }
        else
        {
            behaviours[toolDetail.ToolType] = CreateToolBehaviour(toolDetail, player);
            return behaviours[toolDetail.ToolType];
        }
    }

    private ToolBehaviour CreateToolBehaviour(ConfigItemTool toolDetail, Player player)
    {
        switch (toolDetail.ToolType)
        {
            case ToolType.Hoe:
                return new HoeBehaviour(toolDetail, player);
            case ToolType.Axe:
                return new AxeBehaviour(toolDetail, player);
            case ToolType.Pickaxe:
                return new PickaxeBehaviour(toolDetail, player);
            case ToolType.WateringCan:
                return new WateringCanBehaviour(toolDetail, player);
            case ToolType.FishingPole:
                return new FishingBehaviour(toolDetail, player);
            default:
                return null;
        }
    }
}
