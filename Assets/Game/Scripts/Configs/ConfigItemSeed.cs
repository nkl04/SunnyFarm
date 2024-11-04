namespace SunnyFarm.Game.Entities.Item.Data
{
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New ConfigItemSeed", menuName = "Configs/Items/ConfigItem Seed")]
    public class ConfigItemSeed : ConfigItem
    {
        public bool CanBePlanted;

        public int buyPrice;

        public int sellPrice;
    }
}
