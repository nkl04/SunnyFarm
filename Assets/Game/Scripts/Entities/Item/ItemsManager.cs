using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using SunnyFarm.Game.Inventory.Data;
using SunnyFarm.Game.Managers;
using SunnyFarm.Game.Tilemap;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private GridCursor gridCursor;

    [SerializeField] private List<ItemController> itemControllers = new List<ItemController>();

    private ItemController _usingController;

    private Player player;

    private AnimationEventReceiver receiver;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        receiver = player.GetComponentInChildren<AnimationEventReceiver>();

        SetupToolAnimationEvents(receiver);

        EventHandlers.OnInventoryItemSelected += OnInventoryItemSelected;
    }

    private void OnInventoryItemSelected(InventoryKey key, string itemId)
    {
        ConfigItem itemDetail = ItemSystemManager.Instance.GetItemDetail(itemId);

        if (player.InventoryKey != key) return;

        if (itemDetail == null) return;

        SetSelectedItem(itemDetail);
    }

    public void SetSelectedItem(ConfigItem item)
    {

        _usingController = null;

        foreach (var controller in itemControllers)
        {
            controller.DisableController();

            if (controller.ItemType == item.ItemType)
            {
                controller.EnableController();
                controller.SetUpCursor(gridCursor);
                controller.SetUpDetail(item);
                _usingController = controller;
            }
        }
    }
    public void ResetTool()
    {
        _usingController = null;
    }
    private void SetupToolAnimationEvents(AnimationEventReceiver receiver)
    {
        AnimationEvent onUseEvent = new();
        onUseEvent.EventName = "OnUse";
        onUseEvent.OnAnimationEvent += () => _usingController?.UseItem();

        receiver.AddAnimationEvent(onUseEvent);

        AnimationEvent onFinishEvent = new();
        onFinishEvent.EventName = "OnFinish";
        onFinishEvent.OnAnimationEvent += () => _usingController?.ReactivateTool();

        receiver.AddAnimationEvent(onFinishEvent);
    }
}
