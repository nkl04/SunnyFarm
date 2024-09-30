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
        public UIInventoryItem[] SetupItemsUI()
        {
            for (int i = 0; i < itemsUI.Length; i++)
            {
                itemsUI[i] = transform.GetChild(i).GetComponent<UIInventoryItem>();
                itemsUI[i].SetItemLocation(InventoryLocation.Player);
            }
            return itemsUI;
        }
        public void UpdateUIItemData(int itemIdx, Sprite sprite, int quantity)
        {
            itemsUI[itemIdx].SetData(sprite, quantity);
        }
    }
}