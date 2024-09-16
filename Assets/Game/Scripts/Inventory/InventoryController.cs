namespace SunnyFarm.Game.Inventory
{
    using SunnyFarm.Game.Input;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using System.Collections.Generic;
    using UnityEngine;
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
        private UIInventoryView inventoryView;
        private InventoryDataController inventoryData;

        [SerializeField] private List<InventoryItem> initialItems = new List<InventoryItem>();

        private PlayerInputAction inputActions;

        private void Start()
        {
            inputActions = new PlayerInputAction();
            inputActions.Enable();
            // register event
            inputActions.Inventory.Open.performed += inventoryView.OnOpenOrCloseInventory;
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
            inventoryData.SetupSize(capacity);

            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

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
            inventoryView.InitializeInventoryUI(capacity);

            // Regist events
            inventoryView.OnSwapItems += HandleSwapItems;
            inventoryView.OnDescriptionRequested += HandleDescriptionRequested;
        }

        private void HandleSwapItems(int idx1, int idx2)
        {
            inventoryData.SwapItems(idx1, idx2);
        }

        private void HandleDescriptionRequested()
        {

        }

        private void UpdateInventoryUI(InventoryItem[] inventoryItems)
        {
            inventoryView.ResetAllUIItems();
            for (int i = 0; i < evolveInventoryMap[inventoryData.InventoryLevel]; i++)
            {
                inventoryView.UpdateUIItemData(i, inventoryItems[i].Item?.ItemImage ?? null,
                    inventoryItems[i].Quantity);
            }
        }
    }
}