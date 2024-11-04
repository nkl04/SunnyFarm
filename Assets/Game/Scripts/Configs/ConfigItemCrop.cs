namespace SunnyFarm.Game.Entities.Item.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New ConfigItemCrop", menuName = "Configs/Items/ConfigItem Crop")]
    public class ConfigItemCrop : ConfigItem
    {
        public bool CanBeEaten;

        public int buyPrice;

        public int sellPrice;
    }
}
