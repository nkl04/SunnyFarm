namespace SunnyFarm.Game.Inventory
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using SunnyFarm.Game.Managers;
    using SunnyFarm.Game.Managers.GameInput;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryManager : Singleton<InventoryManager>
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

        [Header("Initial Invenory Data")]
        [SerializeField] private ItemDetail[] initialInventoryItems;

        [Header("UI Inventory")]

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

            EventHandlers.OnInventoryUpdated += UpdateUIInventory;
            EventHandlers.OnInventoryCapacityUpdated += UpdateUIInventoryCapacity;

            inventoryData = new InventoryData();

            SetupModel();
        }

        #region Setup 
        private void SetupModel()
        {
            inventoryData.SetUpInventoryList();
        }
        #endregion

        public void AddInventory(InventoryKey inventoryKey)
        {
            inventoryData.AddInventoryData(inventoryKey);

            uiBagView.SetupUIInventorySlot(inventoryKey);

            uiToolBarView.SetupUIInventorySlot(inventoryKey);

            uiBagView.UpdateUIBagCapacity(inventoryKey, evolveInventoryMap[1]);

            foreach (ItemDetail itemDetail in initialInventoryItems)
            {
                inventoryData.AddItem(inventoryKey, itemDetail.ID, 1);
            }
        }

        public void ToggleInventoryView()
        {
            if (uiBagView.gameObject.activeSelf)
            {
                uiBagView.Hide();

                draggedItemCursor.Hide();

                uiToolBarView.Show();

                selectedItemCursor.Show();

                GameInputManager.Instance.CanPlayerActionInput = true;
            }
            else
            {
                uiBagView.Show();

                draggedItemCursor.Show();

                uiToolBarView.Hide();

                selectedItemCursor.Hide();

                GameInputManager.Instance.CanPlayerActionInput = false;
            }
        }

        private void UpdateUIInventory(InventoryKey inventoryKey, InventoryItem[] inventoryItems)
        {
            uiBagView.UpdateUIBag(inventoryKey, inventoryItems);

            uiToolBarView.UpdateUIToolBar(inventoryKey, inventoryItems);
        }

        private void UpdateUIInventoryCapacity(InventoryKey inventoryKey, int capacity)
        {
            uiBagView.UpdateUIBagCapacity(inventoryKey, capacity);
        }

        private void OnLeftPointerClickInventorySlot(UIInventorySlot slot)
        {
            if (slot.slotLocation == InventorySlotLocation.ToolBar)
            {
                SelectSlot(slot);
            }
            else if (slot.slotLocation == InventorySlotLocation.Container)
            {
                if (draggedItemCursor.InventoryItem.itemID != slot.itemID || draggedItemCursor.IsEmpty)
                {
                    InventoryData.HandleSwapItem(slot.inventoryKey, ref draggedItemCursor, slot.slotIndex);
                }
                else if (draggedItemCursor.InventoryItem.itemID == slot.itemID && !draggedItemCursor.IsEmpty)
                {
                    InventoryData.HandleMergeItem(slot.inventoryKey, ref draggedItemCursor, slot.slotIndex);
                }

                draggedItemCursor.UpdateDraggedItemVisual();

                // check can not turn off the inventory if in dragging item 
                GameInputManager.Instance.CanToggleInventory = draggedItemCursor.IsEmpty;

            }
        }

        private void OnRightPointerClickInventorySlot(UIInventorySlot slot)
        {
            if (slot.slotLocation == InventorySlotLocation.Container)
            {
                if (draggedItemCursor.IsEmpty || draggedItemCursor.InventoryItem.itemID == slot.itemID)
                {
                    InventoryData.HandleSplitItem(slot.inventoryKey, ref draggedItemCursor, slot.slotIndex, 1);
                }
                else if (slot.IsEmpty && !draggedItemCursor.IsEmpty)
                {
                    InventoryData.AddItemAtPosition(slot.inventoryKey, draggedItemCursor.InventoryItem.itemID, slot.slotIndex, 1);

                    draggedItemCursor.InventoryItem.IncrementQuantity(-1);
                }

                draggedItemCursor.UpdateDraggedItemVisual();

                // check can not turn off the inventory if in dragging item 
                GameInputManager.Instance.CanToggleInventory = draggedItemCursor.IsEmpty;
            }
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
                uiToolBarView.ClearHighlightOnInventorySlots(slot.inventoryKey);
                // clear the selected slot in the bag
                uiBagView.ClearHighlightOnInventorySlots(slot.inventoryKey);
                // select the slot in the toolbar
                uiToolBarView.SetHighlightSelectInventorySlot(slot.slotIndex, slot.inventoryKey);
                // select the slot in the bag
                uiBagView.SetHighlightSelectInventorySlot(slot.slotIndex, slot.inventoryKey);
                // set the selected item
                InventoryData.SetSelectedInventoryItem(slot.inventoryKey, slot.itemID);

                // set the selected item to the cursor
                ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(slot.itemID);

                if (itemDetail != null && itemDetail.CanBeCarried)
                {
                    selectedItemCursor.SetData(slot.itemID, slot.itemQuantity);
                }
                else
                {
                    selectedItemCursor.ClearData();
                }
            }
        }
    }
}