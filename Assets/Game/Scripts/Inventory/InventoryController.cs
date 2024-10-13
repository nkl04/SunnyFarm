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
        public InventoryData InventoryData => inventoryData;
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

        InventoryData inventoryData;

        private void OnEnable()
        {
            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
            EventHandlers.OnToggleInventory += ToggleInventoryView;
            EventHandlers.OnQuickSelectSlot += QuickSelectSlot;
            EventHandlers.OnLeftPointerClick += OnPointerClickInventorySlot;


        }



        private void OnDisable()
        {
            EventHandlers.OnInventoryUpdated -= UpdateUIInventory;
        }

        private void Start()
        {
            // assign data controller
            inventoryData = new InventoryData();

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;

            EventHandlers.OnInventoryCapacityUpdated += UpdateUIInventoryCapacity;

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

            inventoryData.UpgradeInventoryCapacity(InventoryLocation.Player, capacity);
        }

        /// <summary>
        /// Set up for the view
        /// </summary>
        private void SetupView()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];

            uiBagView.InitializeInventoryUI(capacity);

            uiToolBarView.SetupUIInventorySlot();
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

        private void UpdateUIInventoryCapacity(InventoryLocation location, int capacity)
        {
            uiBagView.UpdateUIBagCapacity(location, capacity);
        }

        private void OnPointerClickInventorySlot(UIInventorySlot slot)
        {
            SetSelectInventorySlot(slot);
        }

        private void QuickSelectSlot(int slotIndex)
        {
            UIInventorySlot slot = uiToolBarView.GetInventorySlot(slotIndex);
            SetSelectInventorySlot(slot);
        }


        private void SetSelectInventorySlot(UIInventorySlot slot)
        {
            if (slot.isSelected)
            {
                // if slot is already selected
                uiToolBarView.DeselectAllInventorySlot();
                inventoryData.ResetSelectedInventoryItem(slot.inventoryLocation);
            }
            else
            {
                //if slot is not selected

                uiToolBarView.DeselectAllInventorySlot();
                inventoryData.ResetSelectedInventoryItem(slot.inventoryLocation);
                //select the slot
                slot.Select();
                // set the selected item data
                inventoryData.SetSelectedInventoryItem(slot.inventoryLocation, slot.itemID);
            }
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