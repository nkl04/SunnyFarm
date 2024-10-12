namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryDataController : Singleton<InventoryDataController>
    {
        public event Action<InventoryItem[]> OnBagUpdated;
        public event Action<string, InventoryItem[]> OnChestUpdated;

        public int InventoryLevel { get; private set; } = 1;
        [HideInInspector] public InventoryItem[][] inventoryArray; // array of inventory list
        [HideInInspector] public int[] inventoryListCapacityArray; // capacity of each inventory list

        // private Dictionary<string, InventorySlot[]> chests;

        public void CreateInventoryList()
        {
            inventoryArray = new InventoryItem[(int)InventoryLocation.Count][];

            inventoryListCapacityArray = new int[(int)InventoryLocation.Count];

            inventoryListCapacityArray[(int)InventoryLocation.Player] = Constant.Inventory.PlayerInventoryMinCapacity;

            for (int i = 0; i < (int)InventoryLocation.Count; i++)
            {
                inventoryArray[i] = new InventoryItem[inventoryListCapacityArray[i]];
            }

        }

        private void Start()
        {
            GetData();
        }

        // /// <summary>
        // /// Set up the size of the inventory item data and create each item in array
        // /// </summary>
        // /// <param name="size"></param>
        // public void Setup()
        // {
        //     inventoryItemList = new InventoryItem[](Constant.Inventory.PlayerInventoryMaxCapacity);
        //     for (int i = 0; i < inventoryItemList.Count; i++)
        //     {
        //         inventoryItemList[i] = new InventoryItem();
        //     }
        // }

        #region Get item logic
        /// <summary>
        /// Get item data in player's bag based on index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetItemInInventory(int index, InventoryItem[] inventoryItemList)
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
            InventoryItem[] inventoryItemList = inventoryArray[(int)inventoryLocation];
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(item.ItemID);

            if (itemDetail.IsStackable)
            {
                // item can be stacked
                AddStackableItem(inventoryItemList, itemDetail, quantity);
            }
            else
            {
                // item cannot be stacked
                // find the first empty slot in the inventory
                while (quantity > 0 && !IsInventoryFull(inventoryItemList))
                {
                    int firstEmptySlot = FindFirstEmptySlot(inventoryItemList);
                    AddItemAtPosition(inventoryItemList, item.ItemID, firstEmptySlot, 1);
                    quantity--;
                }

            }

            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }

        private void AddItemAtPosition(InventoryItem[] inventoryItemList, string itemId, int itemPosition, int quantity)
        {

            InventoryItem inventoryItemInSlot = GetItemInInventory(itemPosition, inventoryItemList);
            if (inventoryItemInSlot.isEmpty)
            {
                InventoryItem inventoryItem = new(itemId);
                inventoryItem.IncrementQuantity(quantity);
                inventoryItemList[itemPosition].SetData(itemId, inventoryItem.quantity);
            }
            else
            {
                inventoryItemInSlot.IncrementQuantity(quantity);
            }
        }

        /// <summary>
        /// Logic that add statckable into the player's bag
        /// </summary>
        /// <param name="item"></param>    
        /// <param name="quantity"></param>
        public void AddStackableItem(InventoryItem[] inventoryItems, ItemDetail item, int quantity)
        {
            string itemId = item.ID;

            // Check if the item is already in the inventory
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].isEmpty) continue;

                // Check if item already exists in inventory
                if (inventoryItems[i].itemId == itemId)
                {
                    int inventoryItemPos = i;

                    if (IsInventorySlotFullStackWithItem(itemId, inventoryItems, inventoryItemPos)) continue;

                    // Calculate how much we can add to this stack
                    int amountPossibleToTake = item.MaxStackSize - inventoryItems[i].quantity;

                    // If we can take the whole quantity or a partial amount
                    int amountToAdd = Mathf.Min(quantity, amountPossibleToTake);
                    // Increment quantity in stack and decrease the amount we need to add
                    inventoryItems[i].IncrementQuantity(amountToAdd);
                    // Debug.Log(inventoryItem.quantity);
                    quantity -= amountToAdd;

                    // If all the quantity is added, exit early
                    if (quantity == 0)
                    {
                        return;
                    }
                }
            }

            // If item doesn't exist or we need to add new stacks
            while (quantity > 0 && !IsInventoryFull(inventoryItems))
            {
                // Find the first empty slot
                int firstEmptySlot = FindFirstEmptySlot(inventoryItems);
                // Add a new stack of the item
                int amountToAdd = Mathf.Min(quantity, item.MaxStackSize);
                AddItemAtPosition(inventoryItems, itemId, firstEmptySlot, amountToAdd);

                // Decrease the quantity to be added
                quantity -= amountToAdd;
            }
        }

        #endregion

        #region Swap item logic
        // /// <summary>
        // /// Logic that swap 2 items' data in bag
        // /// </summary>
        // /// <param name="item1"></param>
        // /// <param name="item2"></param>
        // public void SwapItemsInBag(UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        // {
        //     int itemIdx1 = item1.Index;
        //     int itemIdx2 = item2.Index;

        //     InventoryItem item = inventoryItemList[itemIdx1];
        //     inventoryItemList[itemIdx1] = inventoryItemList[itemIdx2];
        //     inventoryItemList[itemIdx2] = item;

        //     InformAboutBagChange();
        // }

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
        /// <summary>
        /// Find the position of the item in the inventory
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>-1 if not found</returns>
        private int FindItemPositionInInventory(string itemId, InventoryLocation inventoryLocation)
        {
            InventoryItem[] inventoryItemList = inventoryArray[(int)inventoryLocation];
            for (int i = 0; i < inventoryItemList.Length; i++)
            {
                if (inventoryItemList[i].itemId == itemId)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindFirstEmptySlot(InventoryItem[] inventoryItems)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
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
        private bool IsInventoryFull(InventoryItem[] inventoryItemList)
        {
            foreach (var item in inventoryItemList)
            {
                if (item.isEmpty) return false;
            }
            return true;
        }

        private bool IsInventorySlotFullStackWithItem(string itemId, InventoryItem[] inventoryItems, int slotPosition)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

            int itemStackSize = itemDetail.MaxStackSize;

            return inventoryItems[slotPosition].quantity == itemStackSize;
        }
        #endregion

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