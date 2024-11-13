namespace SunnyFarm.Game.Entities.Item.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "New ConfigItemTool", menuName = "Configs/Items/ConfigItem Tool")]
    public class ConfigItemTool : ConfigItem
    {
        public ToolType ToolType;
        public List<ResourceType> ResourceCanBeHit;
        public int ChargeCapacity;
        public int ChargeConsume;
        public int OffsetDistance = 1;
        public float InteractableAreaSize = 0.5f;
        public bool CanPowerUp = false;
    }
}