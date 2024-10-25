namespace SunnyFarm.Game.Inventory.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIToolBar : UIInventoryView
    {
        public bool IsToolBarBottomPosition { get => isToolBarBottomPosition; }
        private RectTransform rectTransform;
        private bool isToolBarBottomPosition = true;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateUIToolBar(InventoryLocation location, InventoryItem[] inventoryItems)
        {
            if (location == InventoryLocation.Player)
            {
                ClearInventorySlot();

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
                                InventoryController.Instance.SelectedItemCursor.SetData(uiInventorySlots[i].itemID, uiInventorySlots[i].itemQuantity);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set up items' UI to the tool bar
        /// </summary>
        /// <returns></returns>
        public override void SetupUIInventorySlot()
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].inventoryLocation = InventoryLocation.Player;

                uiInventorySlots[i].slotLocation = InventorySlotLocation.ToolBar;

                uiInventorySlots[i].slotIndex = i;

                uiInventorySlots[i].IsUnlocked = true;
            }
        }

        private void ClearInventorySlot()
        {
            if (uiInventorySlots.Length > 0)
            {
                for (int i = 0; i < uiInventorySlots.Length; i++)
                {
                    uiInventorySlots[i].inventorySlotItemImage.sprite = transparentSprite;

                    uiInventorySlots[i].itemQuantityText.text = "";
                }
            }
        }

        public void DeselectAllInventorySlot()
        {
            if (uiInventorySlots.Length > 0)
            {
                for (int i = 0; i < uiInventorySlots.Length; i++)
                {
                    uiInventorySlots[i].SetSelect(false);
                }
            }
        }

        public UIInventorySlot GetInventorySlot(int slotIndex)
        {
            if (uiInventorySlots.Length > 0)
            {
                return uiInventorySlots[slotIndex];
            }
            else return null;
        }

        public void SetHighlightSelectInventorySlot(int slotPosition)
        {
            if (uiInventorySlots.Length > 0)
            {
                uiInventorySlots[slotPosition].SetSelect(true);

                uiInventorySlots[slotPosition].SetHighLight(true);

                InventoryController.Instance.InventoryData.SetSelectedInventoryItem(InventoryLocation.Player, uiInventorySlots[slotPosition].itemID);
            }
        }

        public void ClearHighlightOnInventorySlots()
        {
            if (uiInventorySlots.Length > 0)
            {
                // loop through all uiinventory slots and clear the highlight
                for (int i = 0; i < uiInventorySlots.Length; i++)
                {
                    if (uiInventorySlots[i].isSelected)
                    {
                        uiInventorySlots[i].SetSelect(false);

                        uiInventorySlots[i].SetHighLight(false);

                        // Update inventory to show item as not selected
                        InventoryController.Instance.InventoryData.ClearSelectedInventoryItem(InventoryLocation.Player);
                    }
                }
            }
        }

        public UIInventorySlot GetSelectedInventorySlot()
        {
            if (uiInventorySlots.Length > 0)
            {
                return uiInventorySlots.FirstOrDefault(x => x.isSelected);
            }
            return null;
        }

        public UIInventorySlot GetInventorySlotByIndex(int index)
        {
            if (uiInventorySlots.Length > 0)
            {
                return uiInventorySlots.FirstOrDefault(x => x.slotIndex == index);
            }
            else return null;

        }
        /// <summary>
        /// Return the next inventory slot that has an item
        /// </summary>
        /// <param name="currentindex">Index of the current selected slot</param>
        /// <returns></returns>
        public UIInventorySlot GetTheNextInventorySlotHasItem(int currentindex)
        {
            if (uiInventorySlots.Length > 0)
            {
                for (int i = currentindex + 1; i < uiInventorySlots.Length; i++)
                {
                    if (!string.IsNullOrEmpty(uiInventorySlots[i].itemID))
                    {
                        return uiInventorySlots[i];
                    }
                }

                for (int i = 0; i < uiInventorySlots.Length; ++i)
                {
                    if (!string.IsNullOrEmpty(uiInventorySlots[i].itemID))
                    {
                        return uiInventorySlots[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Return the previous inventory slot that has an item
        /// </summary>
        /// <param name="currentindex">Index of the current selected slot</param>
        /// <returns></returns>
        public UIInventorySlot GetThePreviousInventorySlotHasItem(int currentindex)
        {
            if (uiInventorySlots.Length > 0)
            {
                if (currentindex == 0)
                {
                    currentindex = uiInventorySlots.Length;
                }

                for (int i = currentindex - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(uiInventorySlots[i].itemID))
                    {
                        return uiInventorySlots[i];
                    }
                }
            }
            return null;
        }

        private void Update()
        {
            SwitchUIToolBarPosition();
        }

        private void SwitchUIToolBarPosition()
        {
            Vector3 playerPos = Player.Instance.GetViewportPosition(); // Get the player's viewport position

            if (playerPos.y > 0.3f && !isToolBarBottomPosition)
            {

                rectTransform.pivot = new Vector2(0.5f, 0f);

                rectTransform.anchorMin = new Vector2(0.5f, 0f);

                rectTransform.anchorMax = new Vector2(0.5f, 0f);

                rectTransform.anchoredPosition = new Vector2(0, 8f);

                isToolBarBottomPosition = true;
            }
            else if (playerPos.y <= 0.3f && isToolBarBottomPosition)
            {
                rectTransform.pivot = new Vector2(0.5f, 1f);

                rectTransform.anchorMin = new Vector2(0.5f, 1f);

                rectTransform.anchorMax = new Vector2(0.5f, 1f);

                rectTransform.anchoredPosition = new Vector2(0, -8f);

                isToolBarBottomPosition = false;
            }
        }



    }
}