namespace SunnyFarm.Game.Inventory.UI
{
    using System.Collections.Generic;
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

        private void Start()
        {
            SetupUIInventorySlot();
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

        /// <summary>
        /// Set up items' UI to the tool bar
        /// </summary>
        /// <returns></returns>
        public void SetupUIInventorySlot()
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].inventoryLocation = InventoryLocation.Player;
                uiInventorySlots[i].slotLocation = InventorySlotLocation.ToolBar;
                uiInventorySlots[i].slotIndex = i;
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