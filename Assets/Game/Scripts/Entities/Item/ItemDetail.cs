namespace SunnyFarm.Game.Entities.Item.Data
{
    using UnityEngine;
    using System;
    using static SunnyFarm.Game.Constant.Enums;

    [Serializable]
    public class ItemDetail
    {
        public string ID;
        public ItemType ItemType;
        public string Name;
        public string Description;
        public Sprite ItemImage;
        public bool IsStackable;
        public int MaxStackSize;
        public bool CanBePickUp;
        public bool CanBeCarried;
        public bool CanBeEaten;
    }
}