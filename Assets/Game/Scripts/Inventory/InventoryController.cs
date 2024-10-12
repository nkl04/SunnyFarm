namespace SunnyFarm.Game.Inventory
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Input;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using SunnyFarm.Game.Managers;
    using SunnyFarm.Game.Managers.GameInput;
    using System;
    using System.Collections.Generic;
    using UnityEditor.UIElements;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryController : Singleton<InventoryController>
    {
        // Define map for capacity of the inventory based on inventory's level
        // key: level, value: capacity
        private Dictionary<int, int> evolveInventoryMap = new Dictionary<int, int>()
        {
            {1, 12},
            {2, 24},
            {3, 36},
        };

        [SerializeField] private UIBagView uiBagView;

        [SerializeField] private UIChestView uiChestView;

        [SerializeField] private UIToolBar uiToolBarView;

        private InventoryDataController inventoryData;

        private void OnEnable()
        {
            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
        }

        private void OnDisable()
        {
            EventHandlers.OnInventoryUpdated -= UpdateUIInventory;
        }

        private void Start()
        {
            // assign data controller
            inventoryData = InventoryDataController.Instance;
            SetupView();
            SetupModel();
        }

        /// <summary>
        /// Set up for the model
        /// </summary>
        private void SetupModel()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];
            inventoryData.CreateInventoryList();
            // inventoryData.Setup();

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
            // inventoryData.OnChestUpdated += UpdateChestUIItems;

            // foreach (InventoryItem item in initialItems)
            // {
            //     if (item.IsEmpty) continue;
            //     inventoryData.AddItem(item.itemId, item.Quantity);
            // }
        }

        /// <summary>
        /// Set up for the view
        /// </summary>
        private void SetupView()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];
            uiBagView.InitializeInventoryUI(capacity);

            // // Regist events for bag view
            // uiBagView.OnSwapItems += HandleSwapItemsInBagView;
            // uiBagView.OnDescriptionRequested += HandleDescriptionRequested;

            // // Regist events for chest view
            // uiChestView.InitializeInventoryUI(30); // test
            // uiChestView.OnSwapItems += HandleSwapItemsInChestView;
            // uiChestView.OnDescriptionRequested += HandleDescriptionRequested;
        }

        public void ToggleInventoryView()
        {
            if (uiBagView.gameObject.activeSelf)
            {
                uiBagView.Hide();
                uiToolBarView.Show();
            }
            else
            {
                uiBagView.Show();
                uiToolBarView.Hide();
            }
        }

        #region Handle events
        // /// <summary>
        // /// Hande logic of swap item in bag view
        // /// </summary>
        // /// <param name="item1"></param>
        // /// <param name="item2"></param>
        // private void HandleSwapItemsInBagView(UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        // {
        //     if (item1.CompareLocation(item2))
        //         inventoryData.SwapItemsInBag(item1, item2);
        //     // else
        //     //     inventoryData.SwapItemsInDifLocation(uiChestView.ID, item1, item2);
        // }
        // /// <summary>
        // /// Hande logic of swap item in chest view
        // /// </summary>
        // /// <param name="chestID"></param>
        // /// <param name="item1"></param>
        // /// <param name="item2"></param>
        // private void HandleSwapItemsInChestView(string chestID, UIInventoryItemKeyData item1,
        //     UIInventoryItemKeyData item2)
        // {
        //     if (item1.CompareLocation(item2))
        //         inventoryData.SwapItemsInChest(chestID, item1, item2);
        //     else
        //         inventoryData.SwapItemsInDifLocation(chestID, item1, item2);
        // }
        // /// <summary>
        // /// Handle the requested description event
        // /// </summary>
        // private void HandleDescriptionRequested(UIInventoryItemKeyData itemData)
        // {
        //     InventorySlot item = null;
        //     if (itemData.ItemLocation == InventoryLocation.Player)
        //     {
        //         item = inventoryData.GetItemInBag(itemData.Index);
        //         if (item.IsEmpty) return;

        //         uiBagView.UpdateItemDescription(itemData.Index, item.Item.Name,
        //             item.Item.ItemType, item.Item.Description);
        //     }
        //     else
        //     {
        //         item = inventoryData.GetItemInChest(uiChestView.ID, itemData.Index);
        //         if (item.IsEmpty) return;

        //         uiChestView.UpdateItemDescription(itemData.Index, item.Item.Name,
        //             item.Item.ItemType, item.Item.Description);
        //     }
        // }
        #endregion

        /// <summary>
        /// Update UI inventory
        /// </summary>
        /// <param name="inventoryLocation"></param>
        /// <param name="inventoryItems"></param>
        private void UpdateUIInventory(InventoryLocation inventoryLocation, InventoryItem[] inventoryItems)
        {
            uiBagView.UpdateUIBag(inventoryLocation, inventoryItems);
            uiToolBarView.UpdateUIToolBar(inventoryLocation, inventoryItems);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inventoryItems"></param>
        private void UpdateChestUIItems(string id, InventoryItem[] inventoryItems)
        {
            // uiChestView.ResetAllUIItems();
            // for (int i = 0; i < inventoryItems.Length; i++)
            // {
            //     uiChestView.UpdateUIItemData(i, inventoryItems[i].itemId?.ItemImage ?? null,
            //         inventoryItems[i].Quantity);
            // }
        }
    }
}