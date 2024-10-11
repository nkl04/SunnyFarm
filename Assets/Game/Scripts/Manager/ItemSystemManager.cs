namespace SunnyFarm.Game.Managers
{
    using System;
    using System.Collections.Generic;
    using SunnyFarm.Game.Configs;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;

    public class ItemSystemManager : Singleton<ItemSystemManager>
    {
        [SerializeField] private ConfigItemList configItemList;
        private Dictionary<string, ItemDetail> itemDetails = new Dictionary<string, ItemDetail>();

        protected override void Awake()
        {
            base.Awake();
            CreateItemDetailsDictionary();
        }

        /// <summary>
        /// Populate the itemDetails dictionary with the itemDetails from the configItemList
        /// </summary>
        private void CreateItemDetailsDictionary()
        {
            itemDetails = new Dictionary<string, ItemDetail>();

            foreach (var itemDetail in configItemList.itemDetails)
            {
                itemDetails.Add(itemDetail.ID, itemDetail);
            }
        }

        /// <summary>
        /// Get the item detail by item ID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ItemDetail GetItemDetail(string itemID)
        {
            if (itemID == null) return null;

            if (itemDetails.ContainsKey(itemID))
            {
                return itemDetails[itemID];
            }

            return null;
        }
    }
}
