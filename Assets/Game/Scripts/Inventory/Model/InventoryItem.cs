namespace SunnyFarm.Game.Inventory.Data
{
    using System;

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public string itemId;
        public bool isEmpty => string.IsNullOrEmpty(itemId);
        public void IncrementQuantity(int value)
        {
            quantity += value;
        }

        public InventoryItem(string itemId)
        {
            this.itemId = itemId;
            quantity = 0;
        }

        public InventoryItem(string itemId, int quantity)
        {
            this.itemId = itemId;
            this.quantity = quantity;
        }

        public void SetData(string itemId, int quantity)
        {
            this.itemId = itemId;
            this.quantity = quantity;
        }
    }
}