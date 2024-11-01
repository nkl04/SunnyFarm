namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "ToolData", menuName = "Items/Tool")]
    public class ToolDetail : ItemDetail
    {
        public ToolType toolType;
        public int ChargeCapacity;
        public int ChargeConsume;
        public float OffsetDistance = 1f;
        public float InteractableAreaSize = 0.5f;
        public bool CanPowerUp = false;
    }
}