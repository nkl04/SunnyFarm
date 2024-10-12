namespace SunnyFarm.Game.Inventory.UI
{
    using System;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIBagView : UILargeInventoryView
    {
        public UIToolBar UIToolBar;

        public event Action<UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;

        private void Awake()
        {
            EventHandlers.OnInventoryUpdated += UpdateBagUIItems;
        }

        private void UpdateBagUIItems(InventoryLocation location, InventoryItem[] inventoryItems)
        {
            if (location == InventoryLocation.Player)
            {
                if (uiInventorySlots.Length > 0 && inventoryItems.Length > 0)
                {
                    for (int i = 0; i < uiInventorySlots.Length; i++)
                    {
                        if (i < inventoryItems.Length)
                        {
                            string itemId = inventoryItems[i].itemId;

                            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

                            if (itemDetail != null)
                            {
                                uiInventorySlots[i].SetData(itemId, itemDetail.ItemImage, inventoryItems[i].quantity);
                            }
                        }
                    }
                }


            }
        }

        public override void InitializeInventoryUI(int avalableCapacity)
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].SetItemLocation(InventoryLocation.Player);
            }

        }


        /// <summary>
        /// Handle logic of swap data in bag view
        /// </summary>
        /// <param name="item"></param>
        protected override void HandleSwap(UIInventorySlot item)
        {
            if (currentlyDraggedItem == null) return;

            UIInventoryItemKeyData itemData1 = new UIInventoryItemKeyData
            {
                Index = currentlyDraggedItem.ItemIndex,
                ItemLocation = currentlyDraggedItem.ItemLocation
            };
            UIInventoryItemKeyData itemData2 = new UIInventoryItemKeyData
            {
                Index = item.ItemIndex,
                ItemLocation = item.ItemLocation
            };

            OnSwapItems?.Invoke(itemData1, itemData2);
        }
    }

}

