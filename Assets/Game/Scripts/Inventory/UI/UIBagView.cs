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

        public override void SetupUIInventorySlot(InventoryKey inventoryKey)
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].inventoryKey = inventoryKey;

                uiInventorySlots[i].slotLocation = InventorySlotLocation.Container;

                uiInventorySlots[i].slotIndex = i;
            }
        }

        public void UpdateUIBag(InventoryKey inventoryKey, InventoryItem[] inventoryItems)
        {
            if (inventoryKey.inventoryLocation == InventoryLocation.Player)
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
                                InventoryManager.Instance.InventoryData.SetSelectedInventoryItem(inventoryKey, uiInventorySlots[i].itemID);

                            }
                        }
                    }
                }
            }
        }

        public void UpdateUIBagCapacity(InventoryKey inventoryKey, int capacity)
        {
            if (inventoryKey.inventoryLocation == InventoryLocation.Player)
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


        public void ClearHighlightOnInventorySlots(InventoryKey inventoryKey)
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

                    InventoryManager.Instance.InventoryData.ClearSelectedInventoryItem(inventoryKey);
                }
            }
        }

        public void SetHighlightSelectInventorySlot(int slotIndex, InventoryKey inventoryKey)
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
                InventoryManager.Instance.InventoryData.SetSelectedInventoryItem(inventoryKey, uiInventorySlots[slotIndex].itemID);
            }
        }
    }

}

