namespace SunnyFarm.Game.Inventory
{
    using SunnyFarm.Game.Input;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using SunnyFarm.Game.Managers.GameInput;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class InventoryController : MonoBehaviour
    {
        // Define map for capacity of the inventory based on inventory's level
        // key: level, value: capacity
        private Dictionary<int, int> evolveInventoryMap = new Dictionary<int, int>()
        {
            {1, 10},
            {2, 20},
            {3, 30},
        };

        [SerializeField]
        private UIBagView uiBagView;
        [SerializeField]
        private UIChestView uiChestView;
        private InventoryDataController inventoryData;

        [SerializeField] private List<InventoryItem> initialItems = new List<InventoryItem>();

        private PlayerInputAction inputActions;

        private void Start()
        {

            // register event
            GameInputManager.Instance.InputActions.Player.Inventory.started += uiBagView.OnOpenOrCloseBag;

            // assign data controller
            inventoryData = InventoryDataController.Instance;

            SetupView();
            SetupModel();
        }

        /// <summary>
        /// Set up for the model
        /// </summary>
        private void SetupModel()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];
            inventoryData.Setup();

            inventoryData.OnBagUpdated += UpdateBagUIItems;
            inventoryData.OnChestUpdated += UpdateChestUIItems;

            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue;
                inventoryData.AddItem(item.Item, item.Quantity);
            }
        }

        /// <summary>
        /// Set up for the view
        /// </summary>
        private void SetupView()
        {
            int capacity = evolveInventoryMap[inventoryData.InventoryLevel];
            uiBagView.InitializeInventoryUI(capacity);

            // Regist events for bag view
            uiBagView.OnSwapItems += HandleSwapItemsInBagView;
            uiBagView.OnDescriptionRequested += HandleDescriptionRequested;

            // Regist events for chest view
            uiChestView.InitializeInventoryUI(30); // test
            uiChestView.OnSwapItems += HandleSwapItemsInChestView;
            uiChestView.OnDescriptionRequested += HandleDescriptionRequested;
        }



        #region Handle events
        /// <summary>
        /// Hande logic of swap item in bag view
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        private void HandleSwapItemsInBagView(UIInventoryItemKeyData item1, UIInventoryItemKeyData item2)
        {
            if (item1.CompareLocation(item2))
                inventoryData.SwapItemsInBag(item1, item2);
            else
                inventoryData.SwapItemsInDifLocation(uiChestView.ID, item1, item2);
        }
        /// <summary>
        /// Hande logic of swap item in chest view
        /// </summary>
        /// <param name="chestID"></param>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        private void HandleSwapItemsInChestView(string chestID, UIInventoryItemKeyData item1,
            UIInventoryItemKeyData item2)
        {
            if (item1.CompareLocation(item2))
                inventoryData.SwapItemsInChest(chestID, item1, item2);
            else
                inventoryData.SwapItemsInDifLocation(chestID, item1, item2);
        }
        /// <summary>
        /// Handle the requested description event
        /// </summary>
        private void HandleDescriptionRequested(UIInventoryItemKeyData itemData)
        {
            InventoryItem item = null;
            if (itemData.ItemLocation == InventoryLocation.Player)
            {
                item = inventoryData.GetItemInBag(itemData.Index);
                if (item.IsEmpty) return;

                uiBagView.UpdateItemDescription(itemData.Index, item.Item.Name,
                    item.Item.ItemType, item.Item.Description);
            }
            else
            {
                item = inventoryData.GetItemInChest(uiChestView.ID, itemData.Index);
                if (item.IsEmpty) return;

                uiChestView.UpdateItemDescription(itemData.Index, item.Item.Name,
                    item.Item.ItemType, item.Item.Description);
            }
        }
        #endregion

        /// <summary>
        /// Update bag view and mini bag view based on inventory list of data in model
        /// </summary>
        /// <param name="inventoryItems"></param>
        private void UpdateBagUIItems(InventoryItem[] inventoryItems)
        {
            uiBagView.ResetAllUIItems();
            for (int i = 0; i < evolveInventoryMap[inventoryData.InventoryLevel]; i++)
            {
                uiBagView.UpdateUIItemData(i, inventoryItems[i].Item?.ItemImage ?? null,
                    inventoryItems[i].Quantity);

                if (i < Constant.Inventory.ToolbarCapacity)
                {
                    uiBagView.UIToolBar.UpdateUIItemData(i, inventoryItems[i].Item?.ItemImage ?? null,
                        inventoryItems[i].Quantity);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inventoryItems"></param>
        private void UpdateChestUIItems(string id, InventoryItem[] inventoryItems)
        {
            uiChestView.ResetAllUIItems();
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                uiChestView.UpdateUIItemData(i, inventoryItems[i].Item?.ItemImage ?? null,
                    inventoryItems[i].Quantity);
            }
        }
    }
}