namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Entities.Item.Data;
    using System;
    using static SunnyFarm.Game.Constant.Enums;

    [Serializable]
    public class ToolDetail : ItemDetail
    {
        public ToolType ToolType;
        public float OffsetDistance = 1f;
        public float InteractableAreaSize = 0.5f;
    }
}