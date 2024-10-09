namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryDataController : Singleton<InventoryDataController>
    {
        public event Action<List<InventoryItem>> OnBagUpdated;
        public event Action<string, List<InventoryItem>> OnChestUpdated;

        public int InventoryLevel { get; private set; } = 1;
        [HideInInspector] public List<InventoryItem>[] inventoryArray; // array of inventory list
        [HideInInspector] public int[] inventoryListCapacityArray; // capacity of each inventory list
        [SerializeField] private List<InventoryItem> inventoryItemList;// list of items in the inventory

        // private Dictionary<string, InventorySlot[]> chests;
        protected override void Awake()
        {
            base.Awake();

            CreateInventoryList();
        }

        private void CreateInventoryList()
        {
            inventoryArray = new List<InventoryItem>[(int)InventoryLocation.Count];
            for (int i = 0; i < (int)InventoryLocation.Count; i++)
            {
                inventoryArray[i] = new List<InventoryItem>();
            }

            inventoryListCapacityArray = new int[(int)InventoryLocation.Count];
            inventoryListCapacityArray[(int)InventoryLocation.Player] = Constant.Inventory.PlayerInventoryMinCapacity;
        }

        private void Start()
        {
            GetData();
        }

        /// <summary>
        /// Set up the size of the inventory item data and create each item in array
        /// </summary>
        /// <param name="size"></param>
        public void Setup()
        {
            inventoryItemList = new List<InventoryItem>(Constant.Inventory.PlayerInventoryMaxCapacity);
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                inventoryItemList[i] = new InventoryItem();
            }
        }

        #region Get item logic
        /// <summary>
        /// Get item data in player's bag based on index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetItemInInventory(int index)
        {
            return inventoryItemList[index];
        }

        // /// <summary>
        // /// Get item data in player's chest based on index and chest Id
        // /// </summary>
        // /// <param name="chestID"></param>
        // /// <param name="index"></param>
        // /// <returns></returns>
        // public InventorySlot GetItemInChest(string chestID, int index)
        // {
        //     return chests[chestID][index];
        // }
        #endregion

        #region Add item logic
        /// <summary>
        /// Logic that add item into player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(InventoryLocation inventoryLocation, Item item, int quantity)
        {
            List<InventoryItem> inventoryItemList = inventoryArray[(int)inventoryLocation];
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(item.ItemID);

            if (itemDetail.IsStackable)
            {
                // item can be stacked
                // find the item in the inventory
                AddStackableItem(itemDetail, quantity);
            }
            else
            {
                // item cannot be stacked
                // find the first empty slot in the inventory
                while (quantity > 0 && !IsInventoryFull())
                {
                    int firstEmptySlot = FindFirstEmptySlot(inventoryItemList);
                    AddItemAtPosition(inventoryItemList, item.ItemID, firstEmptySlot, 1);
                    quantity--;
                }

            }

            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }

        private void AddItemAtPosition(List<InventoryItem> inventoryItemList, string itemId, int itemPosition, int quantity)
        {
            InventoryItem inventoryItem = new InventoryItem(itemId);
            inventoryItem.quantity += quantity;
            inventoryItemList[itemPosition] = inventoryItem;
        }

        /// <summary>
        /// Logic that add statckable into the player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddStackableItem(ItemDetail item, int quantity)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].isEmpty) continue;

                if (inventoryItemList[i].itemId == item.ID)
                {
                    int amountPossibleToTake = item.MaxStackSize - inventoryItemList[i].quantity;

                    inventoryItemList[i].IncrementQuantity(amountPossibleToTake);

                    if (quantity > amountPossibleToTake)
                    {
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItemList[i].IncrementQuantity(amountPossibleToTake);
                        return;
                    }
                }
            }
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                int firstEmptySlot = FindFirstEmptySlot(inventoryItemList);
                AddItemAtPosition(inventoryItemList, item.ID, firstEmptySlot, newQuantity);
            }
        }
        #endregion

        #region Swap item logic
        /// <summary>
        /// Logic that swap 2 items' data in bag
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public void SwapItemsInBag(UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        {
            int itemIdx1 = item1.Index;
            int itemIdx2 = item2.Index;

            InventoryItem item = inventoryItemList[itemIdx1];
            inventoryItemList[itemIdx1] = inventoryItemList[itemIdx2];
            inventoryItemList[itemIdx2] = item;

            InformAboutBagChange();
        }

        // /// <summary>
        // /// Logic that swap 2 items' data in chest
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="item1"></param>
        // /// <param name="item2"></param>
        // public void SwapItemsInChest(string id, UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        // {
        //     var items = chests[id];

        //     int itemIdx1 = item1.Index;
        //     int itemIdx2 = item2.Index;

        //     InventorySlot item = items[itemIdx1];
        //     items[itemIdx1] = items[itemIdx2];
        //     items[itemIdx2] = item;

        //     InformAboutChestChange(id);
        // }

        // /// <summary>
        // /// Logic that swap item's data from chest to bag or vice versa
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="item1"></param>
        // /// <param name="item2"></param>
        // public void SwapItemsInDifLocation(string id, UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        // {
        //     var chestItems = chests[id];

        //     int itemIdx1 = item1.Index;
        //     int itemIdx2 = item2.Index;

        //     InventorySlot item = null;

        //     if (item1.ItemLocation == InventoryLocation.Chest)
        //     {
        //         item = chestItems[itemIdx1];
        //         chestItems[itemIdx1] = inventoryItemList[itemIdx2];
        //         inventoryItemList[itemIdx2] = item;
        //     }
        //     else if (item1.ItemLocation == InventoryLocation.Player)
        //     {
        //         item = inventoryItemList[itemIdx1];
        //         inventoryItemList[itemIdx1] = chestItems[itemIdx2];
        //         chestItems[itemIdx2] = item;
        //     }

        //     InformAboutBagChange();
        //     InformAboutChestChange(id);
        // }
        #endregion

        #region Remove item logic
        // /// <summary>
        // /// Remove quantity's item in the player's bag
        // /// </summary>
        // /// <param name="itemIdx"></param>
        // /// <param name="quantity"></param>
        // public void RemoveBagItem(int itemIdx, int quantity)
        // {
        //     if (inventoryItemList[itemIdx].IsEmpty) return;

        //     int remainder = inventoryItemList[itemIdx].Quantity - quantity;
        //     if (remainder <= 0)
        //     {
        //         inventoryItemList[itemIdx].ChangeItem(null, 0);
        //     }
        //     else
        //     {
        //         inventoryItemList[itemIdx].ChangeQuantity(remainder);
        //     }
        //     InformAboutBagChange();
        // }

        // /// <summary>
        // /// Remove quantity's item in the player's chest
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="itemIdx"></param>
        // /// <param name="quantity"></param>
        // public void RemoveChestItem(string id, int itemIdx, int quantity)
        // {
        //     var items = chests[id];
        //     if (items[itemIdx].IsEmpty) return;

        //     int remainder = items[itemIdx].Quantity - quantity;
        //     if (remainder <= 0)
        //     {
        //         items[itemIdx].ChangeItem(null, 0);
        //     }
        //     else
        //     {
        //         items[itemIdx].ChangeQuantity(remainder);
        //     }
        //     InformAboutChestChange(id);
        // }
        #endregion

        #region Find logic
        private int FindItemInInventory(string itemId)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemId == itemId)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindFirstEmptySlot(List<InventoryItem> inventoryItems)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region Check logic
        /// <summary>
        /// Check if the inventory is full
        /// </summary>
        private bool IsInventoryFull()
        {
            foreach (var item in inventoryItemList)
            {
                if (item.isEmpty) return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Events that trigger when having changes in the bag list data
        /// </summary>
        private void InformAboutBagChange()
        {
            OnBagUpdated?.Invoke(inventoryItemList);
        }

        // /// <summary>
        // /// Events that trigger when having changes in the chest list data
        // /// </summary>
        // private void InformAboutChestChange(string id)
        // {
        //     OnChestUpdated?.Invoke(id, chests[id]);
        // }

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