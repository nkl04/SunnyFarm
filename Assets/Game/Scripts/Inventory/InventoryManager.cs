namespace SunnyFarm.Game.Inventory
{
    using DG.Tweening;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using SunnyFarm.Game.Managers;
    using SunnyFarm.Game.Managers.GameInput;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryManager : Singleton<InventoryManager>
    {
        public InventoryData InventoryData => inventoryData;

        public SelectedItemCursor SelectedItemCursor => selectedItemCursor;

        public DraggedItemCursor DraggedItemCursor => draggedItemCursor;

        public bool CanChangeSelectedInventorySlot { get; set; } = true;
        // Define map for capacity of the inventory based on inventory's level
        // key: level, value: capacity
        private Dictionary<int, int> evolveInventoryMap = new Dictionary<int, int>()
        {
            {1, 12},
            {2, 24},
            {3, 36},
        };

        [SerializeField] private GameObject itemWorldPrefab;

        [Header("Initial Invenory Data")]
        [SerializeField] private ConfigItem[] initialInventoryItems;

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

            draggedItemCursor.InventoryKey = inventoryKey;

            selectedItemCursor.InventoryKey = inventoryKey;

            foreach (ConfigItem itemDetail in initialInventoryItems)
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

                GameInputManager.Instance.CanPlayerKeyBoardInput = true;
            }
            else
            {
                uiBagView.Show();

                draggedItemCursor.Show();

                uiToolBarView.Hide();

                selectedItemCursor.Hide();

                GameInputManager.Instance.CanPlayerKeyBoardInput = false;
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

        private void OnLeftPointerClickInventorySlot(IPointerClickHandler _object)
        {
            if (_object is UIInventorySlot)
            {
                UIInventorySlot slot = _object as UIInventorySlot;

                if (slot.slotLocation == InventorySlotLocation.ToolBar)
                {
                    if (!CanChangeSelectedInventorySlot) return;

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
                }
            }

            else

            if (_object is DropToWorldArea)
            {
                // click to droptoworld area to drop the item to the world

                if (draggedItemCursor.IsEmpty) return;

                ConfigItem itemDetail = ItemSystemManager.Instance.GetItemDetail(draggedItemCursor.InventoryItem.itemID);

                if (!itemDetail.CanBeDropped) return;

                InventoryItem item = draggedItemCursor.InventoryItem;

                draggedItemCursor.ClearDraggedItem();

                Player player = GameManager.Instance.GetPlayer(draggedItemCursor.InventoryKey);

                Vector2 playerDirection = player.GetPlayerDirection();

                Vector3 playerPosition = player.transform.position;

                Vector3 dropPosition = playerPosition + new Vector3(playerDirection.x, playerDirection.y, 0) * 2.5f;

                GameObject itemWorld = Instantiate(itemWorldPrefab, playerPosition, Quaternion.identity);

                itemWorld.GetComponent<Item>().SetUp(item.itemID, item.quantity);

                itemWorld.GetComponent<BoxCollider2D>().enabled = false;

                // drop the item to the world 
                float dropHeight = 0.5f;
                float duration = 0.5f;

                Vector3 targetPosition = dropPosition + new Vector3(0, dropHeight, 0);
                itemWorld.transform.DOJump(targetPosition, dropHeight, 1, duration)
                    .OnComplete(() =>
                    {
                        itemWorld.transform.DOMoveY(targetPosition.y - 0.1f, 0.2f)
                         .SetEase(Ease.OutBounce)
                         .OnComplete(() =>
                         {
                             Debug.Log("Dropped item " + item.itemID + " to the world");
                             itemWorld.GetComponent<BoxCollider2D>().enabled = true;
                         });

                    });

                itemWorld.transform.DOScale(Vector3.one * 1.2f, duration / 2)
                    .SetLoops(2, LoopType.Yoyo);

            }

            // check can not turn off the inventory if in dragging item 
            GameInputManager.Instance.CanToggleInventory = draggedItemCursor.IsEmpty;
        }

        private void OnRightPointerClickInventorySlot(IPointerClickHandler _object)
        {
            if (_object is not UIInventorySlot) return;

            UIInventorySlot slot = _object as UIInventorySlot;

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
            if (!CanChangeSelectedInventorySlot) return;

            UIInventorySlot slot = uiToolBarView.GetInventorySlot(slotIndex);

            SelectSlot(slot);
        }

        private void OnMouseScrollSelectSlotInput(float scrollInput)
        {
            if (!CanChangeSelectedInventorySlot) return;

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
                ConfigItem itemDetail = ItemSystemManager.Instance.GetItemDetail(slot.itemID);

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