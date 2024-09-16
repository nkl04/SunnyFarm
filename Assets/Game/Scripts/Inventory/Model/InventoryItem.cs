namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.Item.Data;
    public class InventoryItem
    {
        public int Quantity { get; private set; }
        public Item Item { get; private set; }
        public bool IsEmpty => Item == null;

        /// <summary>
        /// Change the quantity of the item
        /// </summary>
        /// <param name="quantity"></param>
        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }
        /// <summary>
        /// Change the item and quantity when swapping
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void SwapItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}