namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIToolBar : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem[] itemsUI = new UIInventoryItem[Constant.Inventory.ToolbarCapacity];
        private RectTransform rectTransform;
        private bool isToolBarBottomPosition = true;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            SwitchUIToolBarPosition();
        }

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

        private void SwitchUIToolBarPosition()
        {
            Vector3 playerPos = Player.Instance.GetViewportPosition(); // Get the player's viewport position

            if (playerPos.y > 0.3f && !isToolBarBottomPosition)
            {

                rectTransform.pivot = new Vector2(0.5f, 0f);
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                rectTransform.anchoredPosition = new Vector2(0, 8f);

                isToolBarBottomPosition = true;
            }
            else if (playerPos.y <= 0.3f && isToolBarBottomPosition)
            {
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = new Vector2(0, -8f);

                isToolBarBottomPosition = false;
            }
        }
    }
}