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

        public bool IsEmpty => string.IsNullOrEmpty(itemID);
        private Canvas parentCanvas;
        private bool isUnlocked = false;

        private void Awake()
        {
            Deselect();
            parentCanvas = GetComponentInParent<Canvas>();
        }

        /// <summary>
        /// Get the sprite and text in item ui
        /// </summary>
        /// <returns></returns>
        public (Sprite, TextMeshProUGUI) GetData()
        {
            return (inventorySlotItemImage.sprite, itemQuantityText);
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

        /// <summary>
        /// Show the data again when not dropping dragged item into slot
        /// </summary>
        public void ShowData()
        {
            inventorySlotItemImage.gameObject.SetActive(inventorySlotItemImage.sprite != null);
        }
        /// <summary>
        /// Hide the data when begining to drag the item
        /// </summary>
        public void HideData()
        {
            inventorySlotItemImage.gameObject.SetActive(false);
        }

        #region Select & Deselect
        /// <summary>
        /// Select item ui then show the select box
        /// </summary>
        public void Select()
        {
            inventorySlotHighlightImage.gameObject.SetActive(true);
        }

        /// <summary>
        /// Deselect item ui then disappear the select box
        /// </summary>
        public void Deselect()
        {
            inventorySlotHighlightImage.gameObject.SetActive(false);
        }

        #endregion

        public void SetUnlocked(bool unlocked)
        {
            isUnlocked = unlocked;
        }

        #region UI Events
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
            if (!isUnlocked) return;
            EventHandlers.CallOnPointerClick(this);
        }
        #endregion
    }
}