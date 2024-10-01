namespace SunnyFarm.Game.Inventory.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIBagView : UIInventoryView
    {
        [SerializeField] private RectTransform quickAccessPanel;
        [SerializeField] private RectTransform notQuickAccessPanel;

        public UIMiniBag UIMiniBag;

        public event Action<UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void InitializeInventoryUI(int capacity)
        {
            //
            SetupMiniBagUI();
            // Instantiate items first and set up their events
            for (int i = 0; i < listOfUIItems.Length; i++)
            {
                // Instantiate item in main inventory
                UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
                item.ItemIndex = i;
                item.SetItemLocation(InventoryLocation.Player);
                listOfUIItems[i] = item;

                // set up events
                item.OnItemBeginDrag += HandleItemBeginDrag;
                item.OnItemDrag += HandleItemDrag;
                item.OnItemEndDrag += HandleItemEndDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemHover += HandleItemHover;
                item.OnItemEndHover += HandleItemEndHover;

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
        void SetupMiniBagUI()
        {
            var items = UIMiniBag.SetupItemsUI();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].OnItemHover += HandleItemHover;
                items[i].OnItemEndHover += HandleItemEndHover;
            }
        }
        /// <summary>
        /// Open or close the inventory UI based on is E pressed?
        /// </summary>
        /// <param name="context"></param>
        public void OnOpenOrCloseBag(InputAction.CallbackContext context)
        {
            UIMiniBag.gameObject.SetActive(gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
        }

        protected override void HandleSwap(UIInventoryItem item)
        {
            if (currentlyDraggedItem == null) return;

            UIInventoryItemKeyData itemData1 = new UIInventoryItemKeyData
            {
                Index = currentlyDraggedItem.ItemIndex,
                ItemLocation = currentlyDraggedItem.ItemLocation
            };
            UIInventoryItemKeyData itemData2 = new UIInventoryItemKeyData
            {
                Index = item.ItemIndex,
                ItemLocation = item.ItemLocation
            };

            OnSwapItems?.Invoke(itemData1, itemData2);
        }
    }
}

