namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item.Data;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryDataController : Singleton<InventoryDataController>
    {
        [SerializeField] private InventoryItem[] bagItems;
        private Dictionary<string, InventoryItem[]> chests;

        public event Action<InventoryItem[]> OnBagUpdated;
        public event Action<string, InventoryItem[]> OnChestUpdated;

        public int InventoryLevel { get; private set; } = 1;

        private int maxSizeInventory;
        void Start()
        {
            GetData();
        }
        protected override void Awake()
        {
            base.Awake();
        }
        /// <summary>
        /// Check when the inventory is fully occupied
        /// </summary>
        private bool IsBagFullyOccupied()
        {
            foreach (var item in bagItems)
            {
                if (item.IsEmpty) return false;
            }
            return true;
        }
        /// <summary>
        /// Set up the size of the inventory item data and create each item in array
        /// </summary>
        /// <param name="size"></param>
        public void Setup()
        {
            bagItems = new InventoryItem[Constant.Inventory.MaxCapacity];
            for (int i = 0; i < bagItems.Length; i++)
            {
                bagItems[i] = new InventoryItem();
            }
        }

        public InventoryItem GetItemInBag(int index)
        {
            return bagItems[index];
        }

        public InventoryItem GetItemInChest(string chestID, int index)
        {
            return chests[chestID][index];
        }
        /// <summary>
        /// Logic that add item into inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(ItemDetail item, int quantity)
        {
            if (item.IsStackable == false)
            {
                while (quantity > 0 && !IsBagFullyOccupied())
                {
                    AddItemToFirstFreeSlot(item, 1);
                    quantity--;
                }
            }
            else
            {
                AddStackableItem(item, quantity);
            }
            InformAboutBagChange();
        }
        /// <summary>
        /// Add the item to the first free slot in inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItemToFirstFreeSlot(ItemDetail item, int quantity)
        {
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i].IsEmpty)
                {
                    bagItems[i].ChangeItem(item, quantity);
                    return;
                }
            }
        }
        /// <summary>
        /// Logic that add statckable into the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddStackableItem(ItemDetail item, int quantity)
        {
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i].IsEmpty) continue;

                if (bagItems[i].Item.ID == item.ID)
                {
                    int amountPossibleToTake = item.MaxStackSize - bagItems[i].Quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        bagItems[i].ChangeQuantity(item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        bagItems[i].ChangeQuantity(bagItems[i].Quantity + amountPossibleToTake);
                        return;
                    }
                }
            }
            while (quantity > 0 && !IsBagFullyOccupied())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
        }

        /// <summary>
        /// Logic that swap 2 bag item data
        /// </summary>
        /// <param name="itemIdx1"></param>
        /// <param name="itemIdx2"></param>
        public void SwapItemsInBag(UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        {
            int itemIdx1 = item1.Index;
            int itemIdx2 = item2.Index;

            InventoryItem item = bagItems[itemIdx1];
            bagItems[itemIdx1] = bagItems[itemIdx2];
            bagItems[itemIdx2] = item;

            InformAboutBagChange();
        }
        public void SwapItemsInChest(string id, UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        {
            var items = chests[id];

            int itemIdx1 = item1.Index;
            int itemIdx2 = item2.Index;

            InventoryItem item = items[itemIdx1];
            items[itemIdx1] = items[itemIdx2];
            items[itemIdx2] = item;

            InformAboutChestChange(id);
        }
        public void SwapItemsInDifLocation(string id, UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        {
            var chestItems = chests[id];

            int itemIdx1 = item1.Index;
            int itemIdx2 = item2.Index;

            InventoryItem item = null;
            if (item1.ItemLocation == InventoryLocation.Chest)
            {
                item = chestItems[itemIdx1];
                chestItems[itemIdx1] = bagItems[itemIdx2];
                bagItems[itemIdx2] = item;
            }
            else if (item1.ItemLocation == InventoryLocation.Player)
            {
                item = bagItems[itemIdx1];
                bagItems[itemIdx1] = chestItems[itemIdx2];
                chestItems[itemIdx2] = item;
            }

            InformAboutBagChange();
            InformAboutChestChange(id);
        }
        /// <summary>
        /// Remove quantity's item in the inventory
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="quantity"></param>
        public void RemoveBagItem(int itemIdx, int quantity)
        {
            if (bagItems[itemIdx].IsEmpty) return;

            int remainder = bagItems[itemIdx].Quantity - quantity;
            if (remainder <= 0)
            {
                bagItems[itemIdx].ChangeItem(null, 0);
            }
            else
            {
                bagItems[itemIdx].ChangeQuantity(remainder);
            }
            InformAboutBagChange();
        }
        public void RemoveChestItem(string id, int itemIdx, int quantity)
        {
            var items = chests[id];
            if (items[itemIdx].IsEmpty) return;

            int remainder = items[itemIdx].Quantity - quantity;
            if (remainder <= 0)
            {
                items[itemIdx].ChangeItem(null, 0);
            }
            else
            {
                items[itemIdx].ChangeQuantity(remainder);
            }
            InformAboutChestChange(id);
        }
        /// <summary>
        /// Events that trigger when having changes in the bag list data
        /// </summary>
        private void InformAboutBagChange()
        {
            OnBagUpdated?.Invoke(bagItems);
        }
        /// <summary>
        /// Events that trigger when having changes in the chest list data
        /// </summary>
        private void InformAboutChestChange(string id)
        {
            OnChestUpdated?.Invoke(id, chests[id]);
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

        }
    }
}