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
        [SerializeField] private Sprite transparentSprite;
        private RectTransform rectTransform;
        private bool isToolBarBottomPosition = true;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            EventHandlers.OnInventoryUpdated += InventoryUpdated;
        }

        private void Start()
        {
            SetupItemsUI();
        }

        private void InventoryUpdated(InventoryLocation location, InventoryItem[] inventoryItems)
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
                            string itemId = inventoryItems[i].itemId;

                            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

                            if (itemDetail != null)
                            {
                                uiInventorySlots[i].SetData(itemDetail.ItemImage, inventoryItems[i].quantity);
                            }
                        }
                    }
                }
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

        /// <summary>
        /// Set up items' UI to the tool bar
        /// </summary>
        /// <returns></returns>
        public UIInventorySlot[] SetupItemsUI()
        {
            for (int i = 0; i < uiInventorySlots.Length; i++)
            {
                uiInventorySlots[i].SetItemLocation(InventoryLocation.Player);
                // uiInventorySlots[i].OnItemHover += HandleItemHover;
                // uiInventorySlots[i].OnItemEndHover += HandleItemEndHover;
            }
            return uiInventorySlots;
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


        public override void InitializeInventoryUI(int capacity)
        {
            throw new System.NotImplementedException();
        }
    }
}