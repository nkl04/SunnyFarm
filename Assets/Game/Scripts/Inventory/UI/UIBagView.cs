namespace SunnyFarm.Game.Inventory.UI
{
    using System;
    using System.Collections.Generic;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIBagView : UIInventoryView
    {
        [SerializeField] private List<TextMeshProUGUI> quickSelectSlotTexts;
        [SerializeField] private Color selectedInventorySlotColor;
        [SerializeField] private Color baseInventorySlotColor;
        [SerializeField] private Sprite lockedSlotSprite;

        public override void SetupUIInventorySlot()
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
                            InventoryItem inventoryItem = inventoryItems[i];

                            string itemId = inventoryItem.itemID;

                            int itemQuantity = inventoryItem.quantity;

                            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

                            if (itemDetail != null && itemQuantity > 0)
                            {
                                uiInventorySlots[i].SetData(itemId, itemDetail.ItemImage, inventoryItems[i].quantity);
                            }
                            else
                            {
                                uiInventorySlots[i].SetData(null, transparentSprite, 0);
                            }

                            if (uiInventorySlots[i].isSelected)
                            {
                                InventoryController.Instance.InventoryData.SetSelectedInventoryItem(location, uiInventorySlots[i].itemID);

                                InventoryController.Instance.SelectedItemCursor.SetData(uiInventorySlots[i].itemID, uiInventorySlots[i].itemQuantity);
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


        public void ClearHighlightOnInventorySlots()
        {
            if (uiInventorySlots.Length > 0)
            {
                // Clear highlight quick select slot (change the color of the text)
                for (int i = 0; i < quickSelectSlotTexts.Count; i++)
                {
                    quickSelectSlotTexts[i].color = baseInventorySlotColor;
                }

                // Clear all selected items
                for (int i = 0; i < uiInventorySlots.Length; i++)
                {
                    uiInventorySlots[i].SetSelect(false);

                    uiInventorySlots[i].SetHighLight(false);

                    InventoryController.Instance.InventoryData.ClearSelectedInventoryItem(InventoryLocation.Player);
                }
            }
        }

        public void SetHighlightSelectInventorySlot(int slotIndex)
        {
            if (uiInventorySlots.Length > 0)
            {
                // Just set hight light for the quick number of the slot
                if (slotIndex < quickSelectSlotTexts.Count)
                {
                    // Set highlight quick select slot (change the color of the text)
                    quickSelectSlotTexts[slotIndex].color = selectedInventorySlotColor;
                }

                // select the slot
                uiInventorySlots[slotIndex].SetSelect(true);

                // Update the selected item
                InventoryController.Instance.InventoryData.SetSelectedInventoryItem(InventoryLocation.Player, uiInventorySlots[slotIndex].itemID);
            }
        }
    }

}

