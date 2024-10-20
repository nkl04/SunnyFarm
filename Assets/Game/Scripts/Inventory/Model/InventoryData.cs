namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Inventory.UI;
    using SunnyFarm.Game.Managers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryData
    {
        public int InventoryLevel { get; private set; } = 1;
        [HideInInspector] public InventoryItem[][] inventoryArray; // array of inventory list
        [HideInInspector] public int[] inventoryListCapacityArray; // capacity of each inventory list
        private string[] selectedInventoryItem;

        public void CreateInventoryList()
        {
            inventoryArray = new InventoryItem[(int)InventoryLocation.Count][];

            inventoryListCapacityArray = new int[(int)InventoryLocation.Count];

            inventoryListCapacityArray[(int)InventoryLocation.Player] = Constant.Inventory.PlayerInventoryMinCapacity;

            // Initialize the inventory list
            for (int i = 0; i < (int)InventoryLocation.Count; i++)
            {
                inventoryArray[i] = new InventoryItem[inventoryListCapacityArray[i]];
            }

            // Initialize the selected inventory item
            selectedInventoryItem = new string[(int)InventoryLocation.Count];

            for (int i = 0; i < selectedInventoryItem.Length; i++)
            {
                selectedInventoryItem[i] = "";
            }

        }

        #region Get logic
        /// <summary>
        /// Get item data in player's bag based on index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetItemInInventory(int index, InventoryItem[] inventoryItemList)
        {
            return inventoryItemList[index];
        }

        /// <summary>
        /// Find the position of the item in the inventory
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>-1 if not found</returns>
        private int GetItemPositionInInventory(string itemId, InventoryLocation inventoryLocation)
        {
            InventoryItem[] inventoryItemList = inventoryArray[(int)inventoryLocation];
            for (int i = 0; i < inventoryItemList.Length; i++)
            {
                if (inventoryItemList[i].itemID == itemId)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindFirstEmptySlot(InventoryLocation inventoryLocation)
        {
            InventoryItem[] inventoryItems = inventoryArray[(int)inventoryLocation];
            return FindFirstEmptySlot(inventoryItems);
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

        #region Select inventory item logic
        public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, string itemId)
        {
            selectedInventoryItem[(int)inventoryLocation] = itemId;
        }

        public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
        {
            selectedInventoryItem[(int)inventoryLocation] = "";
        }

        public string GetSelectedInventoryItem(InventoryLocation inventoryLocation)
        {
            return selectedInventoryItem[(int)inventoryLocation];
        }

        #endregion

        #region Add item logic
        /// <summary>
        /// Logic that add item into player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(InventoryLocation inventoryLocation, Item item, int quantity)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(item.ItemID);

            if (itemDetail.IsStackable)
            {
                // item can be stacked
                AddStackableItem(inventoryLocation, itemDetail, quantity);
            }
            else
            {
                // item cannot be stacked
                // find the first empty slot in the inventory
                while (quantity > 0 && !IsInventoryFull(inventoryLocation))
                {
                    int firstEmptySlot = FindFirstEmptySlot(inventoryLocation);
                    AddItemAtPosition(inventoryLocation, item.ItemID, firstEmptySlot, 1);
                    quantity--;
                }

            }

            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }

        /// <summary>
        /// Logic that add item into position in player's inventory
        /// </summary>
        /// <param name="inventoryItemList"></param>
        /// <param name="itemId"></param>
        /// <param name="itemPosition"></param>
        /// <param name="quantity"></param>
        public void AddItemAtPosition(InventoryLocation inventoryLocation, string itemId, int itemPosition, int quantity)
        {
            InventoryItem[] inventoryItems = inventoryArray[(int)inventoryLocation];

            InventoryItem inventoryItemInSlot = GetItemInInventory(itemPosition, inventoryItems);
            if (inventoryItemInSlot.isEmpty)
            {
                InventoryItem inventoryItem = new(itemId);
                inventoryItem.IncrementQuantity(quantity);
                inventoryItems[itemPosition].SetData(itemId, inventoryItem.quantity);
            }
            else
            {
                inventoryItemInSlot.IncrementQuantity(quantity);
            }

            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }

        /// <summary>
        /// Logic that add statckable into the player's bag
        /// </summary>
        /// <param name="item"></param>    
        /// <param name="quantity"></param>
        public void AddStackableItem(InventoryLocation inventoryLocation, ItemDetail item, int quantity)
        {
            InventoryItem[] inventoryItems = inventoryArray[(int)inventoryLocation];

            string itemId = item.ID;

            // Check if the item is already in the inventory
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].isEmpty) continue;

                // Check if item already exists in inventory
                if (inventoryItems[i].itemID == itemId)
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
                AddItemAtPosition(inventoryLocation, itemId, firstEmptySlot, amountToAdd);

                // Decrease the quantity to be added
                quantity -= amountToAdd;
            }
        }


        #endregion

        #region Swap item logic

        public void HandleSwapItem(InventoryLocation inventoryLocation, ref DraggedItemCursor dragItem, UIInventorySlot inventorySlot)
        {
            InventoryItem inventoryItem = inventoryArray[(int)inventoryLocation][inventorySlot.slotIndex];

            InventoryItem inventoryItemCursor = dragItem.InventoryItem;

            dragItem.InventoryItem = inventoryItem;
            inventoryArray[(int)inventoryLocation][inventorySlot.slotIndex] = inventoryItemCursor;

            // update the inventory data
            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }
        #endregion

        #region Remove item logic

        public void RemoveItem(InventoryLocation inventoryLocation, string itemId, int quantity)
        {
            InventoryItem[] inventoryItemList = inventoryArray[(int)inventoryLocation];

            EventHandlers.CallOnInventoryUpdated(inventoryLocation, inventoryArray[(int)inventoryLocation]);
        }

        #endregion

        #region Upgrade inventory logic

        public void UpgradeInventoryCapacity(InventoryLocation inventoryLocation, int newCapacity)
        {
            inventoryListCapacityArray[(int)inventoryLocation] = newCapacity;

            Array.Resize(ref inventoryArray[(int)inventoryLocation], newCapacity);

            EventHandlers.CallOnInventoryCapacityUpdated(inventoryLocation, newCapacity);
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

        public bool IsInventoryFull(InventoryLocation inventoryLocation)
        {
            return IsInventoryFull(inventoryArray[(int)inventoryLocation]);
        }

        private bool IsInventorySlotFullStackWithItem(string itemId, InventoryItem[] inventoryItems, int slotPosition)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

            int itemStackSize = itemDetail.MaxStackSize;

            return inventoryItems[slotPosition].quantity == itemStackSize;
        }

        #endregion

    }
}