using SunnyFarm.Game;
using SunnyFarm.Game.Inventory.Data;
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
