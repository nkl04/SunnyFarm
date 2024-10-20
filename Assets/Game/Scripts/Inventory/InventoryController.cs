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
        public SelectedItemCursor SelectedItemCursor => selectedItemCursor;
        public DraggedItemCursor DraggedItemCursor => draggedItemCursor;
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

        [SerializeField] private SelectedItemCursor selectedItemCursor;

        [SerializeField] private DraggedItemCursor draggedItemCursor;

        InventoryData inventoryData;

        protected override void Awake()
        {
            base.Awake();

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
            EventHandlers.OnToggleInventory += ToggleInventoryView;
            EventHandlers.OnQuickSelectSlot += QuickSelectSlot;
            EventHandlers.OnLeftPointerClick += OnLeftPointerClickInventorySlot;
            EventHandlers.OnRightPointerClick += OnRightPointerClickInventorySlot;
        }

        private void Start()
        {
            inventoryData = new InventoryData();

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;

            EventHandlers.OnInventoryCapacityUpdated += UpdateUIInventoryCapacity;

            SetupView();

            SetupModel();
        }

        #region Setup 
        private void SetupModel()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];

            inventoryData.CreateInventoryList();

            inventoryData.UpgradeInventoryCapacity(InventoryLocation.Player, capacity);
        }
        private void SetupView()
        {
            uiBagView.SetupUIInventorySlot();

            uiToolBarView.SetupUIInventorySlot();
        }

        #endregion

        public void ToggleInventoryView()
        {
            if (uiBagView.gameObject.activeSelf)
            {
                uiBagView.Hide();

                draggedItemCursor.Hide();

                uiToolBarView.Show();

                selectedItemCursor.Show();
            }
            else
            {
                uiBagView.Show();

                draggedItemCursor.Show();

                uiToolBarView.Hide();

                selectedItemCursor.Hide();
            }
        }

        private void UpdateUIInventory(InventoryLocation inventoryLocation, InventoryItem[] inventoryItems)
        {
            uiBagView.UpdateUIBag(inventoryLocation, inventoryItems);

            uiToolBarView.UpdateUIToolBar(inventoryLocation, inventoryItems);
        }

        private void UpdateUIInventoryCapacity(InventoryLocation location, int capacity)
        {
            uiBagView.UpdateUIBagCapacity(location, capacity);
        }

        private void OnLeftPointerClickInventorySlot(UIInventorySlot slot)
        {

            if (slot.slotLocation == InventorySlotLocation.ToolBar)
            {
                uiToolBarView.ClearHighlightOnInventorySlots();

                uiBagView.ClearHighlightOnInventorySlots();

                uiToolBarView.SetHighlightSelectInventorySlot(slot.slotIndex);

                uiBagView.SetHighlightSelectInventorySlot(slot.slotIndex);

                selectedItemCursor.SetData(slot.itemID, slot.itemQuantity);
            }
            else if (slot.slotLocation == InventorySlotLocation.Container)
            {
                if (draggedItemCursor.InventoryItem.itemID == slot.itemID)
                {
                    ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(slot.itemID);

                    if (itemDetail.IsStackable)
                    {
                        int remainingSpace = itemDetail.MaxStackSize - slot.itemQuantity;

                        int quantityToAdd = Mathf.Min(remainingSpace, draggedItemCursor.InventoryItem.quantity);

                        InventoryData.AddItemAtPosition(slot.inventoryLocation, slot.itemID, slot.slotIndex, quantityToAdd);
                    }
                    else
                    {
                        InventoryData.HandleSwapItem(slot.inventoryLocation, ref draggedItemCursor, slot);
                    }
                }
                else
                {
                    InventoryData.HandleSwapItem(slot.inventoryLocation, ref draggedItemCursor, slot);
                }
            }
        }

        private void OnRightPointerClickInventorySlot(UIInventorySlot slot)
        {
            throw new NotImplementedException();
        }


        private void QuickSelectSlot(int slotIndex)
        {
            UIInventorySlot slot = uiToolBarView.GetInventorySlot(slotIndex);
        }

        private void SetSelectedInventorySlot(UIInventorySlot slot)
        {

        }

    }
}