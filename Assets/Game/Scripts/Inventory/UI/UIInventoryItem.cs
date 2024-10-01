namespace SunnyFarm.Game.Inventory.UI
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIInventoryItem : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Item's info")]
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemQuantity;
        [SerializeField] private RectTransform selectBox;

        [Header("Background Image")]
        [SerializeField] private Sprite unlockedBG;
        [SerializeField] private Image backgroundSlot;

        public InventoryLocation ItemLocation { get; private set; }

        private bool isEmpty = false; // serialize for test

        public int ItemIndex { get; set; }

        public event Action<UIInventoryItem> OnItemDroppedOn,
            OnItemBeginDrag, OnItemDrag, OnItemEndDrag, OnItemHover, OnItemEndHover;
        private void Awake()
        {

        }
        public void SetItemLocation(InventoryLocation location)
        {
            ItemLocation = location;
        }
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
            itemQuantity.text = quantity > 1 ? quantity.ToString() : "";
            isEmpty = image == null;
        }
        /// <summary>
        /// Reset data in item ui
        /// </summary>
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            itemQuantity.text = "";
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
            backgroundSlot.sprite = unlockedBG;
        }

        #region UI Events
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            OnItemDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isEmpty) return;

            OnItemHover?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isEmpty) return;
            OnItemEndHover?.Invoke(this);
        }
        #endregion
    }
}