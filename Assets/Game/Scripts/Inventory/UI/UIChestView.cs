using SunnyFarm.Game.Inventory.UI;
using System;
using static SunnyFarm.Game.Constant.Enums;

public class UIChestView : UIInventoryView
{
    private string id; // for complie error
    // field: the items in bag
    //


    public string ID { get; private set; }
    public event Action<string, UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void InitializeInventoryUI(int capacity)
    {

    }

    protected override void HandleSwap(UIInventoryItem item)
    {
        int index = item.ItemIndex;
        if (index == -1)
            return;

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
public struct UIInventoryItemKeyData
{
    public int Index;
    public InventoryLocation ItemLocation;

    public bool CompareLocation(UIInventoryItemKeyData data)
    {
        return ItemLocation == data.ItemLocation;
    }
}
