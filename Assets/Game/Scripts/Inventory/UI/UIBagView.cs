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
        [SerializeField] private Sprite lockedSlotSprite;
        public event Action<UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;

        public override void InitializeInventoryUI(int avalableCapacity)
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].SetItemLocation(InventoryLocation.Player);
            }
        }

        public void UpdateUIBag(InventoryLocation location, InventoryItem[] inventoryItems)
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

        public void UpdateUIBagCapacity(InventoryLocation location, int capacity)
        {
            if (location == InventoryLocation.Player)
            {
                if (uiInventorySlots.Length > 0 && capacity > 0)
                {
                    for (int i = 0; i < uiInventorySlots.Length; i++)
                    {
                        if (i < capacity)
                        {
                            uiInventorySlots[i].SetUnlocked(true);
                            uiInventorySlots[i].inventorySlotItemImage.sprite = transparentSprite;
                        }
                        else
                        {
                            uiInventorySlots[i].SetUnlocked(false);
                            uiInventorySlots[i].inventorySlotItemImage.sprite = lockedSlotSprite;
                        }
                    }
                }
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

