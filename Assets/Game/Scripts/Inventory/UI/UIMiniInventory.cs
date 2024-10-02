namespace SunnyFarm.Game.Inventory.UI
{
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIMiniBag : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem[] itemsUI = new UIInventoryItem[Constant.Inventory.HotbarCapacity];
        private void Awake()
        {

        }
        void Start()
        {

        }
        /// <summary>
        /// Set up items' UI to the mini bag view
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
        /// Update the item's data in mini bag view
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