namespace SunnyFarm.Game.Entities.Crops.Data
{
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "CropDetail", menuName = "Configs/Crop/CropDetail")]
    public class ConfigCrop : ScriptableObject
    {
        [Header("Seed")]
        [ItemAttribute]
        public string seedItemId;

        public int[] growthStages;

        public int totalGrowthDays;

        public int daysToRegrow;

        [Space(15)]

        public Sprite[] growthSprite;

        public Sprite harvestedSprite;

        [Space(15)]

        public Season[] seasons;

        [Header("Harvested Tool")]

        [ItemAttribute]
        public string harvestedToolItemId;

        [Header("Crop Product")]

        [ItemAttribute]
        public string[] cropProductedItemId;

        public int[] cropProductedMinQuantity;

        public int[] cropProductedMaxQuantity;



#if UNITY_EDITOR
        private void OnValidate()
        {
            if (growthStages == null) return;

            totalGrowthDays = 0;

            for (int i = 0; i < growthStages.Length; i++)
            {
                totalGrowthDays += growthStages[i];
            }
        }
#endif
    }
}

