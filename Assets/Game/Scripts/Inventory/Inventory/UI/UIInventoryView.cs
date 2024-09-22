namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    public class UIInventoryView : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;

        [SerializeField] private RectTransform draggedItem;

        [SerializeField] private RectTransform quickAccessPanel;
        [SerializeField] private RectTransform notQuickAccessPanel;
        [SerializeField] private UIInventoryDescription uiInventoryDescription;

        private int currentlyDraggedItemIndex = -1;

        // field to store quick-access items
        [SerializeField]
        UIInventoryItem[] listOfUIItems = new UIInventoryItem[Constant.Inventory.MaxCapacity];

        // Event
        public event Action OnDescriptionRequested;
        public event Action<int, int> OnSwapItems;

        private void Awake()
        {

        }

        /// <summary>
        /// Init item slot ui for the whole inventory 
        /// </summary>
        /// <param name="capacity"></param>
        public void InitializeInventoryUI(int capacity)
        {
            // Instantiate items first and set up their events
            for (int i = 0; i < listOfUIItems.Length; i++)
            {
                // Instantiate item
                UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.ItemIndex = i;
                listOfUIItems[i] = item;

                // set up events
                item.OnItemClicked += HandleItemClick;
                item.OnItemBeginDrag += HandleItemBeginDrag;
                item.OnItemDrag += HandleItemDrag;
                item.OnItemEndDrag += HandleItemEndDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemHover += HandleItemHover;

                // set the item's parent and store in their array
                if (i < Constant.Inventory.HotbarCapacity)
                {
                    item.transform.SetParent(quickAccessPanel);
                }
                else
                {
                    item.transform.SetParent(notQuickAccessPanel);
                }
            }
            // Unlock the slot item based on capacity
            for (int i = 0; i < capacity; i++)
            {
                listOfUIItems[i].UnlockSlot();
            }
        }
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
        /// <exception cref="NotImplementedException"></exception>
        private void HandleSwap(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (currentlyDraggedItemIndex == -1) return;
            OnSwapItems?.Invoke(index, currentlyDraggedItemIndex);

        }
        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem(item);
        }
        /// <summary>
        /// Item follow the mouse position
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemDrag(UIInventoryItem item)
        {
            Vector2 position;
            Canvas canvas = transform.root.GetComponent<Canvas>();
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
        private void HandleItemBeginDrag(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
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
        private void HandleItemHover(UIInventoryItem item)
        {
            int index = item.ItemIndex;
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke();
        }
        /// <summary>
        /// Reset dragged item when end dragging
        /// </summary>
        /// <param name="item"></param>
        private void ResetDraggedItem(UIInventoryItem item)
        {
            item.ShowData();
            draggedItem.gameObject.SetActive(false);
            currentlyDraggedItemIndex = -1;
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


        /// <summary>
        /// Open or close the inventory UI based on is E pressed?
        /// </summary>
        /// <param name="context"></param>
        public void OnOpenOrCloseInventory(InputAction.CallbackContext context)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }

    }
}