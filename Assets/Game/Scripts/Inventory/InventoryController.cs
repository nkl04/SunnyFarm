namespace SunnyFarm.Game.Inventory
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Entities.Player;
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

        private bool IsInventoryOpen => uiBagView.gameObject.activeSelf;

        protected override void Awake()
        {
            base.Awake();

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
            EventHandlers.OnToggleInventory += ToggleInventoryView;

            EventHandlers.OnLeftPointerClick += OnLeftPointerClickInventorySlot;
            EventHandlers.OnRightPointerClick += OnRightPointerClickInventorySlot;

            EventHandlers.OnQuickSelectSlot += QuickSelectSlot;
            EventHandlers.OnMouseScroll += OnMouseScrollSelectSlotInput;

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
                SelectSlot(slot);
            }
            else if (slot.slotLocation == InventorySlotLocation.Container)
            {
                InventoryData.HandleSwapItem(InventoryLocation.Player, ref draggedItemCursor, slot);
                draggedItemCursor.UpdateDraggedItemVisual();

                // check can not turn off the inventory if in dragging item 
                Player.Instance.CanToggleInventory = draggedItemCursor.IsEmpty;
            }
        }

        private void OnRightPointerClickInventorySlot(UIInventorySlot slot)
        {
        }


        private void QuickSelectSlot(int slotIndex)
        {
            UIInventorySlot slot = uiToolBarView.GetInventorySlot(slotIndex);

            SelectSlot(slot);
        }

        private void OnMouseScrollSelectSlotInput(float scrollInput)
        {
            if (!IsInventoryOpen)
            {
                UIInventorySlot slot = uiToolBarView.GetSelectedInventorySlot();

                if (slot != null)
                {
                    if (scrollInput > 0)
                    {
                        int currentSlotIndex = slot.slotIndex;

                        UIInventorySlot nextSlot = uiToolBarView.GetTheNextInventorySlotHasItem(currentSlotIndex);

                        SelectSlot(nextSlot);
                    }
                    else
                    {
                        int currentSlotIndex = slot.slotIndex;

                        UIInventorySlot prevSlot = uiToolBarView.GetThePreviousInventorySlotHasItem(currentSlotIndex);

                        SelectSlot(prevSlot);
                    }
                }
                else
                {
                    if (scrollInput > 0)
                    {
                        SelectSlot(uiToolBarView.GetInventorySlotByIndex(0));
                    }
                    else
                    {
                        SelectSlot(uiToolBarView.GetInventorySlotByIndex(Constant.Inventory.PlayerInventoryMinCapacity - 1));
                    }
                }
            }
        }

        public void SelectSlot(UIInventorySlot slot)
        {
            if (slot != null)
            {
                // clear the selected slot in the toolbar
                uiToolBarView.ClearHighlightOnInventorySlots();
                // clear the selected slot in the bag
                uiBagView.ClearHighlightOnInventorySlots();
                // select the slot in the toolbar
                uiToolBarView.SetHighlightSelectInventorySlot(slot.slotIndex);
                // select the slot in the bag
                uiBagView.SetHighlightSelectInventorySlot(slot.slotIndex);
                // set the selected item
                InventoryData.SetSelectedInventoryItem(InventoryLocation.Player, slot.itemID);
                // set the selected item to the cursor
                selectedItemCursor.SetData(slot.itemID, slot.itemQuantity);
            }
        }

        private void UpdateSelectedInventoryItem()
        {

        }

    }
}