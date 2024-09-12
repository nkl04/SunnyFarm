namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game.Constant;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    public class UIInventoryView : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;

        [SerializeField] private RectTransform quickAccessPanel;
        [SerializeField] private RectTransform notQuickAccessPanel;
        [SerializeField] private UIInventoryDescription uiInventoryDescription;

        // field to store quick-access items
        List<UIInventoryItem> quickAccessItems = new List<UIInventoryItem>();

        // field to store not quick-access items
        List<UIInventoryItem> nonQuickAccessItems = new List<UIInventoryItem>();

        public event Action<int> OnDescriptionRequested,
                OnStartDragging;

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
            for (int i = 0; i < Constant.Inventory.MaxCapacity; i++)
            {
                // Instantiate item
                UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);

                // set up events
                item.OnItemClicked += HandleItemClick;
                item.OnItemBeginDrag += HandleItemBeginDrag;
                item.OnItemDrag += HandleItemDrag;
                item.OnItemEndDrag += HandleItemEndDrag;
                item.OnItemHover += HandleItemHover;

                // set the item's parent and store in their array
                if (i < Constant.Inventory.HotbarCapacity)
                {
                    item.transform.SetParent(quickAccessPanel);
                    quickAccessItems.Add(item);
                }
                else
                {
                    item.transform.SetParent(notQuickAccessPanel);
                    nonQuickAccessItems.Add(item);
                }
            }
            // Unlock the slot item based on capacity
            for (int i = 0; i < capacity; i++)
            {
                // init for level 1 of inventory
                if (i < Constant.Inventory.HotbarCapacity)
                {
                    quickAccessItems[i].UnlockSlot();
                }
                // init for other levels of inventory
                else
                {
                    nonQuickAccessItems[i - Constant.Inventory.HotbarCapacity].UnlockSlot();
                }
            }
        }

        #region Handle events' logic
        /// <summary>
        /// Check if can swap and implement the logic of swapping
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemEndDrag(UIInventoryItem item)
        {

        }
        /// <summary>
        /// Item follow the mouse position
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemDrag(UIInventoryItem item)
        {

        }
        /// <summary>
        /// Reset data in item ui and instantiate item
        /// </summary>
        /// <param name="item"></param>
        private void HandleItemBeginDrag(UIInventoryItem item)
        {

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

        }
        #endregion

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