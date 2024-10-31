using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using SunnyFarm.Game.Inventory.Data;
using SunnyFarm.Game.Managers;
using SunnyFarm.Game.Tilemap;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private List<ItemController> itemPrefabs; // need to store in SO instead in component
    [SerializeField] private GridCursor gridCursor;

    private List<ItemController> itemControllers = new List<ItemController>();
    private ItemController itemUsing;

    private Player player;

    [SerializeField] protected ItemDetail itemDetail;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        foreach (var controller in itemPrefabs)
        {
            ItemController tool = Instantiate(controller, transform);

            itemControllers.Add(tool);
        }

        SetupToolAnimationEvents(player.GetComponentInChildren<AnimationEventReceiver>());

        EventHandlers.OnInventoryItemSelected += OnSelectedItem;
    }

    private void OnSelectedItem(InventoryKey key, string itemId)
    {
        if (key == player.InventoryKey)
        {
            ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

            if (itemDetail == null) return;

            SetItem(itemDetail);
        }
    }

    public void SetItem(ItemDetail tool)
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
    private void SetupToolAnimationEvents(AnimationEventReceiver receiver)
    {
        AnimationEvent onUseEvent = new();
        onUseEvent.EventName = "OnUse";
        onUseEvent.OnAnimationEvent += () => itemUsing?.UseItem();

        receiver.AddAnimationEvent(onUseEvent);

        AnimationEvent onFinishEvent = new();
        onFinishEvent.EventName = "OnFinish";
        onFinishEvent.OnAnimationEvent += () => itemUsing?.ReactivateTool();

        receiver.AddAnimationEvent(onFinishEvent);
    }
}
