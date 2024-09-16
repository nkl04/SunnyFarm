namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern.Singleton;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class InventoryDataController : Singleton<InventoryDataController>
    {

        private List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public int InventoryLevel { get; private set; } = 1;
        void Start()
        {
            GetData();
            Debug.Log(inventoryItems[0].Quantity);
        }
        protected override void Awake()
        {
            base.Awake();
        }
        void Update()
        {

        }
        /// <summary>
        /// Save the data of the inventory to file
        /// </summary>
        void SaveData()
        {
        }
        /// <summary>
        /// Get the data of the inventory from the file
        /// </summary>
        void GetData()
        {
            // Just uses for testing
            for (int i = 0; i < 10; i++)
            {
                InventoryItem item = new();
                item.ChangeQuantity(1);
                inventoryItems.Add(item);
            }
            //
        }
    }
}