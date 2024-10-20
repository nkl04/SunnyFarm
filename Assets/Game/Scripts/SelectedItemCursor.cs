using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Inventory.Data;
using SunnyFarm.Game.Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SunnyFarm.Game.Constant.Enums;

public class SelectedItemCursor : MonoBehaviour
{
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private Sprite transparentSprite;
    [SerializeField] private TextMeshProUGUI quantityItemText;

    [HideInInspector] public InventoryItem InventoryItem;

    private void Awake()
    {
        selectedItemImage.sprite = transparentSprite;

        quantityItemText.text = string.Empty;

        selectedItemImage.color = new Color(1, 1, 1, Constant.ColorStat.Alpha_08);

        EventHandlers.OnInventoryUpdated += UpdateUIInventory;
    }

    private void UpdateUIInventory(InventoryLocation location, InventoryItem[] inventoryItems)
    {
        if (location == InventoryLocation.Player)
        {

        }
    }

    public void SetData(string itemId, int quantity)
    {
        ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

        if (itemDetail != null && quantity > 0)
        {
            selectedItemImage.sprite = itemDetail.ItemImage;

            quantityItemText.text = quantity > 1 ? quantity.ToString() : "";
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
