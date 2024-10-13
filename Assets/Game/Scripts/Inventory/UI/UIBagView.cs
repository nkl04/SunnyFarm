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
        public override void InitializeInventoryUI(int avalableCapacity)
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].inventoryLocation = InventoryLocation.Player;
                uiInventorySlots[i].slotLocation = InventorySlotLocation.Container;
                uiInventorySlots[i].slotIndex = i;
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
                            string itemId = inventoryItems[i].itemID;

                            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

                            if (itemDetail != null)
                            {
                                uiInventorySlots[i].SetData(itemId, itemDetail.ItemImage, inventoryItems[i].quantity);
                            }
                            else
                            {
                                uiInventorySlots[i].SetData(null, transparentSprite, 0);
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
                            uiInventorySlots[i].IsUnlocked = true;
                            uiInventorySlots[i].inventorySlotItemImage.sprite = transparentSprite;
                        }
                        else
                        {
                            uiInventorySlots[i].IsUnlocked = false;
                            uiInventorySlots[i].inventorySlotItemImage.sprite = lockedSlotSprite;
                        }
                    }
                }
            }
        }
    }

}

