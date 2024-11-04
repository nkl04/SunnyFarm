namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public abstract class UIInventoryView : MonoBehaviour
    {
        [SerializeField] protected Sprite transparentSprite;

        [SerializeField] protected UIInventoryDescription uiInventoryDescription;

        protected UIInventorySlot currentlyDraggedItem = null;

        [SerializeField] protected UIInventorySlot[] uiInventorySlots;
        protected virtual void OnEnable()
        {

            EventHandlers.OnPointerEnter += HandleItemHover;
            EventHandlers.OnPointerExit += HandleItemEndHover;
        }

        protected virtual void OnDisable()
        {

            EventHandlers.OnPointerEnter -= HandleItemHover;
            EventHandlers.OnPointerExit -= HandleItemEndHover;
        }

        public virtual void SetupUIInventorySlot(InventoryKey inventoryKey) { }


        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="_object"></param>
        protected void HandleItemHover(IPointerEnterHandler _object)
        {
            UIInventorySlot uiSlot = _object as UIInventorySlot;

            ConfigItem itemDetail = ItemSystemManager.Instance.GetItemDetail(uiSlot.itemID);

            if (itemDetail != null)
            {
                string itemName = itemDetail.Name;
                string itemType = itemDetail.ItemType.ToString();
                string itemDescription = itemDetail.Description;

                uiInventoryDescription.SetTextboxText(itemName, itemType, "", itemDescription, "", "");

                // if (uiSlot.toolBar.IsToolBarBottomPosition)
                // {
                //     uiInventoryDescription.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                //     uiInventoryDescription.transform.position = new Vector2(uiSlot.transform.position.x, uiSlot.transform.position.y + 50);
                // }
                // else
                // {
                //     uiInventoryDescription.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                //     uiInventoryDescription.transform.position = new Vector2(uiSlot.transform.position.x, uiSlot.transform.position.y - 50);
                // }

                uiInventoryDescription.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Show the description of the item
        /// </summary>
        /// <param name="item"></param>
        protected void HandleItemEndHover(IPointerExitHandler _object)
        {
            uiInventoryDescription.gameObject.SetActive(false);
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            uiInventoryDescription.gameObject.SetActive(false);
        }
    }
}