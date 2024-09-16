namespace SunnyFarm.Game.Item.Data
{
    using UnityEngine;

    public class Item : MonoBehaviour
    {
        public string ID;
        public string Name;
        public string Description;
        public Sprite ItemImage;
        public bool IsStackable;
        public int MaxStackSize;
    }
}