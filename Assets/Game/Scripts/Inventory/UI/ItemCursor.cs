namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using static SunnyFarm.Game.Constant.Enums;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using SunnyFarm.Game.Entities.Player;
    using System;
    using SunnyFarm.Game.Inventory.Data;

    public class ItemCursor : MonoBehaviour
    {
        [SerializeField] private Image draggedItemImage;
        [SerializeField] private Sprite transparentSprite;
        [SerializeField] private TextMeshProUGUI quantityItemText;

        [HideInInspector] public InventoryItem inventoryItem;
        private RectTransform rectTransform;
        private Canvas canvas;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();

            EventHandlers.OnLeftPointerClick += HandleLeftPointerClick;
            EventHandlers.OnToggleInventory += ClearDraggedItem;
        }

        private void HandleLeftPointerClick(UIInventorySlot slot)
        {
            if (slot == null) return;
            if (slot.slotLocation == InventorySlotLocation.ToolBar)
            {
                InventoryItem inventoryItem = new(slot.itemID, 1);
                // if click on slot in toolbar
                // set select the slot

                // if has an item in slot (which can not be carry),
                // set blur item in item cursor
                SetItemCursor(inventoryItem, Constant.ColorStat.Alpha_08);
            }
            else if (slot.slotLocation == InventorySlotLocation.Container)
            {
                ItemCursor itemCursor = this;
                // if has an item in slot, get the item and set it to item cursor
                InventoryController.Instance.InventoryData.HandleSwapItem(slot.inventoryLocation, ref itemCursor, slot);

                if (!itemCursor.inventoryItem.isEmpty)
                {
                    // update item cursor ui
                    SetItemCursor(itemCursor.inventoryItem, Constant.ColorStat.Alpha_1);
                    // if has an item in item cursor, can not toggle inventory
                    Player.Instance.CanToggleInventory = false;
                }
                else
                {
                    // if not has an item in item cursor, can toggle inventory
                    Player.Instance.CanToggleInventory = true;

                    // clear item cursor image
                    ClearDraggedItem();
                }
            }
        }

        private void SetItemCursor(InventoryItem inventoryItem, float apha)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(inventoryItem.itemID);
            if (itemDetail != null)
            {
                this.inventoryItem = inventoryItem;

                draggedItemImage.sprite = itemDetail.ItemImage;

                draggedItemImage.color = new Color(1, 1, 1, apha);

                quantityItemText.text = inventoryItem.quantity > 1 ? inventoryItem.quantity.ToString() : "";

            }
            else
            {
                draggedItemImage.sprite = transparentSprite;
            }
        }


        private void ClearDraggedItem()
        {
            draggedItemImage.sprite = transparentSprite;

            quantityItemText.text = "";
        }

        private void LateUpdate()
        {
            FollowCursor();
        }

        private void FollowCursor()
        {
            if (rectTransform == null || canvas == null) return;

            Vector2 mousePosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint);

            Vector2 adjustedPosition = localPoint + new Vector2(rectTransform.rect.width, -rectTransform.rect.height);

            rectTransform.anchoredPosition = adjustedPosition;
        }
    }

}
