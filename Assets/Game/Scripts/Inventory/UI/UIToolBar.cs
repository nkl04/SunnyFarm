namespace SunnyFarm.Game.Inventory.UI
{
    using System.Collections.Generic;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIToolBar : MonoBehaviour
    {
        [SerializeField] private UIInventorySlot[] inventorySlots = new UIInventorySlot[Constant.Inventory.PlayerInventoryMinCapacity];
        [SerializeField] private Sprite transparentSprite;
        private RectTransform rectTransform;
        private bool isToolBarBottomPosition = true;

        private void OnEnable()
        {
            EventHandlers.OnInventoryUpdated += InventoryUpdated;
        }

        private void OnDisable()
        {
            EventHandlers.OnInventoryUpdated -= InventoryUpdated;
        }

        private void InventoryUpdated(InventoryLocation location, List<InventoryItem> inventoryItemList)
        {
            if (location == InventoryLocation.Player)
            {
                ClearInventorySlot();

                if (inventorySlots.Length > 0 && inventoryItemList.Count > 0)
                {
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (i < inventoryItemList.Count)
                        {
                            string itemId = inventoryItemList[i].itemId;

                            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

                            if (itemDetail != null)
                            {
                                inventorySlots[i].SetData(itemDetail.ItemImage, inventoryItemList[i].quantity);
                            }
                        }
                    }
                }
            }
        }

        private void ClearInventorySlot()
        {
            if (inventorySlots.Length > 0)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    inventorySlots[i].inventorySlotImage.sprite = transparentSprite;
                    inventorySlots[i].itemQuantityText.text = "";
                }
            }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
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
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].SetItemLocation(InventoryLocation.Player);
            }
            return inventorySlots;
        }
        /// <summary>
        /// Update the item's data in tool bar
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        public void UpdateUIItemData(int itemIdx, Sprite sprite, int quantity)
        {
            inventorySlots[itemIdx].SetData(sprite, quantity);
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