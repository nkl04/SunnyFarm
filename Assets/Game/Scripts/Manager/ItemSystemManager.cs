namespace SunnyFarm.Game.Managers
{
    using System.Collections.Generic;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Crops.Data;
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;

    public class ItemSystemManager : Singleton<ItemSystemManager>
    {
        [SerializeField] private ConfigItemList configItemList;
        [SerializeField] private ConfigCropList configCropList;

        private Dictionary<string, ConfigItem> itemDetails = new Dictionary<string, ConfigItem>();
        private Dictionary<string, ConfigCrop> cropDetails = new Dictionary<string, ConfigCrop>();


        protected override void Awake()
        {
            base.Awake();
            CreateItemDetailsDictionary();
            CreateCropDetailsDictionary();
        }

        /// <summary>
        /// Populate the itemDetails dictionary with the itemDetails from the configItemList
        /// </summary>
        private void CreateItemDetailsDictionary()
        {
            itemDetails = new Dictionary<string, ConfigItem>();

            foreach (var itemDetail in configItemList.configItemDetails)
            {
                itemDetails.Add(itemDetail.ID, itemDetail);
            }
        }

        /// <summary>
        /// Populate the cropDetails dictionary with the cropDetails from the configCropList
        /// </summary>

        private void CreateCropDetailsDictionary()
        {
            cropDetails = new Dictionary<string, ConfigCrop>();

            foreach (var cropDetail in configCropList.configCropDetails)
            {
                cropDetails.Add(cropDetail.seedItemId, cropDetail);
            }
        }

        /// <summary>
        /// Get the item detail by item ID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ConfigItem GetItemDetail(string itemID)
        {
            if (itemID == null) return null;

            if (itemDetails.ContainsKey(itemID))
            {
                return itemDetails[itemID];
            }

            return null;
        }

        /// <summary>
        /// Get the crop detail by item seed ID
        /// </summary>
        /// <param name="seedItemId"></param>
        /// <returns></returns>

        public ConfigCrop GetCropDetail(string seedItemId)
        {
            if (seedItemId == null) return null;

            if (cropDetails.ContainsKey(seedItemId))
            {
                return cropDetails[seedItemId];
            }

            return null;
        }
    }
}
