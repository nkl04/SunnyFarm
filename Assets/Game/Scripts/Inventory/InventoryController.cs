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
    private InventoryDataController inventoryDataController;

    private PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();

        inputActions.Enable();
        // register event
        inputActions.Inventory.Open.performed += inventoryView.OnOpenOrCloseInventory;

        inventoryDataController = InventoryDataController.Instance;
    }

    private void SetupUI()
    {
        inventoryView.InitializeInventoryUI(inventoryDataController.InventoryLevel);

    }
}
