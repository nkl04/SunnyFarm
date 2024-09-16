
namespace SunnyFarm.Game.Item.Data
{
    using UnityEngine;
    public abstract class Item : MonoBehaviour
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Sprite ItemImage { get; set; }
        public bool IsStackable { get; set; }
        public int MaxStackSize { get; set; } = 1;
    }
}