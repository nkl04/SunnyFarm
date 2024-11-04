namespace SunnyFarm.Game.Entities.Crops.Data
{
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using System.Linq;
    using Unity.VisualScripting;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "CropDetail", menuName = "Configs/Crop/ConfigCrop")]
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
            if (!string.IsNullOrEmpty(seedItemId))
            {
                var number = new string(seedItemId.Where(char.IsDigit).ToArray());

                var productId = "C" + number;

                if (cropProductedItemId.Length == 0)
                {
                    cropProductedItemId = new string[1];
                    cropProductedItemId[0] = productId;
                }

            }


            if (growthStages != null)
            {
                totalGrowthDays = 0;

                for (int i = 0; i < growthStages.Length; i++)
                {
                    totalGrowthDays += growthStages[i];
                }
            }
        }
#endif
    }
}

