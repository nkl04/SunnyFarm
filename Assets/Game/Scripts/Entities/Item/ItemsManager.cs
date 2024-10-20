using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Tilemap;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private List<ItemController> itemPrefabs; // need to store in SO instead in component
    [SerializeField] private GridCursor gridCursor;

    private List<ItemController> itemControllers = new List<ItemController>();
    private ItemController itemUsing;

    // test
    [SerializeField] protected ItemDetail itemDetail;
    private void Start()
    {
        foreach (var controller in itemPrefabs)
        {
            ItemController tool = Instantiate(controller, transform);
            itemControllers.Add(tool);
        }

        PlayerAnimationTrigger.OnAnimationTrigger += HandleAnimationTrigger;
        PlayerAnimationTrigger.OnAnimationFinished += HandleAnimationFinished;

        ChangeItem(itemDetail);
    }
    public void ChangeItem(ItemDetail tool)
    {
        if (itemUsing != null && itemUsing.ItemType == tool.ItemType) return;
        foreach (var controller in itemControllers)
        {
            if (controller.ItemType == tool.ItemType)
            {
                controller.enabled = true;
                controller.SetUpDetail(tool);
                controller.SetUpCursor(gridCursor);
                controller.EnableController();
                itemUsing?.DisableController();
                itemUsing = controller;
            }
            else
                controller.enabled = false;
        }
    }
    public void ResetTool()
    {
        itemUsing = null;
    }
    private void HandleAnimationTrigger()
    {
        // Only the current tool's state processes the animation trigger
        itemUsing?.UseItem();
    }

    private void HandleAnimationFinished()
    {
        // test
        itemUsing?.ReactivateTool();
    }
}
