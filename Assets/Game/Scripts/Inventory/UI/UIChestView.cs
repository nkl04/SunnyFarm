using SunnyFarm.Game.Inventory.UI;
using System;
using UnityEngine;
using static SunnyFarm.Game.Constant.Enums;

public class UIChestView : UILargeInventoryView
{
    [SerializeField] private string id; // for complie error

    [SerializeField] private RectTransform content;

    public string ID { get; private set; }

    public override void InitializeInventoryUI(int capacity)
    {
        // Instantiate items first and set up their events
        for (int i = 0; i < uiInventorySlots.Length; i++)
        {
            // Instantiate item in main inventory
            uiInventorySlots[i].inventoryLocation = InventoryLocation.Chest;
            uiInventorySlots[i].slotLocation = InventorySlotLocation.Container;
            uiInventorySlots[i].slotIndex = i;
        }
        // Unlock the slot item based on capacity
        for (int i = 0; i < capacity; i++)
        {
            uiInventorySlots[i].IsUnlocked = true;
        }
    }
}

