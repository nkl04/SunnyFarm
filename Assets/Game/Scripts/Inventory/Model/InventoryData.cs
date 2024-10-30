namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryData
    {
        public int InventoryLevel { get; private set; } = 1;
        [HideInInspector] public Dictionary<InventoryKey, InventoryItem[]> inventoryDictionary; // array of inventory list
        [HideInInspector] public Dictionary<InventoryKey, int> inventoryListCapacityArray; // capacity of each inventory list
        private Dictionary<InventoryKey, string> selectedInventoryItem;

        public void SetUpInventoryList()
        {
            inventoryDictionary = new Dictionary<InventoryKey, InventoryItem[]>(); // array of inventory list

            inventoryListCapacityArray = new Dictionary<InventoryKey, int>(); // capacity of each inventory list

            selectedInventoryItem = new Dictionary<InventoryKey, string>();
        }

        public void AddInventory(InventoryKey inventoryKey)
        {
            inventoryListCapacityArray.Add(inventoryKey, Constant.Inventory.PlayerInventoryMinCapacity);

            inventoryDictionary.Add(inventoryKey, new InventoryItem[inventoryListCapacityArray[inventoryKey]]);

            selectedInventoryItem.Add(inventoryKey, "");
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
        private int GetItemPositionInInventory(string itemId, InventoryKey inventoryKey)
        {
            InventoryItem[] inventoryItemList = inventoryDictionary[inventoryKey];
            for (int i = 0; i < inventoryItemList.Length; i++)
            {
                if (inventoryItemList[i].itemID == itemId)
                {
                    return i;
                }
            }
            return -1;
        }


        private int FindFirstEmptySlot(InventoryKey inventoryKey)
        {
            InventoryItem[] inventoryItems = inventoryDictionary[inventoryKey];
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
        public void SetSelectedInventoryItem(InventoryKey inventoryKey, string itemId)
        {
            selectedInventoryItem[inventoryKey] = itemId;
            Debug.Log("Selected item: " + itemId);
        }

        public void ClearSelectedInventoryItem(InventoryKey inventoryKey)
        {
            selectedInventoryItem[inventoryKey] = "";
        }

        public string GetSelectedInventoryItem(InventoryKey inventoryKey)
        {
            return selectedInventoryItem[inventoryKey];
        }

        #endregion

        #region Add item logic
        /// <summary>
        /// Logic that add item into player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(InventoryKey inventoryKey, string itemId, int quantity)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

            if (itemDetail.IsStackable)
            {
                // item can be stacked
                AddStackableItem(inventoryKey, itemDetail, quantity);
            }
            else
            {
                // item cannot be stacked
                // find the first empty slot in the inventory
                while (quantity > 0 && !IsInventoryFull(inventoryKey))
                {
                    int firstEmptySlot = FindFirstEmptySlot(inventoryKey);
                    AddItemAtPosition(inventoryKey, itemId, firstEmptySlot, 1);
                    quantity--;
                }

            }

            EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
        }

        /// <summary>
        /// Logic that add item into position in player's inventory
        /// </summary>
        /// <param name="inventoryItemList"></param>
        /// <param name="itemId"></param>
        /// <param name="itemPosition"></param>
        /// <param name="quantity"></param>
        public void AddItemAtPosition(InventoryKey inventoryKey, string itemId, int itemPosition, int quantity)
        {
            InventoryItem[] inventoryItems = inventoryDictionary[inventoryKey];

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

            EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
        }

        /// <summary>
        /// Logic that add statckable into the player's bag
        /// </summary>
        /// <param name="item"></param>    
        /// <param name="quantity"></param>
        public void AddStackableItem(InventoryKey inventoryKey, ItemDetail item, int quantity)
        {
            InventoryItem[] inventoryItems = inventoryDictionary[inventoryKey];

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
                AddItemAtPosition(inventoryKey, itemId, firstEmptySlot, amountToAdd);

                // Decrease the quantity to be added
                quantity -= amountToAdd;
            }
        }


        #endregion

        #region Swap & Merge item logic

        public void HandleSwapItem(InventoryKey inventoryKey, ref DraggedItemCursor dragItem, int slotIndex)
        {
            InventoryItem inventoryItem = inventoryDictionary[inventoryKey][slotIndex];

            InventoryItem inventoryItemCursor = dragItem.InventoryItem;

            dragItem.InventoryItem = inventoryItem;

            inventoryDictionary[inventoryKey][slotIndex] = inventoryItemCursor;
            // update the inventory data
            EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
        }

        public void HandleMergeItem(InventoryKey inventoryKey, ref DraggedItemCursor dragItem, int slotIndex)
        {
            InventoryItem inventoryItem = inventoryDictionary[inventoryKey][slotIndex];

            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(inventoryItem.itemID);

            if (itemDetail.IsStackable && inventoryItem.itemID == dragItem.InventoryItem.itemID)
            {
                int amountPossibleToTake = itemDetail.MaxStackSize - inventoryItem.quantity;

                int amountToAdd = Mathf.Min(dragItem.InventoryItem.quantity, amountPossibleToTake);

                inventoryItem.IncrementQuantity(amountToAdd);

                dragItem.InventoryItem.SetData(dragItem.InventoryItem.itemID, dragItem.InventoryItem.quantity - amountToAdd);

                if (dragItem.InventoryItem.isEmpty)
                {
                    dragItem.ClearDraggedItem();
                }

                inventoryDictionary[inventoryKey][slotIndex] = inventoryItem;

                EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
            }
        }


        /// <summary>
        /// Handle the split item from inventory slot to cursor
        /// </summary>
        /// <param name="inventoryKey"></param>
        /// <param name="dragItem"></param>
        /// <param name="inventorySlot"></param>
        /// <param name="quantity"></param>
        public void HandleSplitItem(InventoryKey inventoryKey, ref DraggedItemCursor dragItem, int slotIndex, int quantity)
        {
            InventoryItem inventoryItem = inventoryDictionary[inventoryKey][slotIndex];

            if (dragItem.IsEmpty)
            {
                dragItem.InventoryItem.SetData(inventoryItem.itemID, quantity);
            }
            else if (dragItem.InventoryItem.itemID == inventoryItem.itemID)
            {
                dragItem.InventoryItem.IncrementQuantity(quantity);
            }

            inventoryDictionary[inventoryKey][slotIndex].IncrementQuantity(-quantity);
            // update the inventory data
            EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
        }
        #endregion

        #region Remove item logic

        public void RemoveItem(InventoryKey inventoryKey, string itemId, int quantity)
        {
            InventoryItem[] inventoryItemList = inventoryDictionary[inventoryKey];

            EventHandlers.CallOnInventoryUpdated(inventoryKey, inventoryDictionary[inventoryKey]);
        }

        #endregion

        #region Upgrade inventory logic

        public void UpgradeInventoryCapacity(InventoryKey inventoryKey, int newCapacity)
        {
            inventoryListCapacityArray[inventoryKey] = newCapacity;

            InventoryItem[] resizedArray = new InventoryItem[newCapacity];

            Array.Copy(inventoryDictionary[inventoryKey], resizedArray, inventoryDictionary[inventoryKey].Length);

            inventoryDictionary[inventoryKey] = resizedArray;

            EventHandlers.CallOnInventoryCapacityUpdated(inventoryKey, newCapacity);
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

        public bool IsInventoryFull(InventoryKey inventoryKey)
        {
            return IsInventoryFull(inventoryDictionary[inventoryKey]);
        }

        private bool IsInventorySlotFullStackWithItem(string itemId, InventoryItem[] inventoryItems, int slotPosition)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

            int itemStackSize = itemDetail.MaxStackSize;

            return inventoryItems[slotPosition].quantity == itemStackSize;
        }

        public bool IsInventoryFullWithItem(string itemId, InventoryKey inventoryKey)
        {
            InventoryItem[] inventoryItems = inventoryDictionary[inventoryKey];

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].isEmpty) continue;

                if (inventoryItems[i].itemID == itemId)
                {
                    if (IsInventorySlotFullStackWithItem(itemId, inventoryItems, i)) continue;

                    return false;
                }
            }

            return IsInventoryFull(inventoryItems);
        }

        #endregion

    }
}