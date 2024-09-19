namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern.Singleton;
    using SunnyFarm.Game.Entities.Item.Data;
    using System;
    using UnityEngine;

    public class InventoryDataController : Singleton<InventoryDataController>
    {
        [SerializeField] private InventoryItem[] inventoryItems;

        public event Action<InventoryItem[]> OnInventoryUpdated;

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
        private bool IsInventoryFullyOccupied()
        {
            foreach (var item in inventoryItems)
            {
                if (item.IsEmpty) return false;
            }
            return true;
        }
        /// <summary>
        /// Set up the size of the inventory item data and create each item in array
        /// </summary>
        /// <param name="size"></param>
        public void SetupSize(int size)
        {
            maxSizeInventory = size;
            inventoryItems = new InventoryItem[size];
            for (int i = 0; i < size; i++)
            {
                inventoryItems[i] = new InventoryItem();
            }
        }

        /// <summary>
        /// Logic that add item into inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(Item item, int quantity)
        {
            if (item.IsStackable == false)
            {
                while (quantity > 0 && !IsInventoryFullyOccupied())
                {
                    AddItemToFirstFreeSlot(item, 1);
                    quantity--;
                }
            }
            else
            {
                AddStackableItem(item, quantity);
            }
            InformAboutChange();
        }
        /// <summary>
        /// Add the item to the first free slot in inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItemToFirstFreeSlot(Item item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i].ChangeItem(item, quantity);
                    return;
                }
            }
        }
        /// <summary>
        /// Logic that add statckable into the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddStackableItem(Item item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;

                if (inventoryItems[i].Item.ID == item.ID)
                {
                    int amountPossibleToTake = item.MaxStackSize - inventoryItems[i].Quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i].ChangeQuantity(item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i].ChangeQuantity(inventoryItems[i].Quantity + amountPossibleToTake);
                        return;
                    }
                }
            }
            while (quantity > 0 && !IsInventoryFullyOccupied())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
        }

        /// <summary>
        /// Logic that swap 2 inventory item data
        /// </summary>
        /// <param name="itemIdx1"></param>
        /// <param name="itemIdx2"></param>
        public void SwapItems(int itemIdx1, int itemIdx2)
        {
            InventoryItem item = inventoryItems[itemIdx1];
            inventoryItems[itemIdx1] = inventoryItems[itemIdx2];
            inventoryItems[itemIdx2] = item;
            InformAboutChange();
        }
        /// <summary>
        /// Remove quantity's item in the inventory
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="quantity"></param>
        public void RemoveItem(int itemIdx, int quantity)
        {
            if (inventoryItems[itemIdx].IsEmpty) return;

            int remainder = inventoryItems[itemIdx].Quantity - quantity;
            if (remainder <= 0)
            {
                inventoryItems[itemIdx].ChangeItem(null, 0);
            }
            else
            {
                inventoryItems[itemIdx].ChangeQuantity(remainder);
            }
            InformAboutChange();
        }
        /// <summary>
        /// Events that trigger when having changes in the inventory list data
        /// </summary>
        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(inventoryItems);
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