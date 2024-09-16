namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern.Singleton;
    using SunnyFarm.Game.Item.Data;
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
        /// 
        /// </summary>
        private bool IsInventoryFullyOccupied()
        {
            foreach (var item in inventoryItems)
            {
                if (item.IsEmpty) return false;
            }
            return true;
        }

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
        /// 
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
        /// 
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
        /// 
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


        public void SwapItems(int itemIdx1, int itemIdx2)
        {
            InventoryItem item = inventoryItems[itemIdx1];
            inventoryItems[itemIdx1] = inventoryItems[itemIdx2];
            inventoryItems[itemIdx2] = item;
            InformAboutChange();
        }
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