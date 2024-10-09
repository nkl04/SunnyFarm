namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public abstract class UIInventoryView : MonoBehaviour
    {
        [SerializeField] protected UIInventorySlot itemPrefab;

        [SerializeField] protected RectTransform draggedItem;

        [SerializeField] protected UIInventoryDescription uiInventoryDescription;

        protected UIInventorySlot currentlyDraggedItem = null;

        [SerializeField]
        protected UIInventorySlot[] listOfUIItems = new UIInventorySlot[Constant.Inventory.PlayerInventoryMaxCapacity];

        private bool isDragging;

        // Event
        public event Action<UIInventoryItemKeyData> OnDescriptionRequested;

        private void OnEnable()
        {
            EventHandlers.OnItemBeginDrag += HandleItemBeginDrag;
            EventHandlers.OnItemDrag += HandleItemDrag;
            EventHandlers.OnItemEndDrag += HandleItemEndDrag;
            EventHandlers.OnItemDroppedOn += HandleSwap;
            EventHandlers.OnItemHover += HandleItemHover;
            EventHandlers.OnItemEndHover += HandleItemEndHover;
        }

        private void OnDisable()
        {
            EventHandlers.OnItemBeginDrag -= HandleItemBeginDrag;
            EventHandlers.OnItemDrag -= HandleItemDrag;
            EventHandlers.OnItemEndDrag -= HandleItemEndDrag;
            EventHandlers.OnItemDroppedOn -= HandleSwap;
            EventHandlers.OnItemHover -= HandleItemHover;
            EventHandlers.OnItemEndHover -= HandleItemEndHover;
        }

        /// <summary>
        /// Init item slot ui for the whole inventory 
        /// </summary>
        /// <param name="capacity"></param>
        public abstract void InitializeInventoryUI(int capacity);

        /// <summary>
        /// Update data for the inventory item UI
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        public void UpdateUIItemData(int itemIdx, Sprite sprite, int quantity)
        {
            listOfUIItems[itemIdx].SetData(sprite, quantity);
        }
        /// <summary>
        /// Update description for the hover's item
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="itemDescription"></param>
        public void UpdateItemDescription(int itemIdx, string itemName, ItemType itemType, string itemDescription)
        {
            uiInventoryDescription.SetDescription(itemName, itemType, itemDescription);
        }
        /// <summary>
        /// Reset all slot in inventory items
        /// </summary>
        public void ResetAllUIItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
        #region Handle events' logic
        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <returns></returns>
        protected abstract void HandleSwap(UIInventorySlot item);

        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndDrag(UIInventorySlot item)
        {
            isDragging = false;
            ResetDraggedItem(item);
        }
        /// <summary>
        /// Item follow the mouse position
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemDrag(UIInventorySlot item)
        {
            Vector2 position;
            Canvas canvas = transform.GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
                    );
            draggedItem.position = canvas.transform.TransformPoint(position);
        }
        /// <summary>
        /// Reset data in item ui and instantiate item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemBeginDrag(UIInventorySlot item)
        {
            isDragging = true;

            currentlyDraggedItem = item;
            // Hide the data in drag slot
            item.HideData();
            // Set up for the dragged item
            (Sprite Sprite, TMP_Text Text) data = item.GetData();
            SetupDraggedItem(data.Sprite, data.Text);

            draggedItem.gameObject.SetActive(true);
        }
        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemHover(UIInventorySlot item)
        {
            if (isDragging) return;

            UIInventoryItemKeyData itemData = new UIInventoryItemKeyData
            {
                Index = item.ItemIndex,
                ItemLocation = item.ItemLocation
            };

            Vector2 position = item.transform.position;
            ShowUpDescription(position);

            OnDescriptionRequested?.Invoke(itemData);
        }

        /// <summary>
        /// Show the description based on mouse position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="canvas"></param>
        private void ShowUpDescription(Vector2 position)
        {
            uiInventoryDescription.gameObject.SetActive(true);

            Vector2 sizeUI = uiInventoryDescription.GetComponent<RectTransform>().sizeDelta;

            Vector2 resolution = new Vector2(480, 270);

            if (position.y - (sizeUI.y + 10) < 0)
            {
                uiInventoryDescription.ChangePivot();
            }

            uiInventoryDescription.transform.position = position;
        }

        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndHover(UIInventorySlot item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;

            uiInventoryDescription.ResetPivot();
            uiInventoryDescription.gameObject.SetActive(false);
        }
        /// <summary>
        /// Reset dragged item when end dragging
        /// </summary>
        /// <param name="item"></param>
        private void ResetDraggedItem(UIInventorySlot item)
        {
            item.ShowData();
            draggedItem.gameObject.SetActive(false);
            currentlyDraggedItem = null;
        }
        #endregion

        /// <summary>
        /// Set data for the dragged item 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        private void SetupDraggedItem(Sprite sprite, TMP_Text quantity)
        {
            draggedItem.GetComponent<Image>().sprite = sprite;
            draggedItem.GetComponentInChildren<TMP_Text>().text = quantity.text;
        }

    }
}