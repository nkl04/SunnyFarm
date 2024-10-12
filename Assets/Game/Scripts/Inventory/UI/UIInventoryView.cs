namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public abstract class UIInventoryView : MonoBehaviour
    {
        [SerializeField] protected RectTransform draggedItem;

        [SerializeField] protected UIInventoryDescription uiInventoryDescription;

        protected UIInventorySlot currentlyDraggedItem = null;

        [SerializeField] protected UIInventorySlot[] uiInventorySlots;

        private bool isDragging;

        // Event
        public event Action<UIInventoryItemKeyData> OnDescriptionRequested;


        protected virtual void OnEnable()
        {

            EventHandlers.OnItemHover += HandleItemHover;
            EventHandlers.OnItemEndHover += HandleItemEndHover;
        }

        protected virtual void OnDisable()
        {

            EventHandlers.OnItemHover -= HandleItemHover;
            EventHandlers.OnItemEndHover -= HandleItemEndHover;
        }

        /// <summary>
        /// Init item slot ui for the whole inventory 
        /// </summary>
        /// <param name="capacity"></param>
        public abstract void InitializeInventoryUI(int capacity);

        /// <summary>
        /// Reset all slot in inventory items
        /// </summary>
        public void ResetAllUIItems()
        {
            foreach (var item in uiInventorySlots)
            {
                item.ResetData();
                item.Deselect();
            }
        }
        #region Handle events' logic


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
        /// <param name="uiSlot"></param>
        protected void HandleItemHover(UIInventorySlot uiSlot)
        {
            if (isDragging) return;
            if (uiSlot.itemQuantity != 0)
            {
                ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(uiSlot.itemID);

                string itemName = itemDetail.Name;
                string itemType = itemDetail.ItemType.ToString();
                string itemDescription = itemDetail.Description;

                uiInventoryDescription.SetTextboxText(itemName, itemType, "", itemDescription, "", "");

                if (uiSlot.toolBar.IsToolBarBottomPosition)
                {
                    uiInventoryDescription.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                    uiInventoryDescription.transform.position = new Vector2(uiSlot.transform.position.x, uiSlot.transform.position.y + 50);
                }
                else
                {
                    uiInventoryDescription.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                    uiInventoryDescription.transform.position = new Vector2(uiSlot.transform.position.x, uiSlot.transform.position.y - 50);
                }

                uiInventoryDescription.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndHover(UIInventorySlot item)
        {
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


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public abstract class UILargeInventoryView : UIInventoryView
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            EventHandlers.OnItemBeginDrag += HandleItemBeginDrag;
            EventHandlers.OnItemDrag += HandleItemDrag;
            EventHandlers.OnItemEndDrag += HandleItemEndDrag;
            EventHandlers.OnItemDroppedOn += HandleSwap;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventHandlers.OnItemBeginDrag -= HandleItemBeginDrag;
            EventHandlers.OnItemDrag -= HandleItemDrag;
            EventHandlers.OnItemEndDrag -= HandleItemEndDrag;
            EventHandlers.OnItemDroppedOn -= HandleSwap;
        }

        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <returns></returns>
        protected abstract void HandleSwap(UIInventorySlot item);
    }
}