using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item's info")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemQuantity;
    [SerializeField] private RectTransform selectBox;

    [Header("Background Image")]
    [SerializeField] private Sprite unlockedBG;
    [SerializeField] private Image backgroundSlot;

    private bool isEmpty;
    private bool isInteractable;

    public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn,
        OnItemBeginDrag, OnItemDrag;
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
        isInteractable = true;
    }

    #region UI Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty && !isInteractable) return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("lmao");
        if (!isInteractable) return;
        OnItemDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;
        OnItemClicked?.Invoke(this);
    }
    #endregion
}
