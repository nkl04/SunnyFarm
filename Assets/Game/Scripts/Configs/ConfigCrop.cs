namespace SunnyFarm.Game.Entities.Crops.Data
{
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "CropDetail", menuName = "Configs/Crop/CropDetail")]
    public class ConfigCrop : ScriptableObject
    {
        [Header("Seed Details")]
        [ItemAttribute]
        public string seedItemId;

        public int[] growthStages;

        public int totalGrowthDays;

        public Sprite[] growthSprite;

        public Sprite harvestedSprite;

        public Season[] seasons;

        [Header("Harvested Item Details")]

        [ItemAttribute]
        public string harvestedItemId;

        [Header("Harvested Tool Details")]

        [ItemAttribute]
        public string harvestedToolItemId;

        [Header("Crop Product Details")]

        [ItemAttribute]
        public string[] cropProductedItemId;

        public int[] cropProductedMinQuantity;

        public int[] cropProductedMaxQuantity;

        [Space(10)]

        public int daysToRegrow;

    }
}

