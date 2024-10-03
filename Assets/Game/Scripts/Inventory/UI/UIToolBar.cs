namespace SunnyFarm.Game.Inventory.UI
{
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIToolBar : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem[] itemsUI = new UIInventoryItem[Constant.Inventory.ToolbarCapacity];



        /// <summary>
        /// Set up items' UI to the tool bar
        /// </summary>
        /// <returns></returns>
        public UIInventoryItem[] SetupItemsUI()
        {
            for (int i = 0; i < itemsUI.Length; i++)
            {
                itemsUI[i] = transform.GetChild(i).GetComponent<UIInventoryItem>();
                itemsUI[i].SetItemLocation(InventoryLocation.Player);
            }
            return itemsUI;
        }
        /// <summary>
        /// Update the item's data in tool bar
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        public void UpdateUIItemData(int itemIdx, Sprite sprite, int quantity)
        {
            itemsUI[itemIdx].SetData(sprite, quantity);
        }
    }
}