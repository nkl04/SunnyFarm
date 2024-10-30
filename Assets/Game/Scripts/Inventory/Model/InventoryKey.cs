using static SunnyFarm.Game.Constant.Enums;

namespace SunnyFarm.Game.Inventory.Data
{
    public class InventoryKey
    {
        public InventoryLocation inventoryLocation;

        public int suffixNumber;

        public InventoryKey(InventoryLocation inventoryLocation, int suffixNumber)
        {
            this.inventoryLocation = inventoryLocation;
            this.suffixNumber = suffixNumber;
        }
    }
}