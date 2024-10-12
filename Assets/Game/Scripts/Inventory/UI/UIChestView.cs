using SunnyFarm.Game.Inventory.UI;
using System;
using UnityEngine;
using static SunnyFarm.Game.Constant.Enums;

public class UIChestView : UILargeInventoryView
{
    [SerializeField] private string id; // for complie error

    [SerializeField] private RectTransform content;

    public string ID { get; private set; }
    public event Action<string, UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;

    public override void InitializeInventoryUI(int capacity)
    {
        // Instantiate items first and set up their events
        for (int i = 0; i < uiInventorySlots.Length; i++)
        {
            // Instantiate item in main inventory
            uiInventorySlots[i].SetItemLocation(InventoryLocation.Chest);
        }
        // Unlock the slot item based on capacity
        for (int i = 0; i < capacity; i++)
        {
            uiInventorySlots[i].SetUnlocked(true);
        }
    }
    /// <summary>
    /// Hande swap logic in chest view
    /// </summary>
    /// <param name="item"></param>
    protected override void HandleSwap(UIInventorySlot item)
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

        OnSwapItems?.Invoke(ID, itemData1, itemData2);
    }


}
/// <summary>
/// Struct for store key data of UI inventory item
/// </summary>
public struct UIInventoryItemKeyData
{
    public int Index;
    public InventoryLocation ItemLocation;

    public bool CompareLocation(UIInventoryItemKeyData data)
    {
        return ItemLocation == data.ItemLocation;
    }
}
