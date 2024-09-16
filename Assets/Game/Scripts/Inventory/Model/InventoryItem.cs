namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.Item.Data;
    using System;

    [Serializable]
    public class InventoryItem
    {
        public int quantity;
        public Item item;
        public bool IsEmpty => item == null;

        /// <summary>
        /// Change the quantity of the item
        /// </summary>
        /// <param name="quantity"></param>
        public void ChangeQuantity(int _quantity)
        {
            quantity = _quantity;
        }
        /// <summary>
        /// Change the item and quantity when swapping
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void SwapItem(Item _item, int _quantity)
        {
            item = _item;
            quantity = _quantity;
        }
    }
}