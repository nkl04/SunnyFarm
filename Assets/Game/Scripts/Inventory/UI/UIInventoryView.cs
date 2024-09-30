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
        [SerializeField] protected UIInventoryItem itemPrefab;

        [SerializeField] protected RectTransform draggedItem;

        [SerializeField] protected UIInventoryDescription uiInventoryDescription;

        protected UIInventoryItem currentlyDraggedItem = null;

        [SerializeField]
        protected UIInventoryItem[] listOfUIItems = new UIInventoryItem[Constant.Inventory.MaxCapacity];

        // Event
        public event Action<UIInventoryItemKeyData> OnDescriptionRequested;

        private void Awake()
        {

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
        protected abstract void HandleSwap(UIInventoryItem item);

        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem(item);
        }
        /// <summary>
        /// Item follow the mouse position
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemDrag(UIInventoryItem item)
        {
            Vector2 position;
            Canvas canvas = transform.root.GetComponentInParent<Canvas>();
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
        protected void HandleItemBeginDrag(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;
            currentlyDraggedItem = item;
            // Hide the data in drag slot
            item.HideData();
            // Set up for the dragged item
            (Sprite Sprite, TMP_Text Text) data = item.GetData();
            SetupDraggedItem(data.Sprite, data.Text);

            draggedItem.gameObject.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemClick(UIInventoryItem item)
        {

        }
        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemHover(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;

            UIInventoryItemKeyData itemData = new UIInventoryItemKeyData
            {
                Index = item.ItemIndex,
                ItemLocation = item.ItemLocation
            };

            uiInventoryDescription.gameObject.SetActive(true);

            Vector2 position;
            Canvas canvas = transform.GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
                    );
            uiInventoryDescription.transform.position = canvas.transform.TransformPoint(position);

            OnDescriptionRequested?.Invoke(itemData);
        }
        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndHover(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;

            uiInventoryDescription.gameObject.SetActive(false);
        }
        /// <summary>
        /// Reset dragged item when end dragging
        /// </summary>
        /// <param name="item"></param>
        private void ResetDraggedItem(UIInventoryItem item)
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