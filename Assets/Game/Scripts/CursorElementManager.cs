using SunnyFarm.Game.Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorElementManager : MonoBehaviour
{
    [SerializeField] private SelectedItemCursor selectedItemCursor;
    [SerializeField] private DraggedItemCursor draggedItemCursor;
    [SerializeField] private UIInventoryDescription inventoryDescription;

    private Vector2 initInventoryDescriptionPosition;

    private void Start()
    {
        initInventoryDescriptionPosition = inventoryDescription.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if (!draggedItemCursor.IsEmpty)
        {
            inventoryDescription.gameObject.GetComponent<RectTransform>().anchoredPosition = initInventoryDescriptionPosition * 2;
        }
        else
        {
            inventoryDescription.gameObject.GetComponent<RectTransform>().anchoredPosition = initInventoryDescriptionPosition;
        }
    }
}
