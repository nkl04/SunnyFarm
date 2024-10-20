namespace SunnyFarm.Game.Entities.Item.Data
{
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "ToolData", menuName = "Items/Item")]
    public class ItemDetail : ScriptableObject
    {
        public string ID;
        public ItemType ItemType;
        public string Name;
        public string Description;
        public Sprite ItemImage;
        public bool IsStackable;
        public int MaxStackSize;
        public bool CanBePickUp;
        public bool CanBeEaten;
    }
}