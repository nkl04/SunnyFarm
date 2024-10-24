using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Inventory.Data;
using SunnyFarm.Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggedItemCursor : MonoBehaviour
{
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private Sprite transparentSprite;
    [SerializeField] private TextMeshProUGUI quantityItemText;

    [HideInInspector] public InventoryItem InventoryItem;
    [HideInInspector] public bool IsEmpty => InventoryItem.isEmpty;

    private void Awake()
    {
        selectedItemImage.sprite = transparentSprite;

        quantityItemText.text = string.Empty;

        selectedItemImage.color = new Color(1, 1, 1, Constant.ColorStat.Alpha_1);
    }

    public void UpdateDraggedItemVisual()
    {
        SetUIVisual(InventoryItem);
    }

    public void SetUIVisual(InventoryItem inventoryItem)
    {
        ItemDetail itemDetails = ItemSystemManager.Instance.GetItemDetail(inventoryItem.itemID);

        if (itemDetails != null)
        {
            selectedItemImage.sprite = itemDetails.ItemImage;

            quantityItemText.text = inventoryItem.quantity > 1 ? inventoryItem.quantity.ToString() : string.Empty;
        }
        else
        {
            selectedItemImage.sprite = transparentSprite;
            quantityItemText.text = string.Empty;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
