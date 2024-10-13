namespace SunnyFarm.Game.Inventory.Data
{
    using System;

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public string itemID;
        public bool isEmpty => string.IsNullOrEmpty(itemID);
        public void IncrementQuantity(int value)
        {
            quantity += value;
        }

        public InventoryItem(string itemId)
        {
            this.itemID = itemId;
            quantity = 0;
        }

        public InventoryItem(string itemId, int quantity)
        {
            this.itemID = itemId;
            this.quantity = quantity;
        }

        public void SetData(string itemId, int quantity)
        {
            this.itemID = itemId;
            this.quantity = quantity;
        }
    }
}