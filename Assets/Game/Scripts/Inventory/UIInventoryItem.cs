using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    [Header("Item's info")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemQuantity;
    [SerializeField] private RectTransform selectBox;

    [Header("Background Image")]
    [SerializeField] private Sprite unlockedBG;
    [SerializeField] private Image backgroundSlot;

    private bool isEmpty;

    public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn,
        OnItemBeginDrag, OnItemDrag, OnItemEndDrag, OnItemHover;
    private void Awake()
    {
        ResetData();
        Deselect();
    }
    /// <summary>
    /// Set data to the ui, use it when having data from the db
    /// </summary>
    /// <param name="image"></param>
    /// <param name="quantity"></param>
    public void SetData(Sprite image, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = image;
        itemQuantity.text = quantity.ToString();
        isEmpty = false;
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
        if (!isEmpty) return;
        OnItemDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEmpty) return;
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isEmpty) return;
        OnItemClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEmpty) return;
        OnItemHover?.Invoke(this);
    }
    #endregion
}
