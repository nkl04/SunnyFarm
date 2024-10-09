namespace SunnyFarm.Game.Inventory.UI
{
    using System;
    using SunnyFarm.Game.Entities.Item.Data;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIInventorySlot : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemQuantity;
        [SerializeField] private RectTransform selectBox;

        [Header("Slot")]
        public Image inventorySlotHighlightImage;
        public Image inventorySlotImage;
        public TextMeshProUGUI itemQuantityText;

        private bool isEmpty = false;
        private bool isUnlocked = false;

        public int ItemIndex { get; set; }
        public InventoryLocation ItemLocation { get; private set; }

        public event Action<UIInventorySlot> OnItemDroppedOn,
            OnItemBeginDrag, OnItemDrag, OnItemEndDrag, OnItemHover, OnItemEndHover;

        /// <summary>
        /// Get the sprite and text in item ui
        /// </summary>
        /// <returns></returns>
        public (Sprite, TMP_Text) GetData()
        {
            return (itemImage.sprite, itemQuantity);
        }
        /// <summary>
        /// Set data to the ui, use it when having data from the db
        /// </summary>
        /// <param name="image"></param>
        /// <param name="quantity"></param>
        public void SetData(Sprite image, int quantity)
        {
            itemImage.gameObject.SetActive(image != null);
            itemImage.sprite = image;
            // itemQuantity.text = quantity > 1 ? quantity.ToString() : "";
            isEmpty = image == null;
        }
        /// <summary>
        /// Set item's location to item
        /// </summary>
        /// <param name="location"></param>
        public void SetItemLocation(InventoryLocation location)
        {
            ItemLocation = location;
        }
        /// <summary>
        /// Reset data in item ui
        /// </summary>
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            // itemQuantity.text = "";
            isEmpty = true;
        }
        /// <summary>
        /// Show the data again when not dropping dragged item into slot
        /// </summary>
        public void ShowData()
        {
            itemImage.gameObject.SetActive(itemImage.sprite != null);
        }
        /// <summary>
        /// Hide the data when begining to drag the item
        /// </summary>
        public void HideData()
        {
            itemImage.gameObject.SetActive(false);
        }
        /// <summary>
        /// Deselect item ui then disappear the select box
        /// </summary>
        public void Deselect()
        {
            selectBox.gameObject.SetActive(false);
        }
        /// <summary>
        /// User can interact the slot when unlocking
        /// </summary>
        public void UnlockSlot()
        {
            isUnlocked = true;
            // backgroundSlot.sprite = unlockedBG;
        }

        #region UI Events
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            EventHandlers.CallOnItemBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            EventHandlers.CallOnItemDrag(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EventHandlers.CallOnItemEndDrag(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isEmpty) return;
            EventHandlers.CallOnItemHover(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!isUnlocked) return;
            EventHandlers.CallOnItemDroppedOn(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isEmpty) return;
            EventHandlers.CallOnItemEndHover(this);
        }
        #endregion
    }
}