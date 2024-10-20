namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ToolData", menuName = "Items/Tool")]
    public class ToolDetail : ItemDetail
    {
        public float OffsetDistance = 1f;
        public float InteractableAreaSize = 0.5f;
    }
}