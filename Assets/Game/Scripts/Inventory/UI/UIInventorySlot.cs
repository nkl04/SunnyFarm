namespace SunnyFarm.Game.Inventory.UI
{
    using SunnyFarm.Game.Entities.Item.Data;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIInventorySlot : MonoBehaviour,
         IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public UIToolBar toolBar;

        [Header("Slot")]
        public int slotIndex;
        public Image inventorySlotHighlightImage;
        public Image inventorySlotItemImage;
        public TextMeshProUGUI itemQuantityText;
        public InventoryLocation inventoryLocation;
        public InventorySlotLocation slotLocation;

        [Header("Inventory Slot Description")]
        [SerializeField] private GameObject inventorySlotDescription;

        [HideInInspector] public string itemID;
        [HideInInspector] public int itemQuantity;
        [HideInInspector] public bool isSelected = false;

        public bool IsEmpty => string.IsNullOrEmpty(itemID);
        public bool IsUnlocked { get; set; }

        private void Awake()
        {
            SetSelect(false);
            SetHighLight(false);
        }

        /// <summary>
        /// Set data to the ui, use it when having data from the db
        /// </summary>
        /// <param name="image"></param>
        /// <param name="quantity"></param>
        public void SetData(string itemId, Sprite image, int quantity)
        {
            itemID = itemId;
            itemQuantity = quantity;
            inventorySlotItemImage.sprite = image;
            itemQuantityText.text = itemQuantity > 1 ? itemQuantity.ToString() : "";
        }

        public void SetSelect(bool select)
        {
            isSelected = select;
        }

        public void SetHighLight(bool isHighLight)
        {
            inventorySlotHighlightImage.gameObject.SetActive(isHighLight);
        }

        public void ClearInventoryItem()
        {
            itemID = string.Empty;
            itemQuantity = 0;
            inventorySlotItemImage.sprite = null;
            itemQuantityText.text = string.Empty;
        }

        #region Events
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventHandlers.CallOnItemHover(this);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            EventHandlers.CallOnItemEndHover(this);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsUnlocked) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //Click right mouse button
                EventHandlers.CallOnRightPointerClick(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                //Click left mouse button
                EventHandlers.CallOnLeftPointerClick(this);
            }
        }
        #endregion
    }
}